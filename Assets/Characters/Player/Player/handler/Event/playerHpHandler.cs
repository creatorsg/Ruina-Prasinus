using System.Collections;
using Player;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class playerHpHandler : FindChildObject
{
    private MainPlayer _player;
    private Transform _playerbody;
    private float knockbackForce = 1f;
    PolygonCollider2D _hitbox;
    private SpriteRenderer spriteRenderer;
    private bool _isHeating = false, _isInvicible = false;
    private float _hp;

    public bool IsHeating => _isHeating;

    public PolygonCollider2D Hitbox => _hitbox;
    public void Initialize(MainPlayer player, float hp)
    {
        _player = player;
        _hp = hp;
    }

    public void Awake()
    {
        _playerbody = FindChildWithTag(transform, "PlayerBody");
        spriteRenderer = _playerbody.GetComponent<SpriteRenderer>();

        _hitbox = gameObject.GetComponentInChildren<PolygonCollider2D>();
        _playerbody = FindChildWithTag(transform, "PlayerBody");
        _isHeating = false;
        _isInvicible = false;

        _hp = 50;
    }

    private void Update()
    {
        if (_isHeating)
        {
            _player.ChangeMoveState(MoveBehavior.Idle);
            Debug.Log(_hp);
        }

        if(_hp <= 0)
        {
            _player.ChangeEventState(EventBehavior.Die);
        }
    }


    public void Damaged(float damage, Transform attacker)
    {
        if (_isHeating == false && _isInvicible == false)
        {
            _hp -= damage;
            _isHeating = true;

            _player.ChangeMoveState(MoveBehavior.Idle);

            float damagejump = _player.MoveStatusHandler.CanJump ? 3f : 5f;
            _player.Rigidbody2D.linearVelocity = new Vector2(0f, damagejump);

            StartCoroutine(HitRoutine());
        }
    }

    private IEnumerator HitRoutine()
    {
        StartCoroutine(Blink());
        yield return new WaitForSeconds(0.2f);
        _isInvicible = true;
        _isHeating = false;

        yield return new WaitForSeconds(0.5f);
        _isInvicible = false;
    }

    public IEnumerator Blink()
    {
        Color originalColor = spriteRenderer.color;
        originalColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        while (_isHeating || _isInvicible)
        {
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.3f);
            yield return new WaitForSeconds(0.1f);

            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
        spriteRenderer.color = originalColor;
    }
}
