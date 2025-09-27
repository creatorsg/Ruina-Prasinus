using UnityEngine;
using UnityEngine.SceneManagement;

public class Die : State<MainPlayer>
{
    private float _timer;
    public override void Enter(MainPlayer player)
    {
        Debug.Log("Die ÁøÀÔ");
        _timer = 0;
        player.PlayerHpHandler.Explode();
    }

    public override void Execute(MainPlayer player)
    {
        _timer += Time.deltaTime;

        if (_timer >= 0.8f)
        {
            player.ChangeEventState(EventBehavior.None);
        }
    }
    public override void FixedExecute(MainPlayer player)
    {

    }
    public override void Exit(MainPlayer player)
    {
        SceneManager.LoadScene("GameOver");
    }
}
