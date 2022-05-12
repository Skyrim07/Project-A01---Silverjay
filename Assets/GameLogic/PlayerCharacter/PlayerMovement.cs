using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

using SKCell;

public sealed class PlayerMovement : MonoBehaviour, IPlayerModule
{
    public bool active = true;

    const float GRAVITY = 9.8f;
    const string RUN_BOOL = "Run";
    const string JUMP_START_TRIGGER = "Jump";
    const string JUMP_END_TRIGGER = "JumpEnd";
    const string TAG_GROUND = "Ground";
    const string TAG_SLOPE = "Slope";

    const float MOVE_SPEED_X_MULTIPLIER = 100;
    const float JUMP_SCALE_MULTIPLIER = 1000;

    public int jumpLimit = 1;
    public float moveSpeed = 5f;
    public float jumpForce = 2f;

    public Transform visualTF;
    public Animator visualAnim;

    public Vector3 velo_Input;
    public RaycastHit2D groundHit;


    private float hAxis, vAxis, hAxisRaw;
    private float oGravity;
    private Rigidbody2D rb;
    private Collider2D cld;
    private CinemachineFramingTransposer vcTransposer;
    private Vector3 lastPos, oScale;
    private Vector2 velo_InputMove, velo_Jump;
    private float oCameraY;
    private float lookTimer;
    private int dir = 1;
    private int jumpCount = 0;
    public bool lastIsGround, isGround;
    public bool canDash=true;

    private bool combatMovement;
    private Vector2 combatMovementSpeed;
    
    public void Initialize()
    {
        lastPos =transform.position;
        rb = GetComponent<Rigidbody2D>();
        cld = GetComponent<Collider2D>();
        oScale = visualTF.localScale;
        oGravity = rb.gravityScale;

        RuntimeData.player_FacingDirection = -1; //SetPlayerInitalFacingDirection;

        EventDispatcher.Dispatch(EventDispatcher.Player, EventRef.PLAYER_MOVEMENT_MODULE_START);

        EventDispatcher.AddListener(EventDispatcher.Player, EventRef.PLAYER_ON_ATTACK_MOVEMENT_START, new SJEvent(() =>
        {
            combatMovement = true;
        }));

        EventDispatcher.AddListener(EventDispatcher.Player, EventRef.PLAYER_ON_ATTACK_MOVEMENT_END, new SJEvent(() =>
        {

            combatMovement = false;

        }));

        EventDispatcher.AddListener(EventDispatcher.Common, EventRef.CM_ON_SCENE_LOADED, new SJEvent(() =>
        {
            if(!GlobalLibrary.G_SCENE_SPECIFICS[RuntimeData.activeSceneTitle].disablePlayerGravity)
            SetGravity(oGravity);
        }));
        EventDispatcher.AddListener(EventDispatcher.Common, EventRef.CM_ON_SCENE_EXIT, new SJEvent(() =>
        {
            SetGravity(0);
        }));

        CommonUtils.InvokeAction(0.2f, LateInitialize);
    }

    private void LateInitialize()
    {
        vcTransposer = CommonReference.instance.mainVC.GetCinemachineComponent<CinemachineFramingTransposer>();
        oCameraY = vcTransposer.m_ScreenY;
    }
    public void SetState(bool active)
    {
        this.active = active;
        if(!active)
        rb.velocity = Vector3.zero;
    }
    public void Tick(float unscaledDeltaTime, float deltaTime)
    {
        if (!active)
            return;

        HandleMovement();
        HandleTurn();
        HandleJump();
        HandleVelocity();
        HandleGroundDetection();
        HandleLook();

        RuntimeData.player_PositionDelta = transform.position - lastPos;
        RuntimeData.player_Position = transform.position;
        lastPos = transform.position;
    }

