using UnityEngine;

namespace Runtime.Utilities.Common
{
    public class MainCameraCull : MonoBehaviour
    {
        [field: SerializeField] string _layerName;
        public void CullLayer(bool onoff)
        {
            Camera camera = Camera.main;
            if (onoff)
            {
                camera.cullingMask &= ~(1 << LayerMask.NameToLayer(_layerName));
            }
            else
            {
                camera.cullingMask |= (1 << LayerMask.NameToLayer(_layerName));
            }
        }
    }
}