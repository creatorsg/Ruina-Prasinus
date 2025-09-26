using UnityEngine;
using UnityEngine.SceneManagement;

public class Die : State<MainPlayer>
{
    private float _timer;
    public override void Enter(MainPlayer player)
    {
        player.PlayerHpHandler.Explode();
    }

    public override void Execute(MainPlayer player)
    {
        if(_timer > 0.8f)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
    public override void FixedExecute(MainPlayer player)
    {
        _timer += Time.deltaTime;
    }
    public override void Exit(MainPlayer player)
    {

    }
}
