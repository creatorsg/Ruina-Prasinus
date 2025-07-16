    using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerState))]
public class JangpungManager : MonoBehaviour
{
    [SerializeField] private Jangpung jangpung;
    private PlayerState playerState;
    private bool isOnCooldown = false;
    private int facingDirection = 1;

    void Awake()
    {
        playerState = GetComponent<PlayerState>();
    }

    void Update()
    {
        UpdateFacingDirection();

        if (!isOnCooldown && InputManager.GetKeyDown("Attack"))
        {
            Vector2 dir = CalculateLaunchDirection();
            LaunchProjectile(dir);
            StartCoroutine(Cooldown());
        }
    }

    private void UpdateFacingDirection()
    {
        if (InputManager.GetKeyDown("MoveLeft")) facingDirection = -1;
        else if (InputManager.GetKeyDown("MoveRight")) facingDirection = 1;
    }

    private Vector2 CalculateLaunchDirection()
    {
        if (InputManager.GetKey("LookUP"))
            return Vector2.up;

        if (InputManager.GetKey("LieDown") && !playerState.IsGrounded)
            return (Vector2.down + (facingDirection == 1 ? Vector2.right : Vector2.left)).normalized;

        return facingDirection == 1 ? Vector2.right : Vector2.left;
    }

    private void LaunchProjectile(Vector2 direction)
    {
        GameObject obj = Instantiate(jangpung.projectilePrefab, transform.position, Quaternion.identity);
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
