using Unity.VisualScripting;
using UnityEngine;

public class OBSTACLE : EnemyAttackHandler
{
    private bool _action;
    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        Attack(10f);
    }
}

