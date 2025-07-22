using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class newMovementController : MonoBehaviour
{
    [SerializeField] private PlayerState state;
    [SerializeField] private InputComponent input;
    [SerializeField] private newPlayerView view;
    
    private PlayerStatus2 status;
    private Modeldeldel model;

    [HideInInspector] public RaycastHit2D hit, fronthit;
    private bool dashRequested, jumpRequested;
    public Transform Player, frontCheck;
    private float moveInput;
    public float checkRadius;
    private float distance = 1;
    private LayerMask groundMask;

    void Awake()
    {
        model = new Modeldeldel();
        status = new PlayerStatus2();
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
        view.Move(moveInput, model.moveSpeed, status.perp);

        if (jumpRequested && status.isGround)
        {
            view.Jump();
        }
    }

    void Update()
    {
        bool dashHeld = InputManager.GetKey("Dash");

        hit = Physics2D.Raycast(Player.position, Vector2.down, distance, groundMask);
        fronthit = Physics2D.Raycast(frontCheck.position, transform.right, 0.1f, groundMask);

        status.Update(Player, checkRadius, groundMask);
        model.Update(moveInput, dashRequested, dashHeld, state, Time.deltaTime, status.isGround);

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
