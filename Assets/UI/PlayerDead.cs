using UnityEngine;

public class PlayerDead : MonoBehaviour
{
    [SerializeField] private GameObject deadUIPanel; // 비활성화된 UI 패널
        
    private void Awake()
    {
        if (deadUIPanel != null)
            deadUIPanel.SetActive(false);
    }

    // 🔹 이 메서드가 반드시 필요합니다!

    public void ShowDeadUI()
    {
        Debug.Log("ShowDeadUI 실행됨");
        if (deadUIPanel != null)
        {
            deadUIPanel.SetActive(true);
            Debug.Log($"deadUIPanel 상태: {deadUIPanel.activeSelf}");
        }
        else
        {
            Debug.LogError("deadUIPanel이 Inspector에 연결 안 됨!");
        }
    }


}
