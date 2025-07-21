using UnityEngine;

public class Scriptable : MonoBehaviour
{
    private PlayerState PlayerState;
    private PlayerState copyData;
    void Start()
    {
        copyData = Instantiate(PlayerState);
    
    }



    void Update()
    {

    }
}
