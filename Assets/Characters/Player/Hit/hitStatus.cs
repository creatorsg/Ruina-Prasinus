using Unity.VisualScripting;
using UnityEngine;

public class hitStatus
{
    public bool isHit {  get; private set; }
    public bool isInvincible { get; private set; }

    private float hitTimer, invincibleTimer;

    private void UpdateHit(Collider hitcolider, Collider enemyattackcolider)
    {

    }

    private void OnTriggerEnter2D(Collider hitcolider, Collider enemyattackcolider, initialState state)
    {
        if (!isHit && !isInvincible)
        {
            if (hitcolider.CompareTag("Bullet"))
            {
                state.hp -= 10;
                isHit = true;
            }
        }
    }

}
