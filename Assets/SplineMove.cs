
//using UnityEngine;
//#if UNITY_EDITOR
//using UnityEditor.U2D;
//#endif

//using UnityEngine.U2D;
////{
//public class SplineMove : MonoBehaviour
//{

//    bool bPointMove = false;
//    int currentTargetIndex = 1;
//    private GameObject playerTerra = null;
//    Vector3[] controlPoints;
//    int Direction = 1;
//    float vertical;
//    public float LadderSpeed = 3.5f;


//    void Start()
//    {
//        SpriteShapeController spriteShapeController = gameObject.GetComponent<SpriteShapeController>();
//        // Get the Spline component of the Sprite Shape Controller
//        Spline spline = spriteShapeController.spline;

//        // Create an array to store the control points
//        controlPoints = new Vector3[spriteShapeController.spline.GetPointCount()];

//        // Get the control points of the spline and store them in the array
//        for (int i = 0; i < controlPoints.Length; i++)
//        {
//            controlPoints[i] = spline.GetPosition(i) + gameObject.transform.position;
//        }
//    }

//    void FixedUpdate()
//    {
//        if (bPointMove && null != playerTerra)
//        {
//            vertical = Input.GetAxis("Vertical");
//            if (Mathf.Abs(vertical) > 0f)
//            {
//                float inputDirection = (vertical);
//                if (inputDirection > 0)
//                {
//                    //currentTargetIndex -=1;
//                    if (Direction == -1)
//                    {
//                        if (currentTargetIndex < controlPoints.Length - 1)
//                            currentTargetIndex += 1;
//                        else
//                            currentTargetIndex = controlPoints.Length - 1;

//                        Direction = 1;
//                    }
//                    if (null != playerTerra)
//                        playerTerra.transform.position = Vector3.Lerp(playerTerra.transform.position, controlPoints[currentTargetIndex], Time.deltaTime * LadderSpeed);
//                }
//                else
//                {
//                    if (Direction == 1)
//                    {
//                        if (currentTargetIndex >= 1)
//                            currentTargetIndex -= 1;
//                        else
//                            currentTargetIndex = 0;

//                        Direction = -1;
//                    }
//                    if (null != playerTerra)
//                        playerTerra.transform.position = Vector3.Lerp(playerTerra.transform.position, controlPoints[currentTargetIndex], Time.deltaTime * LadderSpeed);
//                }


//            }
//            else
//            {

//            }


