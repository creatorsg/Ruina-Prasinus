using UnityEngine;

public class DashEffect : MonoBehaviour
{
    [SerializeField] private Animator Animator;

    private void Start()
    {
        PlayEffect();
    }

    private void PlayEffect()
    {
        Destroy(gameObject, 0.5f);
    }
}
