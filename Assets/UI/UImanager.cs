using UnityEngine;

public class UImanager : MonoBehaviour
{
    [SerializeField] private playerHpHandler playerHpHandler; // 플레이어 HP 관리 스크립트
    [SerializeField] private HealthBar healthBar;             // HP 바 UI
    [SerializeField] private PlayerDead playerDead;           // 죽음 UI 관리 스크립트

    private void Start()
    {
        // HP 바랑 연결
        healthBar.Bind(playerHpHandler);

        // 죽음 이벤트 구독
        playerHpHandler.OnPlayerDead += HandlePlayerDead;
    }

    private void OnDestroy()
    {
        // 안전하게 이벤트 해제
        playerHpHandler.OnPlayerDead -= HandlePlayerDead;
    }

    private void HandlePlayerDead()
    {
        Debug.Log("HandlePlayerDead 호출됨");
        // PlayerDead 스크립트에 신호 전달
        playerDead.ShowDeadUI();
    }
}
