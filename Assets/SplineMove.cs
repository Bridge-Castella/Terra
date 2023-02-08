using UnityEngine;

public class SplineMove : MonoBehaviour
{
	private int currentIndex = -1;
	private int predictedIndex = -1;
	private bool onLadder = false;

    private Vector2[] points;
    private float speed = 1.0f;
	private float distanceTrash = 1.0f;
	private LadderMovement.Range scope;

	private PlayerMove move;
	private Rigidbody2D rigid;
	private Animator animator;

	private void Start()
	{
		//Player 관련 변수들 초기화
		move = gameObject.GetComponent<PlayerMove>();
		rigid = gameObject.GetComponent<Rigidbody2D>();
		animator = gameObject.GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		//Ladder 위에 있는 경우에만 연산을 수행
		if (!onLadder) return;

		float verticalInput = Input.GetAxisRaw("Vertical");
		if (HandleInput(verticalInput)) return;

		//가장 가까운 point 탐색
		currentIndex = FindClosestPoint(/*currentIndex, predictedIndex*/);

		//목표지점을 설정
		predictedIndex = PredictWayPoint(verticalInput);

		//끝에 도달한 경우
		if (currentIndex == predictedIndex)
		{
			OnReachEnd();
			return;
		}

		//현재 player 위치
		Vector2 pos = transform.position;

		//목표지점으로의 방향 벡터
		Vector2 direction = points[predictedIndex] - pos;
		direction.Normalize();

		//방향 벡터에 기반, 가속도 설정
		rigid.velocity = speed * move.maxSpeed * direction;

		if (Mathf.Abs(direction.x) > 0.4
			&& ((direction.x > 0f) != move.facingRight))
			move.Flip();
	}

	public void Activate(Vector2[] points, float speed, float distanceTrash, LadderMovement.Range scope)
	{
		this.points = points;
		this.speed = speed;
		this.distanceTrash = distanceTrash;
		this.scope = scope;
		onLadder = true;
	}

	public void Deactivate()
	{
		points = null;
		onLadder = false;
		move.isLaddering = false;
		animator.SetBool("isLaddering", false);
		rigid.gravityScale = 10.0f;
	}

    //TODO: fix index narrowing
    private int FindClosestPoint(int begin = -1, int end = -1)
	{
		Vector2 pos = transform.position;
		float min_dist = float.MaxValue;
		int min_index = -1;

		int begin_idx = begin == -1 ? 0 : begin;
		int end_idx = end == -1 ? points.Length - 1 : end;
		if (begin_idx > end_idx)
			(begin_idx, end_idx) = (end_idx, begin_idx);

		for (int i = begin_idx; i < end_idx; i++)
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
		float speed = verticalInput * move.maxSpeed;

		//현재 속도 및 이전 deltaTime에 기반하여 이동가능 거리 예측
		float predicted_distance = Mathf.Abs(speed * Time.fixedDeltaTime);

		int idx_direction = verticalInput > 0.0f ? 1 : -1;
		int predicted_idx = currentIndex;

		Vector2 pos = transform.position;
		float path_distance = 0.0f;

		//이동가능 거리까지의 점을 목표지점을 설정
		while (path_distance < predicted_distance)
		{
			predicted_idx += idx_direction;

			if (predicted_idx < scope.begin || predicted_idx > points.Length + scope.end)
				return currentIndex;

			path_distance += Vector2.Distance(pos, points[predicted_idx]);
		}

		return predicted_idx;
	}

	private bool HandleInput(float verticalInput)
	{
		//입력 없을 경우
		if (verticalInput == 0.0f)
		{
			//점프시 탈출
			if (move.isJumping)
				ExitLaddering();

			//사다리타고 있는 동안 제자리
			if (move.isLaddering)
				FixMovement();

			return true;
		}
		else //입력이 존재할 때
		{
			//사다리타기 시작
			if (!move.isLaddering)
			{
				if (InitLaddering())
					return true;
			}

			//사다리타고 있는 동안 무중력
			rigid.gravityScale = 0.0f;
		}

		return false;
	}

	private bool InitLaddering()
	{
		currentIndex = FindClosestPoint();
		Vector2 closestPoint = points[currentIndex];

		//사다리와 충분히 가까운지 검사
		float dist = Vector2.Distance(transform.position, closestPoint);
		if (dist > distanceTrash)
			return true;

		//사다리방향으로 방향전환
		bool ladderFacingRight = closestPoint.x - transform.position.x > 0;
		if (ladderFacingRight != move.facingRight)
			move.Flip();

		//사다리에서 가장 가까운 위치로 이동
		transform.position = closestPoint;

		move.isLaddering = true;
		move.isJumping = false;
		animator.SetBool("isLaddering", true);

		return false;
	}

	private void ExitLaddering()
	{
		move.isLaddering = false;
		animator.SetBool("isLaddering", false);
	}

	private void FixMovement()
	{
		rigid.velocity = Vector2.zero;
		rigid.gravityScale = 0.0f;
	}

	private void OnReachEnd()
	{
		//하단 끝
		if (currentIndex < scope.begin + 2)
			ExitLaddering();
		//상단 끝
		else if (currentIndex > points.Length + scope.end - 2)
		{
			transform.position = points[points.Length + scope.end];
			if (!move.isJumping)
				FixMovement();
			else
				ExitLaddering();
		}
	}
}


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