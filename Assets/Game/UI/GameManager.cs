using Player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    public GameObject saveMenuUI; // Canvas > SaveMenu Panel

    private bool isPaused = false;

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    public void OpenSaveMenu()
    {
        if (isPaused) return;
        isPaused = true;

        // 1) ��ü �ð� ���߱�
        Time.timeScale = 0f;
        // 2) �÷��̾� �Է� ��Ȱ��ȭ (��: PlayerController ��ũ��Ʈ)
        PlayerController.Instance.enabled = false;

        // 3) ���� UI �����ֱ�
        saveMenuUI.SetActive(true);
    }

    public void CloseSaveMenu()
    {
        if (!isPaused) return;
        isPaused = false;

        // 1) �ð� �ٽ� �帣��
        Time.timeScale = 1f;
        // 2) �÷��̾� �Է� Ȱ��ȭ
        PlayerController.Instance.enabled = true;

        // 3) ���� UI �����
        saveMenuUI.SetActive(false);
    }
}