//            CheckArrivePoint();
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {

//        if (other.tag == "Player")
//        {
//            MoveToControlPoint(other.gameObject);
//            Debug.Log("TriggerOn");
//            //Terra 무중력 On
//        }
//    }

//    private void OnTriggerExit2D(Collider2D other)
//    {
//        if (other.tag == "Player")
//        {
//            Debug.Log("TriggerOFF");
//            PlayerMove playerMoveScript = playerTerra.GetComponent<PlayerMove>();
//            playerMoveScript.fallGravityMultiflier = 1.0f;
//            bPointMove = false;
//            playerMoveScript.isLaddering = false;
//            //Terra 무중력 On
//        }
//    }

//    private void FindNearestGoal()
//    {
//        if (playerTerra)
//        {
//            int index = 0;
//            float minDist = 999999;
//            for (int i = 0; i < controlPoints.Length; i++)
//            {
//                float distanceGoalandTerra = Vector3.Distance(playerTerra.transform.position, controlPoints[i]);
//                if (minDist >= distanceGoalandTerra)
//                {
//                    minDist = distanceGoalandTerra;
//                    index = i;
//                }
//            }
//            currentTargetIndex = index;
//        }
//    }

//    private void MoveToControlPoint(GameObject player)
//    {
//        playerTerra = player;
//        //currentTargetIndex = 1;

//        PlayerMove playerMoveScript = playerTerra.GetComponent<PlayerMove>();
//        playerMoveScript.fallGravityMultiflier=0.0f;
//        bPointMove = true;
//        playerMoveScript.isLaddering = true;
//        FindNearestGoal();
//    }

//    private void CheckArrivePoint()
//    {
//        Debug.Log(currentTargetIndex);
//        if (0 <= currentTargetIndex && currentTargetIndex <= controlPoints.Length)
//        {
//            float dist = Vector3.Distance(playerTerra.transform.position, controlPoints[currentTargetIndex]);
//            if (dist <= 1.0f)
//            {
//                currentTargetIndex = currentTargetIndex + (Direction * 1);
//                if (controlPoints.Length <= currentTargetIndex)
//                {
//                    currentTargetIndex = 0;
//                    bPointMove = false;
//                    Debug.Log("Arrive");

//                    //Terra 무중력 Off

//                    PlayerMove playerMoveScript = playerTerra.GetComponent<PlayerMove>();
//                    playerMoveScript.fallGravityMultiflier=10.0f;
//                    playerMoveScript.isLaddering = false;
//                    playerMoveScript.Jump();

//                    return;
//                }
//                Debug.Log("Clear" + (currentTargetIndex - 1) + controlPoints[currentTargetIndex]);
//            }
//        }
//    }
//}
////}


using UnityEngine;

public class SplineMove : MonoBehaviour
{
	private Vector2[] points;
	private int currentIndex;
	private bool onLadder = false;
	public float LadderSpeed = 1.0f;

	private PlayerMove pm;
	private Rigidbody2D rb;
	private Animator pa;

	private void Start()
	{
		//Player 관련 변수들 초기화
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		pm = player.GetComponent<PlayerMove>();
		rb = player.GetComponent<Rigidbody2D>();
		pa = player.GetComponent<Animator>();

		//Spline points 위치 초기화
		points = gameObject.GetComponent<EdgeCollider2D>().points;
		Vector2 pos = transform.position;
		for (int i = 0; i < points.Length; i++)
		{
			points[i] += pos;
		}
	}

	private void FixedUpdate()
	{
		//Ladder 위에 있는 경우에만 연산을 수행
		if (!onLadder) return;

		if (handleInput()) return;

		//가장 가까운 point 탐색
		currentIndex = FindClosestPoint();

		//목표지점을 설정
		float verticalInput = Input.GetAxisRaw("Vertical");
		int waypoint_idx = PredictWayPoint(verticalInput);

		// 현재 위치와 목표지점이 같은 경우, 행동X
		if (currentIndex == waypoint_idx)
			return;

		//현재 player 위치
		Vector2 pos = pm.transform.position;

		//목표지점으로의 방향 벡터
		Vector2 direction = points[waypoint_idx] - pos;
		direction.Normalize();

		//방향 벡터에 기반, 가속도 설정
		rb.velocity = LadderSpeed * pm.maxSpeed * direction;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player")) return;

		onLadder = true;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player")) return;

		onLadder = false;
		pm.isLaddering = false;
		pa.SetBool("isLaddering", false);
		rb.gravityScale = 10.0f;
	}

	private int FindClosestPoint()
	{
		Vector2 pos = rb.transform.position;
		float min_dist = float.MaxValue;
		int min_index = -1;

		for (int i = 0; i < points.Length; i++)
		{
			float dist = Vector2.Distance(pos, points[i]);
			if (dist < min_dist)
			{
				min_dist = dist;
				min_index = i;
			}
		}

		return min_index;
	}

	//예측된 이동가능 거리를 기반으로 다음 목표지점을 설정
	//이동가능 거리 = 속력 x 시간(이전 deltaTime)
	//속력 = 입력값 x 속력(PlayerMove.maxSpeed)
	private int PredictWayPoint(float verticalInput)
	{
		//입력값을 통해 현재 속도 측정
		float speed = verticalInput * pm.maxSpeed;

		//현재 속도 및 이전 deltaTime에 기반하여 이동가능 거리 예측
		float predicted_distance = Mathf.Abs(speed * Time.fixedDeltaTime);

		int idx_direction = verticalInput > 0.0f ? 1 : -1;
		int predicted_idx = currentIndex;

		Vector2 pos = pm.transform.position;
		float path_distance = 0.0f;

		//이동가능 거리까지의 점을 목표지점을 설정
		while (path_distance < predicted_distance)
		{
			predicted_idx += idx_direction;
			if (predicted_idx < 0 || predicted_idx > points.Length - 1)
				return currentIndex;

			path_distance += Vector2.Distance(pos, points[predicted_idx]);
		}

		return predicted_idx;
	}

	private bool handleInput()
	{
		//Get input
		float verticalInput = Input.GetAxisRaw("Vertical");
		float horizontalInput = Input.GetAxisRaw("Horizontal");
		bool jumpInput = Input.GetButtonDown("Jump");

		if (verticalInput == 0.0f)
		{
			if (pm.isJumping)
			{
				pm.isLaddering = false;
				pa.SetBool("isLaddering", false);
			}

			if (pm.isLaddering)
			{
				rb.velocity = Vector2.zero;
				rb.gravityScale = 0.0f;
			}
			return true;
		}
		else
		{
			if (!pm.isLaddering)
			{
				pm.isLaddering = true;
				pm.isJumping = false;
				pa.SetBool("isLaddering", true);
			}
			rb.gravityScale = 0.0f;
		}

		return false;
	}
}