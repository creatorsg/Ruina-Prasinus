using player2;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveController : MonoBehaviour
{
    [SerializeField] private PlayerState state;      
    [SerializeField] private InputComponent input;    
    [SerializeField] private MovementView view;
    [SerializeField] private PlayerStatus status;
    [SerializeField] private GravityController gravity;

    private Rigidbody2D rb;
    private MovementModel movemodel;
    private float moveInput;
    private bool dashRequested;
    void Awake()
    {
        movemodel = new MovementModel();

        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<InputComponent>();
        gravity = GetComponent<GravityController>();

        input.OnMove += dir => moveInput = dir;
        input.OnDash += () => dashRequested = true;

        view.SetModel(movemodel);
    }

    void Update()
    {
        bool dashHeld = Input.GetKey(KeyCode.LeftShift);
        bool jumpPressed = Input.GetKeyDown(KeyCode.Space);

        if (jumpPressed)
            gravity.Jump();

        movemodel.Update(moveInput, dashRequested, dashHeld, state, Time.deltaTime, status.isGrounded);
        dashRequested = false;
    }

    void FixedUpdate()
    { 
        float vx = movemodel.Velocity.x;
        float vy = gravity.VerticalVelocity;

        rb.linearVelocity = new Vector2(vx, vy);
    }
}
