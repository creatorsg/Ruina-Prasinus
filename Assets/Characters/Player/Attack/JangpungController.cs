using Player;
using System.Collections;
using UnityEngine;

public class JangpungController : MonoBehaviour
{
    [SerializeField] private Jangpung jangpung;
    private MoveStatusHandler status;
    private bool isOnCooldown = false;
    private int facingDirection = 1;

    public event System.Action OnAttack;
    public event System.Action<bool> OnLookUp;
    public event System.Action<bool> OnLieDown;

    public bool _isAttack;


    void Awake()
    {
        status = new MoveStatusHandler();
    }

    void Update()
    {
        UpdateFacingDirection();

        bool isDown = Input.GetKey(KeyCode.DownArrow);
        OnLieDown?.Invoke(isDown);

        bool isUp = Input.GetKey(KeyCode.UpArrow);
        OnLookUp?.Invoke(isUp);



        if (!isOnCooldown && Input.GetKeyDown(KeyCode.X))
        {
            Vector2 dir = CalculateLaunchDirection();
            LaunchProjectile(dir);
            StartCoroutine(Cooldown());
            OnAttack?.Invoke();
        }
    }

    private void UpdateFacingDirection()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) facingDirection = -1;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) facingDirection = 1;
    }

    private Vector2 CalculateLaunchDirection()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            return Vector2.up;
        if (Input.GetKey(KeyCode.DownArrow) && !status.CanJump)
        {
            
            return (Vector2.down + (facingDirection == 1 ? Vector2.right : Vector2.left)).normalized;
        }

        return facingDirection == 1 ? Vector2.right : Vector2.left;  
    }

    private void LaunchProjectile(Vector2 direction)
    {
        Vector2 dir = CalculateLaunchDirection();
        _isAttack = true;
        GameObject obj = Instantiate(jangpung.projectilePrefab, transform.position, Quaternion.identity);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        obj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (obj.TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.linearVelocity = direction * jangpung.speed;
        }
    }

    private IEnumerator Cooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(jangpung.standbyTime);
        isOnCooldown = false;
    }
}
