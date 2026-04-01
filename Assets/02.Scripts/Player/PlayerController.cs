using UnityEngine;
using Utils.ClassUtility;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private float moveSpeed = 5f;

    [Header("Joystick Logic")]
    private Vector2 moveDirection;        // 최종 이동 방향 벡터
    private Vector2 touchStartPosition;   // 처음 터치한 위치
    private Vector2 currentTouchPosition; // 현재 드래그 중인 위치

    // 공격 방향을 저장
    public Vector2 LookDirection = Vector2.right;

    private bool isMoving = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        InputHandler();
        MovePlayer();
    }

    private void InputHandler()
    {
        // 클릭/터치 시작 (처음 핸드폰을 터치한 곳 설정)
        if (Input.GetMouseButtonDown(0))
        {
            isMoving = true;
            touchStartPosition = Input.mousePosition;
            UIManager.Instance.ShowJoystick(touchStartPosition);
        }

        // 드래그 중 (손가락을 당기는 방향 계산)
        if (Input.GetMouseButton(0) && isMoving)
        {
            Vector2 currentPos = Input.mousePosition;
            UIManager.Instance.UpdateHandle(touchStartPosition, currentPos);

            Vector2 dragVector = currentPos - touchStartPosition;
            if (dragVector.magnitude > 0.1f)
            {
                moveDirection = dragVector.normalized;
                // 마지막으로 바라본 방향(공격 방향) 저장
                LookDirection = moveDirection;
            }
        }

        // 터치 종료
        if (Input.GetMouseButtonUp(0))
        {
            isMoving = false;
            moveDirection = Vector2.zero;
            UIManager.Instance.HideJoystick();
        }
    }

    private void MovePlayer()
    {
        if (moveDirection != Vector2.zero)
        {
            if (moveDirection.x != 0)
                spriteRenderer.flipX = moveDirection.x < 0;

            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    // 에디터에서 터치 범위를 시각적으로 확인하기 위함
    private void OnDrawGizmos()
    {
        if (isMoving)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(touchStartPosition, currentTouchPosition);
        }
    }
}