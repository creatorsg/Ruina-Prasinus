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

        // 1) 전체 시간 멈추기
        Time.timeScale = 0f;
        // 2) 플레이어 입력 비활성화 (예: PlayerController 스크립트)
        PlayerController.Instance.enabled = false;

        // 3) 저장 UI 보여주기
        saveMenuUI.SetActive(true);
    }

    public void CloseSaveMenu()
    {
        if (!isPaused) return;
        isPaused = false;

        // 1) 시간 다시 흐르게
        Time.timeScale = 1f;
        // 2) 플레이어 입력 활성화
        PlayerController.Instance.enabled = true;

        // 3) 저장 UI 숨기기
        saveMenuUI.SetActive(false);
    }
}
