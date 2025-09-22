using UnityEngine;
using UnityEngine.SceneManagement;

public class Die : State<MainPlayer>
{
    public override void Enter(MainPlayer player)
    {
        SceneManager.LoadScene("GameOver");
    }

    public override void Execute(MainPlayer player)
    {

    }
    public override void FixedExecute(MainPlayer player)
    {

    }
    public override void Exit(MainPlayer player)
    {

    }
}
