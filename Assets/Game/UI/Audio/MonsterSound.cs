using FMODUnity;
using UnityEngine;

public class MonsterSound : MonoBehaviour
{
    [SerializeField] EventReference explodeSound;
    [SerializeField] EventReference HitSound;

    public void PlayExplodeSound()
    {
        RuntimeManager.PlayOneShotAttached(explodeSound, gameObject);
    }

    public void PlayHitSound()
    {
        RuntimeManager.PlayOneShotAttached(HitSound, gameObject);
    }
}
