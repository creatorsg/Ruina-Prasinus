using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Actor : UITransitionBase
{
    [Header("스프라이트 저장 테이블")]
    [SerializeField] string[] _keys = new string[] { };
    [SerializeField] Sprite[] _imageSource = new Sprite[] { };
    private Dictionary<string, Sprite> _imageTable = new Dictionary<string, Sprite>();

    private void Awake()
    {
        _imageTable =
        _keys.Zip(_imageSource, (k, v) => (k, v)).ToDictionary(x => x.k, x => x.v);
    }

    public void Initialize(string name, string gesture)
    {
        transform.name = name;

        if (!_imageTable.ContainsKey(gesture))
            return;

        Vector2 size = new Vector2(_imageTable[gesture].rect.width, _imageTable[gesture].rect.height);
        _rectTransform.sizeDelta = size;

        SetImage(_imageTable[gesture]);

        Position(TransitionType.Instant, 0f, Vector3.zero, new Vector3(-860f, -330f, 0f));
        Scale(TransitionType.Instant, 0f, Vector3.zero, new Vector3(0.85f, 0.85f, 1f));
        SetColor(TransitionType.Instant, 0f, Color.white, new Color(0.5f, 0.5f, 0.5f, 1f));
    }

    public void SetGesture(string key)
    {
        if (!_imageTable.ContainsKey(key))
            return;

        Vector2 size = new Vector2(_imageTable[key].rect.width, _imageTable[key].rect.height);
        _rectTransform.sizeDelta = size;

        SetImage(_imageTable[key]);
    }

    public void Enter()
    {
        Position(TransitionType.Smooth, 0.2f, new Vector3(-1380f, -330f, 0f), new Vector3(-860f, -330f, 0f));
        Scale(TransitionType.Smooth, 0.2f, new Vector3(0.85f, 0.85f, 1f), new Vector3(0.85f, 0.85f, 1f));
        SetColor(TransitionType.Lerp, 0.2f, new Color(0f, 0f, 0f, 0f), new Color(0.5f, 0.5f, 0.5f, 1f));
    }

    public void Exit()
    {
        Position(TransitionType.Smooth, 0.2f, SelfPosition, new Vector3(-1380f, -330f, 0f));
        Scale(TransitionType.Smooth, 0.2f, SelfScale, new Vector3(0.85f, 0.85f, 1f));
        SetColor(TransitionType.Lerp, 0.2f, SelfColor, new Color(0f, 0f, 0f, 0f));
        Destroy(gameObject, 0.2f);
    }

    public void SetActive()
    {
        Position(TransitionType.Smooth, 0.2f, SelfPosition, new Vector3(-830f, -365f, 0f));
        Scale(TransitionType.Smooth, 0.2f, SelfScale, new Vector3(1f, 1f, 1f));
        SetColor(TransitionType.Lerp, 0.2f, SelfColor, new Color(1f, 1f, 1f, 1f));
    }

    public void SetDeactive()
    {
        Position(TransitionType.Smooth, 0.2f, SelfPosition, new Vector3(-860f, -330f, 0f));
        Scale(TransitionType.Smooth, 0.2f, SelfScale, new Vector3(0.85f, 0.85f, 1f));
        SetColor(TransitionType.Lerp, 0.2f, SelfColor, new Color(0.5f, 0.5f, 0.5f, 1f));
    }
}