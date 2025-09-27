using FMODUnity;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class RiverSounds : MonoBehaviour
{
    [SerializeField] EventReference _riverSound;

    private void Awake()
    {
        RuntimeManager.PlayOneShotAttached(_riverSound, gameObject);
    }
}
