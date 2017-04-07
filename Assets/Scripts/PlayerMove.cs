using UnityEngine;

public enum TouchDir
{
    None,
    Left,
    Right,
    Top,
    Bottom
}

public class PlayerMove : MonoBehaviour
{
    public AudioSource footLand;
    public float speed = 10;
    public float horizontalMoveSpeed = 0.2f;

    public bool isSliding = false;
    public float slideTime = 1.4f / 2f;

    public bool isJumping = false;
    public float jumpHeight = 20;
    public float jumpSpeed = 10;

    public float minTouchLength = 50;
    public int nowLaneIndex = 1;//当前赛道Index
    public int targetLaneIndex = 1;

    private float slideTimer = 0;
    private bool isUp = true;
    private Transform prisoner;
    private Animation anim;
    private float hasJumpHeight = 0;
    private EnvGenerator env;
    private Vector3 moveDownPos = Vector3.zero;
    private float moveHorizontal = 0;//换赛道水平移动距离

    void Awake()
    {
        env = Camera.main.GetComponent<EnvGenerator>();
        prisoner = transform.Find("Prisoner").transform;
        anim = prisoner.GetComponent<Animation>();
    }

    void Update()
    {
        if (GameController.gameState == GameState.Playing)
        {
            Vector3 pos = transform.position;
            Vector3 nextWayPoint = env.forest1.GetNextWayPoint();
            nextWayPoint = new Vector3(nextWayPoint.x + GameController.xOffsets[targetLaneIndex], nextWayPoint.y, nextWayPoint.z);
            Vector3 dir = nextWayPoint - transform.position;
            Vector3 moveDir = dir.normalized * speed * Time.deltaTime;
            transform.position += moveDir;
            transform.rotation = Quaternion.LookRotation(new Vector3(nextWayPoint.x, transform.position.y, nextWayPoint.z) - transform.position, Vector3.up);

            if (targetLaneIndex != nowLaneIndex)
            {
                float move = moveHorizontal * horizontalMoveSpeed;
                moveHorizontal -= moveHorizontal * horizontalMoveSpeed;
                transform.position = new Vector3(transform.position.x + move, transform.position.y, transform.position.z);
                if (Mathf.Abs(moveHorizontal) < 0.5f)
                {
                    transform.position = new Vector3(transform.position.x + moveHorizontal, transform.position.y, transform.position.z);
                    nowLaneIndex = targetLaneIndex;
                }
            }
            if (isJumping)
            {
                float yMove = jumpSpeed * Time.deltaTime;
                if (isUp)
                {
                    prisoner.position = new Vector3(prisoner.position.x, prisoner.position.y + yMove, prisoner.position.z);
                    hasJumpHeight += yMove;
                    if (Mathf.Abs(jumpHeight - hasJumpHeight) < 0.5f)
                    {
                        prisoner.position = new Vector3(prisoner.position.x, prisoner.position.y + jumpHeight - hasJumpHeight, prisoner.position.z);
                        isUp = false;
                        hasJumpHeight = jumpHeight;
                    }
                }
                else
                {
                    prisoner.position = new Vector3(prisoner.position.x, prisoner.position.y - yMove, prisoner.position.z);
                    hasJumpHeight -= yMove;
                    if (Mathf.Abs(hasJumpHeight - 0) < 0.5f)
                    {
                        prisoner.position = new Vector3(prisoner.position.x, prisoner.position.y - (hasJumpHeight - 0), prisoner.position.z);
                        hasJumpHeight = 0;
                        isJumping = false;
                        footLand.Play();
                    }
                }
            }

            if (isSliding)
            {
                slideTimer += Time.deltaTime;
                if (slideTimer > slideTime)
                {
                    isSliding = false;
                    slideTimer = 0;
                }
            }

            MoveControll();

        }
    }

    void MoveControll()
    {
        TouchDir touchDir = GetTouchDir();
        switch (touchDir)
        {
            case TouchDir.None:
                break;
            case TouchDir.Right:
                if (targetLaneIndex < 2)
                {
                    targetLaneIndex++;
                    moveHorizontal = 14;
                }
                break;
            case TouchDir.Left:
                if (targetLaneIndex > 0)
                {
                    targetLaneIndex--;
                    moveHorizontal = -14;
                }
                break;
            case TouchDir.Top:
                if (isJumping == false)
                {
                    isJumping = true;
                    isUp = true;
                    hasJumpHeight = 0;
                }
                break;
            case TouchDir.Bottom:
                if (!isJumping)
                {
                    isSliding = true;
                }
                break;
        }

    }

    TouchDir GetTouchDir()
    {
        if (Input.GetMouseButtonDown(0))
        {
            moveDownPos = Input.mousePosition;
            return TouchDir.None;
        }
        if (Input.GetMouseButtonUp(0) && moveDownPos != Vector3.zero)
        {
            Vector3 moveOffset = Input.mousePosition - moveDownPos;
            if (Mathf.Abs(moveOffset.y) > Mathf.Abs(moveOffset.x) && moveOffset.y > minTouchLength)
            {
                return TouchDir.Top;
            }

            if (Mathf.Abs(moveOffset.y) > Mathf.Abs(moveOffset.x) && moveOffset.y < -minTouchLength)
            {
                return TouchDir.Bottom;
            }

            if (Mathf.Abs(moveOffset.y) < Mathf.Abs(moveOffset.x) && moveOffset.x > minTouchLength)
            {
                return TouchDir.Right;
            }

            if (Mathf.Abs(moveOffset.y) < Mathf.Abs(moveOffset.x) && moveOffset.x < -minTouchLength)
            {
                return TouchDir.Left;
            }
        }
        return TouchDir.None;
    }
}
