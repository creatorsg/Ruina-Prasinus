using UnityEngine;

public class Attack : State<MainPlayer>
{
    private float attackMode, attackTimer, exitTimer;
    private enum Phase { attack1, attack2 }
    private Phase _currentPhase;

    public override void Enter(MainPlayer player)
    {
        attackMode = 0;
        attackTimer = 0;
        _currentPhase = Phase.attack1;
    }

    public override void Execute(MainPlayer player)
    {
        exitTimer += Time.deltaTime;

        switch (_currentPhase)
        {
            case Phase.attack1:
                if (attackTimer == 0)
                    player.footsound.PlayAttackSound(attackMode);

                attackTimer += Time.deltaTime;

                if (InputManager.GetKeyDown("Attack") && attackTimer <= 1f)
                {
                    attackMode = 1;
                    _currentPhase = Phase.attack2;
                    attackTimer = 0;
                    exitTimer = 0;
                }

                break;

            case Phase.attack2:
                if (attackTimer == 0)
                    player.footsound.PlayAttackSound(attackMode);

                attackTimer += Time.deltaTime;

                if (InputManager.GetKeyDown("Attack") && attackTimer <= 1f)
                {
                    attackMode = 0;
                    _currentPhase = Phase.attack1;
                    attackTimer = 0;
                    exitTimer = 0;
                }
                break;
        }

        if (exitTimer > 1.5f)
        {
            player.ChangeEventState(EventBehavior.None);
        }
    }
    public override void FixedExecute(MainPlayer player)
    {

    }
    public override void Exit(MainPlayer player)
    {

    }
}
