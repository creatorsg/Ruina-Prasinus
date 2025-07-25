using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private GameObject optionsPanel;  // OptionsPanel 연결

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        optionsPanel.SetActive(false);
    }

    public void ToggleOptionsMenu()
    {
        optionsPanel.SetActive(!optionsPanel.activeSelf);
        Time.timeScale = optionsPanel.activeSelf ? 0f : 1f; // 게임 일시정지/재개
    }

    void Update()
    {
        if (InputManager.GetKeyDown("Pause"))
            ToggleOptionsMenu();
    }
}
