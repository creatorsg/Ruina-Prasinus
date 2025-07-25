using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Room_Manager : MonoBehaviour
{
    [Header("������ ScriptableObject")]
    [SerializeField] private Map map;

    [Header("��Ż �Ŵ��� ������Ʈ")]
    [SerializeField] private GameObject portals;

    private bool isCleared = false;

    void Start()
    {
        if (portals != null)
            portals.SetActive(false);
        else
            Debug.LogWarning("portals ������ �����ϴ�!");
    }

    void Update()
    {
        // ���� ���� �� ������ ������ �ʿ�� ����, Ŭ���� ���Ǹ� üũ
        if (!isCleared && map.enemy_num <= 0f)
        {
            isCleared = true;
            // �ʿ��ϴٸ� ScriptableObject���� ���� �ݿ�
            map.open_portal = true;
            Debug.Log("�� Ŭ����! ��Ż�� �� �غ� �Ϸ�.");
        }
    }

    // �÷��̾ �� �Ա�(Trigger Collider)�� ���� �� ȣ��
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCleared && other.CompareTag("Player"))
        {
            portals.SetActive(true);
            Debug.Log("�÷��̾� ����: ��Ż Ȱ��ȭ");
        }
    }
}
