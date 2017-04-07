using UnityEngine;

public enum PlayerState
{
    Idle,
    Running,
    MoveLeft,
    MoveRight,
    Jumping,
    Sliding,
    Death
}

public class PlayerAnimation : MonoBehaviour
{
    public Animation anim;

    private PlayerState state = PlayerState.Idle;
    private PlayerMove playerMove;
    private AudioSource footStep;
    private bool havePlayDeath = false;

    void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        footStep = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (GameController.gameState == GameState.Menu)
        {
            state = PlayerState.Idle;
        }
        else if (GameController.gameState == GameState.Playing)
        {
            state = PlayerState.Running;
            if (playerMove.nowLaneIndex != playerMove.targetLaneIndex)
            {
                if (playerMove.nowLaneIndex < playerMove.targetLaneIndex)
                {
                    state = PlayerState.MoveRight;
                }
                else
                {
                    state = PlayerState.MoveLeft;
                }
            }
            else if (playerMove.isJumping)
            {
                state = PlayerState.Jumping;
            }
            else if (playerMove.isSliding)
            {
                state = PlayerState.Sliding;
            }
        }
        else if (GameController.gameState == GameState.End)
        {
            state = PlayerState.Death;
        }
    }

    void LateUpdate()
    {
        switch (state)
        {
            case PlayerState.Idle:
                PlayIdle();
                break;
            case PlayerState.Running:
                PlayRunning();
                break;
            case PlayerState.MoveLeft:
                PlayLeft();
                break;
            case PlayerState.MoveRight:
                PlayRight();
                break;
            case PlayerState.Jumping:
                PlayJump();
                break;
            case PlayerState.Sliding:
                PlaySlide();
                break;
            case PlayerState.Death:
                PlayDeath();
                break;
        }
        if (state == PlayerState.Running || state == PlayerState.MoveRight || state == PlayerState.MoveLeft)
        {
            if (!footStep.isPlaying)
            {
                footStep.Play();
            }
        }
        else
        {
            footStep.Stop();
        }
    }

    void PlayRunning()
    {
        if (!anim.IsPlaying("run"))
        {
            anim.Play("run");
        }
    }

    void PlayIdle()
    {
        if (!anim.IsPlaying("Idle_1") && !anim.IsPlaying("Idle_2"))
        {
            anim.Play("Idle_1");
            anim.PlayQueued("Idle_2");
        }
    }

    void PlayRight()
    {
        if (!anim.IsPlaying("right"))
        {
            anim.Play("right");
        }
    }

    void PlayLeft()
    {
        if (!anim.IsPlaying("left"))
        {
            //anim["left"].speed = 2;
            anim.Play("left");
        }
    }

    void PlayJump()
    {
        if (!anim.IsPlaying("jump"))
        {
            anim.Play("jump");
        }
    }

    void PlaySlide()
    {
        if (!anim.IsPlaying("slide"))
        {
            anim["slide"].speed = 2f;
            anim.Play("slide");
        }
    }

    void PlayDeath()
    {
        if (havePlayDeath == false)
        {
            havePlayDeath = true;
            anim.Play("death");
        }
    }
}
