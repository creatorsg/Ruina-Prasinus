using UnityEngine;
using UnityEditor;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject mainOptionPanel;
    [SerializeField] private GameObject keySettingsPanel;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        optionsPanel.SetActive(false);
        keySettingsPanel.SetActive(false);
    }

    void Update()
    {
        if (InputManager.GetKeyDown("Pause"))
            ToggleOptionsMenu();
    }

    public void ToggleOptionsMenu()
    {
        bool isOpen = !optionsPanel.activeSelf;
        optionsPanel.SetActive(isOpen);
        if (!isOpen && keySettingsPanel.activeSelf)
            keySettingsPanel.SetActive(false);
            mainOptionPanel.SetActive(true);
        Time.timeScale = isOpen ? 0f : 1f;
    }

    public void ResumeGame()
    {
        ToggleOptionsMenu();
    }

    public void OpenKeySettings()
    {
        keySettingsPanel.SetActive(true);
        mainOptionPanel.SetActive(false);
        
    }

    public void CloseKeySettings()
    {
        keySettingsPanel.SetActive(false);
        mainOptionPanel.SetActive(true);
    }
    public void QuitGame()
    {
        Time.timeScale = 1f;  
        Application.Quit();

    }
}
