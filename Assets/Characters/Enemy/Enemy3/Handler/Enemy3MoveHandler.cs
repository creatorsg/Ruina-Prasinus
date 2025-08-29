using UnityEngine;

public class Enemy3MoveHandler : MonoBehaviour
{
    private PurpleMushroom _enemy3;
    private float _moveDirection;

    public float MoveDirection => _moveDirection;

    public void Initialize(PurpleMushroom enemy3)
    {
        _enemy3 = enemy3;
    }
    
    public void CalculMoveDirection()
    {
        _moveDirection = transform.localScale.x > 0 ? 1 : -1;
    }

    public void ChangeDirection()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y,transform.localScale.z);
    }
}
