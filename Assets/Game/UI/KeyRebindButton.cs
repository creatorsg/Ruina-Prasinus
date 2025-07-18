using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class KeyRebindButton : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("KeybindManager�� ��ϵ� �׼� �̸��� �����ϰ� �Է�")]
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

        foreach (KeyCode kc in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kc))
            {
                KeybindManager.SetKey(actionName, kc);
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
