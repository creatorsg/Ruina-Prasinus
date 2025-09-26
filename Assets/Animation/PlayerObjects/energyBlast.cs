using UnityEngine;

public class energyBlast : MonoBehaviour
{
    public Animator Animator;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            Animator.SetBool("hasHitWall", true);
    }
}
