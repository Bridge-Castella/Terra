using UnityEngine;

public class SplineMove : MonoBehaviour
{
	public enum SplineState
	{
		Idle,
		Entered,
		OnLadder,
		Climbing
	}

    private int currentIndex;
    private int predictedIndex;

	private Vector2[] points;
	private float speed = 1.0f;
	private float distanceTrash = 1.0f;

	private PlayerMove move;
	private Rigidbody2D rigid;
	private Animator animator;

	private SplineState state;

	private void Start()
	{
		//Player 관련 변수들 초기화
		move = gameObject.GetComponent<PlayerMove>();
		rigid = gameObject.GetComponent<Rigidbody2D>();
		animator = gameObject.GetComponent<Animator>();

		state = SplineState.Idle;
	}

	private void FixedUpdate()
	{
		//Ladder 위에 있는 경우에만 연산을 수행
		if (state == SplineState.Idle) return;

		float verticalInput = Input.GetAxisRaw("Vertical");
		ChangeState(verticalInput);

		if (state != SplineState.Climbing)
		{
			if (state == SplineState.Entered)
				ExitLaddering();
			else if (state == SplineState.OnLadder)
				FixMovement();
			return;
		}

		//가장 가까운 point 탐색
		//이전 위치 기반 index search range narrowing 적용
		currentIndex = FindClosestPoint(verticalInput);

		//목표지점을 설정
		predictedIndex = PredictWayPoint(verticalInput);

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

	private void ChangeState(float verticalInput)
	{
		//양끝에 도달했는지 검사
		int end_state = TestEnd();

		//입력 없을 경우
		if (verticalInput == 0.0f)
		{
			//점프시 탈출
			if (move.isJumping)
			{
				state = SplineState.Entered;
				return;
			}

			//가만히 멈춰있는 경우
			if (move.isLaddering)
			{
				state = SplineState.OnLadder;
				return;
			}
		}

        //입력이 존재할 때
        else
        {
			//reached below end, 아래 방향을 향할때
			if (end_state == 2 && verticalInput < 0f)
			{
				state = SplineState.Entered;
				return;
			}

			//reached top end, 위 방향을 향할때
			else if (end_state == 1 && verticalInput > 0f)
			{
                if (!move.isJumping)
                    state = SplineState.OnLadder;
                else
                    state = SplineState.Entered;
                return;
			}

			//사다리타기 시작
			if (!move.isLaddering)
			{
				if (!InitLaddering())
					return;
			}

			//사다리타고 있는 동안 무중력
			rigid.gravityScale = 0.0f;
			state = SplineState.Climbing;
		}
	}

    public void Activate(Vector2[] points, float speed, float distanceTrash)
	{
		this.points = points;
		this.speed = speed;
		this.distanceTrash = distanceTrash;
		this.state = SplineState.Entered;

	}

	public void Deactivate()
	{
		points = null;
		this.state = SplineState.Idle;
		ExitLaddering();
	}

	// 전체탐색, initladdering
	private (int, int) FindClosestPoint()
	{
		Vector2 pos = transform.position;
		(float, float) min_dist = (float.MaxValue, float.MaxValue);
		(int, int) min_index = (-1, 0);

		for (int i = 0; i < points.Length; i++)
		{
			float dist = Vector2.Distance(pos, points[i]);
			if (dist < min_dist.Item1)
			{
				min_dist.Item1 = dist;
				min_index.Item1 = i;
			}
			else if (dist < min_dist.Item2)
			{
				min_dist.Item2 = dist;
				min_index.Item2 = i;
			}
		}

		return min_index;
	}

	// 범위 탐색, fixedupdate
	private int FindClosestPoint(float verticalInput)
	{
		Vector2 pos = transform.position;
        int begin = currentIndex;
        int end = predictedIndex;
        if (begin > end)
            (begin, end) = (end, begin);

		if (begin > 0)
			begin -= 1;
		if (end < points.Length - 2)
			end += 1;

        float min_dist = Vector2.Distance(pos, points[begin]);
		int min_index = begin;

		for (int i = begin+1; i < end; i++)
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

			if (predicted_idx < 0)
				return 0;
			else if (predicted_idx > points.Length - 1)
				return points.Length - 1;

			path_distance += Vector2.Distance(pos, points[predicted_idx]);
			pos = points[predicted_idx];
		}

		return predicted_idx;
	}

	private bool InitLaddering()
	{
		var closest = FindClosestPoint();
		currentIndex = closest.Item1;
		predictedIndex = closest.Item2;

		// 방향벡터
		Vector2 diff = (points[closest.Item2] - points[closest.Item1]).normalized;

		// player로의 방향벡터
		Vector2 pos = transform.position;
		Vector2 player_dir = pos - points[closest.Item1];

		// 거리
		float distance = Vector2.Dot(diff, player_dir);

		// 원하는 위치 = 방향벡터 x 거리 + 방향벡터의 원점
		Vector2 closestPoint = (diff * distance) + points[closest.Item1];

		//사다리와 충분히 가까운지 검사
		float dist = Vector2.Distance(transform.position, closestPoint);
		if (dist > distanceTrash)
			return false;

		//사다리방향으로 방향전환
		bool ladderFacingRight = closestPoint.x - transform.position.x > 0;
		if (ladderFacingRight != move.facingRight)
			move.Flip();

		//사다리에서 가장 가까운 위치로 이동
		transform.position = closestPoint;

		move.isLaddering = true;
		move.isJumping = false;
		animator.SetBool("isLaddering", true);

		return true;
	}

	private void ExitLaddering()
	{
		if (!move.isLaddering) return;
		move.isLaddering = false;
		animator.SetBool("isLaddering", false);
		rigid.gravityScale = 10.0f;
	}

	private void FixMovement()
	{
		rigid.velocity = Vector2.zero;
		rigid.gravityScale = 0.0f;
	}

	private int TestEnd()
	{
		Vector2 pos = transform.position;

		// Reach top end
		if (Vector2.Distance(pos, points[points.Length - 1]) < 0.25f)
			return 1;

		// Reach below end
		else if (move.IsGrounded())
			return 2;

		// Non reached
		else
			return 0;
	}
}