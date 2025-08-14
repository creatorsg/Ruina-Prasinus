using UnityEngine;

public class Enemy1Idle : State<Butterflymon>
{
    private float _detection;
    public Enemy1Idle(float detection)
    {
        _detection = detection;
    }
    public override void Enter(Butterflymon enemy1)
    {
        Debug.Log("idle상태 진입");
    }

    public override void Execute(Butterflymon enemy1)
    {
        if(enemy1.Enemy1MoveHandler.Distance < _detection)
        {
            enemy1.ChangeState(EnemyBehavior.Move);
        }
    }
    public override void FixedExecute(Butterflymon enemy1)
    {

    }
    public override void Exit(Butterflymon enemy1)
    {

    }
}
