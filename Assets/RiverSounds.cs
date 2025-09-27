using FMODUnity;
using UnityEngine;

public class RiverSounds : MonoBehaviour
{
    [SerializeField] EventReference _riverSound;

    private void Awake()
    {
        RuntimeManager.PlayOneShotAttached(_riverSound, gameObject);
    }
}
