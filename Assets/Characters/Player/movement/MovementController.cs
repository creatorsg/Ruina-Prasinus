using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class MovementController : MonoBehaviour
{
    [SerializeField] private initialState state;
    [SerializeField] private InputComponent input;
    [SerializeField] private MovePlayerView view;
    
    private MoveStatus status;
    private ModelMove model;

    [HideInInspector] public RaycastHit2D hit, fronthit;
    private bool dashRequested, jumpRequested;
    public Transform Player, frontCheck;
    private float moveInput;
    public float checkRadius;
    private float distance = 1;
    private LayerMask groundMask;

    void Awake()
    {
        model = new ModelMove();
        status = new MoveStatus();
        groundMask = LayerMask.GetMask("Ground");
        input = GetComponent<InputComponent>();

        input.OnMove += dir => moveInput = dir;
        input.OnDash += () => dashRequested = true;
        input.OnJump += () => jumpRequested = true;

        view.SetModel(model);
        view.SetState(state);
        view.SetStatus(status);

    }

    private void FixedUpdate()
    {
        view.Move(moveInput, Time.deltaTime);
    }

    void Update()
    {
        bool dashHeld = InputManager.GetKey("Dash");

        hit = Physics2D.Raycast(Player.position, Vector2.down, distance, groundMask);
        fronthit = Physics2D.Raycast(frontCheck.position, transform.right, 0.1f, groundMask);

        status.Update(Player, checkRadius, groundMask);
        model.Update(moveInput, dashRequested, dashHeld, state, Time.deltaTime, status.isGround);

        if (jumpRequested && status.isGround)
        {
            view.Jump();
        }

        if (hit || fronthit)
        {
            if (fronthit)
                status.SlopeCheck(fronthit);
            else if (hit)
                status.SlopeCheck(hit);
        }

        dashRequested = false;
        jumpRequested = false;
    }
}
