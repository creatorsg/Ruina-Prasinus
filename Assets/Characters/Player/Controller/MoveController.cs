using player2;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveController : MonoBehaviour
{
    [SerializeField] private PlayerState state;      
    [SerializeField] private InputComponent input;    
    [SerializeField] private MovementView view;
    [SerializeField] private PlayerStatus status;
    private MovementModel movemodel;
    private float moveInput;
    private bool dashRequested;
    void Awake()
    {
        movemodel = new MovementModel();

        input = GetComponent<InputComponent>();
        input.OnMove += dir => moveInput = dir;
        input.OnDash += () => dashRequested = true;

        view.SetModel(movemodel);
    }

    void Update()
    {
        bool dashHeld = Input.GetKey(KeyCode.LeftShift);
        movemodel.Update(moveInput, dashRequested, dashHeld, state, Time.deltaTime, status.isGrounded);
        dashRequested = false;
    }
}
