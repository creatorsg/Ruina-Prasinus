using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Room_Manager : MonoBehaviour
{
    [Header("참조할 ScriptableObject")]
    [SerializeField] private Map map;

    [Header("포탈 매니저 오브젝트")]
    [SerializeField] private GameObject portals;

    private bool isCleared = false;

    void Start()
    {
        if (portals != null)
            portals.SetActive(false);
        else
            Debug.LogWarning("portals 참조가 없습니다!");
    }

    void Update()
    {
        // 몬스터 수를 매 프레임 갱신할 필요는 없고, 클리어 조건만 체크
        if (!isCleared && map.enemy_num <= 0f)
        {
            isCleared = true;
            // 필요하다면 ScriptableObject에도 상태 반영
            map.open_portal = true;
            Debug.Log("방 클리어! 포탈을 열 준비 완료.");
        }
    }

    // 플레이어가 방 입구(Trigger Collider)에 들어올 때 호출
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCleared && other.CompareTag("Player"))
        {
            portals.SetActive(true);
            Debug.Log("플레이어 입장: 포탈 활성화");
        }
    }
}