    void HandleVelocity()
    {
        if (!combatMovement)
        {
            Vector2 velocity = Vector2.zero;
            velocity += velo_InputMove;
            velocity.y = rb.velocity.y;
            rb.velocity = velocity;
        }
        else
        {
            
            rb.velocity = combatMovementSpeed;
        }
        
    }
    void HandleGroundDetection()
    {
        //Do not detect when jumping
        if (RuntimeData.player_PositionDelta.y > 0)
        {
            return;
        }

        RaycastHit2D hit = Physics2D.BoxCast(cld.bounds.center, Vector2.one, 0, Vector2.down, 1, 1<<12);
        groundHit = hit;
        if (hit)
        {
            isGround = hit.transform.tag.Equals(TAG_GROUND) || hit.transform.tag.Equals(TAG_SLOPE);
            if (hit.transform.tag.Equals(TAG_SLOPE))
            {
                RuntimeData.player_IsOnSlope = true;
                if (RuntimeData.player_HasMoveInput)
                {
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                }
                else if (!RuntimeData.player_IsInJump)
                {
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation|RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX;
                }
            }
            else
            {
                RuntimeData.player_IsOnSlope = false;
            }
        }
        else
        {
            isGround = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            RuntimeData.player_IsOnSlope = false;
        }

        if (!lastIsGround && isGround)
            OnTouchGround();
        lastIsGround = isGround;
    }

    void HandleMovement()
    {
        if (!CheckCanMove())
        {
            return;
        }

        hAxis = Input.GetAxis("Horizontal");
        hAxisRaw = Input.GetAxisRaw("Horizontal");
        velo_Input = new Vector3(hAxis * Time.fixedDeltaTime * moveSpeed, 0, 0);

        //Fast Run

        if (canDash && isGround)
        {
            float fastMoveSpd = 1.0f;
            if (Input.GetKeyDown(GlobalLibrary.INPUT_FAST_RUN_KEYCODE))
            {
                EventDispatcher.Dispatch(EventDispatcher.Player, EventRef.PLAYER_ON_START_DASH);
            }
            if (Input.GetKey(GlobalLibrary.INPUT_FAST_RUN_KEYCODE))
            {
                fastMoveSpd = GlobalLibrary.G_PLAYER_FAST_RUN_SPD_BONUS;
                RuntimeData.player_IsDashing = hAxisRaw != 0;
            }
            if (Input.GetKeyUp(GlobalLibrary.INPUT_FAST_RUN_KEYCODE))
            {
                EventDispatcher.Dispatch(EventDispatcher.Player, EventRef.PLAYER_ON_END_DASH);
                RuntimeData.player_IsDashing = false;
            }
            velo_Input.x *= fastMoveSpd;
        }
        else
        {
            if (RuntimeData.player_IsDashing)
            {
                EventDispatcher.Dispatch(EventDispatcher.Player, EventRef.PLAYER_ON_END_DASH);
            }
            RuntimeData.player_IsDashing = false;
        }

        velo_InputMove = new Vector2(velo_Input.x * MOVE_SPEED_X_MULTIPLIER, 0);
        visualAnim.SetBool(RUN_BOOL, hAxis != 0);

        RuntimeData.player_HasMoveInput = hAxisRaw != 0;
    }

    bool CheckCanMove()
    {
        bool canMove = true;
        canMove &= !RuntimeData.player_IsLooking;

        return canMove;
    }

    bool CheckCanJump()
    {
        bool canMove = true;
        canMove &= !RuntimeData.player_IsLooking;
        canMove &= jumpCount < jumpLimit;
        return canMove || isGround;
    }
    void HandleLook()
    {
        // Can only look when on ground & static
        if (isGround && RuntimeData.player_PositionDelta.x==0 && RuntimeData.player_PositionDelta.y == 0)
        {
            if(Input.GetKeyDown(GlobalLibrary.INPUT_DOWN_KEYCODE) || Input.GetKeyDown(GlobalLibrary.INPUT_UP_KEYCODE))
            {
                lookTimer = 0;
            }
            if (Input.GetKey(GlobalLibrary.INPUT_DOWN_KEYCODE) || Input.GetKey(GlobalLibrary.INPUT_UP_KEYCODE))
            {
                if(!RuntimeData.player_IsLooking)
                    lookTimer += Time.fixedDeltaTime;
                if (lookTimer >= GlobalLibrary.INPUT_PLAYER_LOOK_DOWN_HOLD_TIME)
                {
                    float vRaw = Input.GetAxisRaw("Vertical");
                    SetCameraLook(GlobalLibrary.G_PLAYER_LOOK_DOWN_CAM_Y_DELTA * vRaw);
                    RuntimeData.player_IsLooking = true;
                    lookTimer = 0;
                }
            }
            if (Input.GetKeyUp(GlobalLibrary.INPUT_DOWN_KEYCODE) || Input.GetKeyUp(GlobalLibrary.INPUT_UP_KEYCODE))
            {
                SetCameraLook(0, 0.4f);
                RuntimeData.player_IsLooking = false;
            }
        }
        else if (RuntimeData.player_IsLooking)
        {
            SetCameraLook(0, 0.4f);
            RuntimeData.player_IsLooking = false;
        }
    }

