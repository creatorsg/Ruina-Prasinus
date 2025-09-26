using UnityEngine;

public class ExplodeEffectHandler : MonoBehaviour
{
    public void ShootAtPlayer()
    {
        // 필요시 공격 로직
        Debug.Log("ShootAtPlayer called!");
    }

    public void OnShootEnd()
    {
        // Destroy 직전 처리 등
        Debug.Log("OnShootEnd called!");
    }
}
