using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class KeyRebindButton : MonoBehaviour, IPointerClickHandler
{
    public string actionName;
    private Text label;
    private bool isWaiting = false;

    void Awake()
    {
        label = GetComponentInChildren<Text>();
        RefreshLabel();
    }

    void Update()
    {
        if (!isWaiting) return;

        foreach (KeyCode keycode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keycode))
            {
                KeybindManager.SetKey(actionName, keycode);
                isWaiting = false;
                RefreshLabel();
                break;
            }
        }
    }

    public void OnPointerClick(PointerEventData e)
    {
        isWaiting = true;
        label.text = "...";
    }

    private void RefreshLabel()
    {
        label.text = KeybindManager.GetKeyCode(actionName).ToString();
    }
}