    void SetCameraLook(float yDelta, float dampTime = 0.7f)
    {
        float curY = vcTransposer.m_ScreenY;
        CommonUtils.StartProcedure(SKCurve.QuinticIn, dampTime, (f) =>
        {
            vcTransposer.m_ScreenY = curY+(oCameraY + (yDelta) - curY)* f;
        });
    }
    void HandleTurn()
    {
        if (hAxis > 0)
        {
            SetOrientation(false);
            RuntimeData.player_FacingDirection = 1;
        }
            
        else if (hAxis < 0)
        {
            SetOrientation(true);
            RuntimeData.player_FacingDirection = -1;
        }
            
    }

    public void SetOrientation(bool isOne)
    {
        visualTF.localScale = new Vector3((isOne?1:-1) * oScale.x, oScale.y, oScale.z);
        
    }

    void HandleJump()
    {

        //Downward jump
        if (isGround && groundHit && groundHit.collider)
        {
            if (Input.GetKey(GlobalLibrary.INPUT_DOWN_KEYCODE))
            {
                if (Input.GetKeyDown(GlobalLibrary.INPUT_JUMP_KEYCODE))
                {
                    PlatformEffector2D pe = groundHit.collider.GetComponent<PlatformEffector2D>();
                    if (pe != null)
                    {
                        Collider2D cld = groundHit.collider;
                        cld.enabled = false;
                        CommonUtils.InvokeAction(GlobalLibrary.G_PLAYER_DOWN_PLATFORM_EFFECTOR_DISABLE_TIME, () =>
                        {
                            cld.enabled = true;
                        });
                        return;
                    }
                }
            }
        }

        //Forward jump
        if (Input.GetKeyDown(GlobalLibrary.INPUT_JUMP_KEYCODE))
        {
            if (!CheckCanJump())
            {
                return;
            }

            //Unfreeze player if jumps on slope
            if (RuntimeData.player_IsOnSlope)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            visualAnim.ResetTrigger(JUMP_END_TRIGGER);
            visualAnim.ResetTrigger(JUMP_START_TRIGGER);
            visualAnim.SetTrigger(JUMP_START_TRIGGER);

            rb.velocity.Set(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpForce * JUMP_SCALE_MULTIPLIER));

            jumpCount++;
            if(jumpCount==jumpLimit)
                EventDispatcher.Dispatch(EventDispatcher.Player, EventRef.PLAYER_ON_REACH_MAX_JUMP);

            EventDispatcher.Dispatch(EventDispatcher.Player, EventRef.PLAYER_ON_INPUT_JUMP);
            RuntimeData.player_IsInJump = true;
        }
    }


    void OnTouchGround()
    {
        visualAnim.ResetTrigger(JUMP_END_TRIGGER);
        visualAnim.SetTrigger(JUMP_END_TRIGGER);

        EventDispatcher.Dispatch(EventDispatcher.Player, EventRef.PLAYER_ON_TOUCH_GROUND);

        rb.velocity.Set(rb.velocity.x, 0);
        jumpCount = 0;
        RuntimeData.player_IsInJump = false;
    }

    public void SetGravity(float val)
    {
        rb.gravityScale = val;
    }

   

    public void SetPlayerVelocity(float velocity)
    {

        combatMovementSpeed = new Vector2 (RuntimeData.player_FacingDirection * velocity, 0);

    }
}
