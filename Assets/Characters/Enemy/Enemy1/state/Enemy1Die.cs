using UnityEngine;

public class Enemy1Die : State<Butterflymon>
{
    public override void Enter(Butterflymon enemy1)
    {
        Debug.Log("죽음 상태 진입");
        enemy1.Enemy1SpawnHandler.CheckDestroyed();
    }

    public override void Execute(Butterflymon enemy1)
    {

    }
    public override void FixedExecute(Butterflymon enemy1)
    {
       
    }
    public override void Exit(Butterflymon enemy1)
    {
        
    }
}
