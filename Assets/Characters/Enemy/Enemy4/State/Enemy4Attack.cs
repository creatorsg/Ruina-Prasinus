using UnityEngine;

public class Enemy4Attack : State<FlowerCannon>
{
    public override void Enter(FlowerCannon enemy4)
    {

    }

    public override void Execute(FlowerCannon enemy4)
    {
        if(!enemy4.AttackHandler.IsCooltime && enemy4.PlayerDetectHandler.PlayerHit)
        {
            enemy4.AttackHandler.ShootBullet();
            enemy4.AttackHandler.CoolTimer();
        }
    }
    public override void FixedExecute(FlowerCannon enemy4)
    {

    }
    public override void Exit(FlowerCannon enemy4)
    {

    }
}
