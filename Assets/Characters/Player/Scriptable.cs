using UnityEngine;

public class Scriptable : MonoBehaviour
{
    private initialState PlayerState;
    private initialState copyData;
    void Start()
    {
        copyData = Instantiate(PlayerState);
    }



    void Update()
    {

    }
}
