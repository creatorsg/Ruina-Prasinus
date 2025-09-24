using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UITransitionBase : MonoBehaviour
{
    // 자기 자신의 정보를 반환.
    public float SelfPosX => _rectTransform.anchoredPosition.x;
    public float SelfPosY => _rectTransform.anchoredPosition.y;

    public Vector2 SelfPosition => _rectTransform.anchoredPosition;
    public Vector3 SelfWorldPosition => _rectTransform.position;
    public Vector3 SelfRotation => _rectTransform.rotation.eulerAngles;
    public Vector3 SelfScale => _rectTransform.localScale;

    public Vector2 SelfArea => _rectTransform.sizeDelta;

    public Color SelfColor => _image.color;

    //  컴포넌트 저장.
    [SerializeField] protected RectTransform _rectTransform;
    [SerializeField] protected Image _image;
    [SerializeField] protected CanvasGroup _canvasGroup;

    // 트랜지션 객체 생성
    private IEnumerator _moveTransition;
    private IEnumerator _positionTransition;
    private IEnumerator _worldPositionTransition;
    private IEnumerator _rotationTransition;
    private IEnumerator _scaleTransition;
    private IEnumerator _areaTransition;
    private IEnumerator _colorTransition;
    private IEnumerator _alphaTransition;

    public void Move(TransitionType Move_Way, float Moving_Time, PositionType Axis, float Start_Axis, float End_Axis)
    {
        if (_moveTransition != null) { StopCoroutine(_moveTransition); }
        _moveTransition = MoveCoroutine(Move_Way, Moving_Time, Axis, Start_Axis, End_Axis);
        StartCoroutine(_moveTransition);
    }

    public IEnumerator MoveCoroutine(TransitionType Move_Way, float Moving_Time, PositionType Axis, float Start_Axis, float End_Axis)
    {
        if (Move_Way == TransitionType.Instant)
        {
            RectTransform Target_Obj_Rect = gameObject.GetComponent<RectTransform>();
            Vector3 Target_Obj_Pos = Target_Obj_Rect.anchoredPosition;

            if (Axis == PositionType.x) { Target_Obj_Pos.x = End_Axis; }
            else if (Axis == PositionType.y) { Target_Obj_Pos.y = End_Axis; }
            else if (Axis == PositionType.z) { Target_Obj_Pos.z = End_Axis; }

            Target_Obj_Rect.anchoredPosition = Target_Obj_Pos;
        }
        else
        {
            RectTransform Target_Obj_Rect = gameObject.GetComponent<RectTransform>();

            Vector3 Start_Pos = Target_Obj_Rect.anchoredPosition;
            Vector3 End_Pos = Target_Obj_Rect.anchoredPosition;

            if (Axis == PositionType.x) { Start_Pos.x = Start_Axis; }
            else if (Axis == PositionType.y) { Start_Pos.y = Start_Axis; }
            else if (Axis == PositionType.z) { Start_Pos.z = Start_Axis; }

            if (Axis == PositionType.x) { End_Pos.x = End_Axis; }
            else if (Axis == PositionType.y) { End_Pos.y = End_Axis; }
            else if (Axis == PositionType.z) { End_Pos.z = End_Axis; }

            Target_Obj_Rect.anchoredPosition = Start_Pos;

            float ElapsedTime = 0f;
            while (ElapsedTime < Moving_Time)
            {
                float Obj_Speed = ElapsedTime / Moving_Time;
                if (Move_Way == TransitionType.Lerp)
                {
                    Target_Obj_Rect.anchoredPosition = new Vector2(Mathf.Lerp(Start_Pos.x, End_Pos.x, Obj_Speed), Mathf.Lerp(Start_Pos.y, End_Pos.y, Obj_Speed));
                }
                else if (Move_Way == TransitionType.Smooth)
                {
                    Target_Obj_Rect.anchoredPosition = new Vector2(Mathf.SmoothStep(Start_Pos.x, End_Pos.x, Obj_Speed), Mathf.SmoothStep(Start_Pos.y, End_Pos.y, Obj_Speed));
                }
                ElapsedTime += Time.deltaTime;
                yield return null;
            }
            Target_Obj_Rect.anchoredPosition = End_Pos;
        }
    }

    public void Position(TransitionType Positioning_Way, float Positioning_Time, Vector3 Start_Pos, Vector3 End_Pos)
    {
        if (_positionTransition != null) { StopCoroutine(_positionTransition); }
        _positionTransition = PositionCoroutine(Positioning_Way, Positioning_Time, Start_Pos, End_Pos);
        StartCoroutine(_positionTransition);
    }

    public IEnumerator PositionCoroutine(TransitionType Positioning_Way, float Positioning_Time, Vector3 Start_Pos, Vector3 End_Pos)
    {
        if (Positioning_Way == TransitionType.Instant)
        {
            RectTransform Target_Obj_Rect = gameObject.GetComponent<RectTransform>();
            Target_Obj_Rect.anchoredPosition = End_Pos;
        }
        else
        {
            RectTransform Target_Obj_Rect = gameObject.GetComponent<RectTransform>();
            Target_Obj_Rect.anchoredPosition = Start_Pos;

            float ElapsedTime = 0f;
            while (ElapsedTime < Positioning_Time)
            {
                float Obj_Speed = ElapsedTime / Positioning_Time;
                if (Positioning_Way == TransitionType.Lerp)
                {
                    Target_Obj_Rect.anchoredPosition = new Vector3(Mathf.Lerp(Start_Pos.x, End_Pos.x, Obj_Speed), Mathf.Lerp(Start_Pos.y, End_Pos.y, Obj_Speed), 0f);
                }
                else if (Positioning_Way == TransitionType.Smooth)
                {
                    Target_Obj_Rect.anchoredPosition = new Vector3(Mathf.SmoothStep(Start_Pos.x, End_Pos.x, Obj_Speed), Mathf.SmoothStep(Start_Pos.y, End_Pos.y, Obj_Speed), 0f);
                }
                ElapsedTime += Time.deltaTime;
                yield return null;
            }
            Target_Obj_Rect.anchoredPosition = End_Pos;
        }
    }

    public void WorldPosition(TransitionType WorldPositioning_Way, float WorldPositioning_Time, Vector3 Start_Pos, Vector3 End_Pos)
    {
        if (_worldPositionTransition != null) { StopCoroutine(_worldPositionTransition); }
        _worldPositionTransition = WorldPositionCoroutine(WorldPositioning_Way, WorldPositioning_Time, Start_Pos, End_Pos);
        StartCoroutine(_worldPositionTransition);
    }

    public IEnumerator WorldPositionCoroutine(TransitionType WorldPositioning_Way, float WorldPositioning_Time, Vector3 Start_Pos, Vector3 End_Pos)
    {
        if (WorldPositioning_Way == TransitionType.Instant)
        {
            Transform Target_Obj_Transform = gameObject.GetComponent<Transform>();
            Target_Obj_Transform.position = End_Pos;
        }
        else
        {
            Transform Target_Obj_Transform = gameObject.GetComponent<Transform>();
            Target_Obj_Transform.position = Start_Pos;

            float ElapsedTime = 0f;
            while (ElapsedTime < WorldPositioning_Time)
            {
                float Obj_Speed = ElapsedTime / WorldPositioning_Time;
                if (WorldPositioning_Way == TransitionType.Lerp)
                {
                    Target_Obj_Transform.position = new Vector3(Mathf.Lerp(Start_Pos.x, End_Pos.x, Obj_Speed), Mathf.Lerp(Start_Pos.y, End_Pos.y, Obj_Speed), 0f);
                }
                else if (WorldPositioning_Way == TransitionType.Smooth)
                {
                    Target_Obj_Transform.position = new Vector3(Mathf.SmoothStep(Start_Pos.x, End_Pos.x, Obj_Speed), Mathf.SmoothStep(Start_Pos.y, End_Pos.y, Obj_Speed), 0f);
                }
                ElapsedTime += Time.deltaTime;
                yield return null;
            }
            Target_Obj_Transform.position = End_Pos;
        }
    }

    public void Rotate(TransitionType Rotating_Way, float Rotating_Time, Vector3 Start_Pos, Vector3 End_Pos)
    {
        if (_rotationTransition != null) { StopCoroutine(_rotationTransition); }
        _rotationTransition = RotateCoroutine(Rotating_Way, Rotating_Time, Start_Pos, End_Pos);
        StartCoroutine(_rotationTransition);
    }

    public IEnumerator RotateCoroutine(TransitionType Rotating_Way, float Rotating_Time, Vector3 Start_Pos, Vector3 End_Pos)
    {
        if (Rotating_Way == TransitionType.Instant)
        {
            RectTransform Target_Obj_Rect = gameObject.GetComponent<RectTransform>();
            Target_Obj_Rect.rotation = Quaternion.Euler(End_Pos);
        }
        else
        {
            RectTransform Target_Obj_Rect = gameObject.GetComponent<RectTransform>();
            Target_Obj_Rect.rotation = Quaternion.Euler(Start_Pos);

            float ElapsedTime = 0f;
            while (ElapsedTime < Rotating_Time)
            {
                float Obj_Speed = ElapsedTime / Rotating_Time;
                if (Rotating_Way == TransitionType.Lerp)
                {
                    Target_Obj_Rect.rotation = Quaternion.Euler(new Vector3(Mathf.Lerp(Start_Pos.x, End_Pos.x, Obj_Speed), Mathf.Lerp(Start_Pos.y, End_Pos.y, Obj_Speed), Mathf.Lerp(Start_Pos.z, End_Pos.z, Obj_Speed)));
                }
                else if (Rotating_Way == TransitionType.Smooth)
                {
                    Target_Obj_Rect.rotation = Quaternion.Euler(new Vector3(Mathf.SmoothStep(Start_Pos.x, End_Pos.x, Obj_Speed), Mathf.SmoothStep(Start_Pos.y, End_Pos.y, Obj_Speed), Mathf.Lerp(Start_Pos.z, End_Pos.z, Obj_Speed)));
                }
                ElapsedTime += Time.deltaTime;
                yield return null;
            }
            Target_Obj_Rect.rotation = Quaternion.Euler(End_Pos);
        }
    }

    public void Scale(TransitionType Scaling_Way, float Scaling_Time, Vector3 Start_Pos, Vector3 End_Pos)
    {
        if (_scaleTransition != null) { StopCoroutine(_scaleTransition); }
        _scaleTransition = ScaleCoroutine(Scaling_Way, Scaling_Time, Start_Pos, End_Pos);
        StartCoroutine(_scaleTransition);
    }

    public IEnumerator ScaleCoroutine(TransitionType Scaling_Way, float Scaling_Time, Vector3 Start_Pos, Vector3 End_Pos)
    {
        if (Scaling_Way == TransitionType.Instant)
        {
            RectTransform Target_Obj_Rect = gameObject.GetComponent<RectTransform>();
            Target_Obj_Rect.localScale = End_Pos;
        }
        else
        {
            RectTransform Target_Obj_Rect = gameObject.GetComponent<RectTransform>();
            Target_Obj_Rect.localScale = Start_Pos;
            float ElapsedTime = 0f;
            while (ElapsedTime < Scaling_Time)
            {
                float Obj_Speed = ElapsedTime / Scaling_Time;
                if (Scaling_Way == TransitionType.Lerp)
                {
                    Target_Obj_Rect.localScale = new Vector3(Mathf.Lerp(Start_Pos.x, End_Pos.x, Obj_Speed), Mathf.Lerp(Start_Pos.y, End_Pos.y, Obj_Speed), 0f);
                }
                else if (Scaling_Way == TransitionType.Smooth)
                {
                    Target_Obj_Rect.localScale = new Vector3(Mathf.SmoothStep(Start_Pos.x, End_Pos.x, Obj_Speed), Mathf.SmoothStep(Start_Pos.y, End_Pos.y, Obj_Speed), 0f);
                }
                ElapsedTime += Time.deltaTime;
                yield return null;
            }
            Target_Obj_Rect.localScale = End_Pos;
        }
    }

    public void Area(TransitionType Enlarging_Way, float Enlarging_Time, Vector2 Start_Area, Vector2 End_Area)
    {
        if (_areaTransition != null) { StopCoroutine(_areaTransition); }
        _areaTransition = AreaCoroutine(Enlarging_Way, Enlarging_Time, Start_Area, End_Area);
        StartCoroutine(_areaTransition);
    }

    public IEnumerator AreaCoroutine(TransitionType Enlarging_Way, float Enlarging_Time, Vector2 Start_Area, Vector2 End_Area)
    {
        if (Enlarging_Way == TransitionType.Instant)
        {
            RectTransform Target_Obj_Rect = gameObject.GetComponent<RectTransform>();
            Target_Obj_Rect.sizeDelta = End_Area;
        }
        else
        {
            RectTransform Target_Obj_Rect = gameObject.GetComponent<RectTransform>();
            Target_Obj_Rect.sizeDelta = Start_Area;
            float ElapsedTime = 0f;
            while (ElapsedTime < Enlarging_Time)
            {
                float Obj_Speed = ElapsedTime / Enlarging_Time;
                if (Enlarging_Way == TransitionType.Lerp)
                {
                    Target_Obj_Rect.sizeDelta = new Vector2(Mathf.Lerp(Start_Area.x, End_Area.x, Obj_Speed), Mathf.Lerp(Start_Area.y, End_Area.y, Obj_Speed));
                }
                else if (Enlarging_Way == TransitionType.Smooth)
                {
                    Target_Obj_Rect.sizeDelta = new Vector2(Mathf.SmoothStep(Start_Area.x, End_Area.x, Obj_Speed), Mathf.SmoothStep(Start_Area.y, End_Area.y, Obj_Speed));
                }
                ElapsedTime += Time.deltaTime;
                yield return null;
            }
            Target_Obj_Rect.sizeDelta = End_Area;
        }
    }

    public void SetColor(TransitionType Coloring_Way, float Coloring_Time, Color Start_Color, Color End_Color)
    {
        if (_colorTransition != null) { StopCoroutine(_colorTransition); }
        _colorTransition = SetColorCoroutine(Coloring_Way, Coloring_Time, Start_Color, End_Color);
        StartCoroutine(_colorTransition);
    }

    public IEnumerator SetColorCoroutine(TransitionType Coloring_Way, float Coloring_Time, Color Start_Color, Color End_Color)
    {
        if (Coloring_Way == TransitionType.Instant)
        {
            Image Target_Obj_Image = gameObject.GetComponent<Image>();
            Target_Obj_Image.color = End_Color;
        }
        else
        {
            Image Target_Obj_Image = gameObject.GetComponent<Image>();
            Target_Obj_Image.color = Start_Color;
            float ElapsedTime = 0f;
            while (ElapsedTime < Coloring_Time)
            {
                float Obj_Speed = ElapsedTime / Coloring_Time;
                if (Coloring_Way == TransitionType.Lerp)
                {
                    Target_Obj_Image.color = Color.Lerp(Start_Color, End_Color, Obj_Speed);
                }
                ElapsedTime += Time.deltaTime;
                yield return null;
            }
            Target_Obj_Image.color = End_Color;
        }
    }

    public void SetImage(Sprite Sprite)
    {
        Image Target_Obj_Image = gameObject.GetComponent<Image>();
        Target_Obj_Image.sprite = Sprite;
    }

    public void SetAlpha(TransitionType Transparenting_Way, float Transparenting_Time, float Start_Alpha, float End_Alpha)
    {
        if (_alphaTransition != null) { StopCoroutine(_alphaTransition); }
        _alphaTransition = SetAlphaCoroutine(Transparenting_Way, Transparenting_Time, Start_Alpha, End_Alpha);
        StartCoroutine(_alphaTransition);
    }

    public IEnumerator SetAlphaCoroutine(TransitionType Transparenting_Way, float Transparenting_Time, float Start_Alpha, float End_Alpha)
    {
        if (Transparenting_Way == TransitionType.Instant)
        {
            CanvasGroup Target_Obj_Alpha = gameObject.GetComponent<CanvasGroup>();
            Target_Obj_Alpha.alpha = End_Alpha;
        }
        else
        {
            CanvasGroup Target_Obj_Alpha = gameObject.GetComponent<CanvasGroup>();
            Target_Obj_Alpha.alpha = Start_Alpha;
            float ElapsedTime = 0f;
            while (ElapsedTime < Transparenting_Time)
            {
                float Obj_Speed = ElapsedTime / Transparenting_Time;
                if (Transparenting_Way == TransitionType.Lerp)
                {
                    Target_Obj_Alpha.alpha = Mathf.Lerp(Start_Alpha, End_Alpha, Obj_Speed);
                }
                else if (Transparenting_Way == TransitionType.Smooth)
                {
                    Target_Obj_Alpha.alpha = Mathf.SmoothStep(Start_Alpha, End_Alpha, Obj_Speed);
                }
                ElapsedTime += Time.deltaTime;
                yield return null;
            }
            Target_Obj_Alpha.alpha = End_Alpha;
        }
    }
}
