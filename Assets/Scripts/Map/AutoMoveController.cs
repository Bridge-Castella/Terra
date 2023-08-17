using System.Collections;
using UnityEngine;

public class AutoMoveController : MonoBehaviour
{
    [SerializeField] AutoMoveTrigger[] triggers;

    public bool IsMoving { get; private set; }

    private PlayerMove player;
    private Rigidbody2D playerRigid;
    private int indexCounter = 1;

    private bool hasReachedEnd = false;
    private bool hasFadeEnded = false;
    private Vector3 playerHeight;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        playerHeight = new Vector3(0, player.GetComponent<CapsuleCollider2D>().size.y / 2, 0);
        playerRigid = player.GetComponent<Rigidbody2D>();
        triggers = GetComponentsInChildren<AutoMoveTrigger>();

        _ResetActivation();
	}

    public void StartAutoMoveChain(AutoMoveTrigger trigger)
    {
        if (IsMoving)
        {
            Debug.LogWarning("Could not play two chains at the smae time. " +
                "Only the early one will be played.");
            return;
        }

        StartCoroutine(_PerformAutoMoveChain(trigger));
    }

    public void MoveNext()
    {
        hasReachedEnd = true;
    }

    private IEnumerator _PerformAutoMoveChain(AutoMoveTrigger start)
    {
        IsMoving = true;

        // this will deactivate all the other triggers
        _SelectCurrentChain(start);

        var current = start;
        do
        {
            // reset variables
            // maybe this should go to the end of the do-while loop..?
            current.gameObject.SetActive(true);
            hasReachedEnd = false;
            hasFadeEnded = false;

            if (current.Waypoint.position == current.StartPoint.position)
            {
                Debug.LogWarning("Start point and waypoint seems same. " +
                    "This may cause non responsive and unmoveable player. " +
                    "Please check the startpoint and waypoint in " + current.name + '\n' +
                    "Map: " + MapManager.state.map.ToString() + ",  " +
                    "AutoMoveController: " + name + ",  " +
                    "AutoMoveTrigger: " + current.name);
            }

            // perform actions
            _PerformFade(current.Operation, current.OperationDuration);
            _PerformMove(current.StartPoint, current.Waypoint);

            // wait for actions to be finished
            yield return _PerformWait(current);

            // move to next node(trigger)
            current.gameObject.SetActive(false);
            current = current.Next;
        }
        // we should exit the chain when it is end or it is recursive
        while (current != null && !current.StartNode);

        _ResetActivation();
        IsMoving = false;
    }

    private void _PerformMove(Transform StartPoint, Transform Waypoint)
    {
        player.prefferedDirection = Waypoint.position - StartPoint.position;
        player.isAutoMove = true;
    }

    private void _PerformFade(AutoMoveTrigger.AutoMoveOperation operation, float duration)
    {
        SceneFader.instance.OnFadeEnd += () =>
        {
            hasFadeEnded = true;
        };

        switch (operation)
        {
            case AutoMoveTrigger.AutoMoveOperation.FadeIn:
                SceneFader.instance.FadeIn(duration);
                break;

            case AutoMoveTrigger.AutoMoveOperation.FadeOut:
                SceneFader.instance.FadeOut(duration);
                break;

            case AutoMoveTrigger.AutoMoveOperation.None:
                break;
        }
    }

    private IEnumerator _PerformWait(AutoMoveTrigger current)
    {
        switch (current.Operation)
        {
            // do nothing, just wait for player to reach the waypoint
            case AutoMoveTrigger.AutoMoveOperation.None:
                while (!hasReachedEnd)
                    yield return null;
                break;

            // wait for fade than teleport the player
            case AutoMoveTrigger.AutoMoveOperation.FadeIn:
                while (!hasFadeEnded)
                {
                    // stay still on early reach
                    if (hasReachedEnd)
                        player.prefferedDirection = Vector3.zero;
                    yield return null;
                }

                var positionToMove = current.Next == null ?
                    // send player to way point
                    // which is the position of ground + height of player
                    current.Waypoint.position + playerHeight :
                    // send player to next start point
                    current.Next.StartPoint.position + playerHeight;

                // teleport player
                player.transform.position = positionToMove;

                // wait for the scene fader delay
                while (SceneFader.instance.Active)
                    yield return null;
                break;

            // wait for fade to end, and then wait for player to reach the waypoint
            case AutoMoveTrigger.AutoMoveOperation.FadeOut:
                while (!hasFadeEnded)
                {
                    // stay still on early reach
                    if (hasReachedEnd)
                        player.prefferedDirection = Vector3.zero;
                    yield return null;
                }

                while (!hasReachedEnd)
                    yield return null;

                break;
        }
    }

    private void _SelectCurrentChain(AutoMoveTrigger current)
    {
        foreach (var trigger in triggers)
        {
            if (trigger == current)
            {
                trigger.gameObject.SetActive(true);
            }
            else
            {
                trigger.gameObject.SetActive(false);
            }
        }
    }

    private void _ResetActivation()
    {
        player.isAutoMove = false;
        foreach (var trigger in triggers)
        {
            trigger.Init(this, playerRigid, indexCounter++);
            if (trigger.StartNode)
            {
                trigger.gameObject.SetActive(true);
            }
            else
            {
                trigger.gameObject.SetActive(false);
            }
        }
    }
}
