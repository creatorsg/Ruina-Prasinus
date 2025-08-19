using Player;
using UnityEngine;

public class YDeltaChecker : MonoBehaviour
{
    private float previousY; // 이전 프레임의 y값
    private float deltaY;    // y 변화량

    [SerializeField] private AnimatorManager animatorManager;
    void Awake()
    {
        animatorManager = GetComponentInChildren<Player.AnimatorManager>();
    }
    void Start()
    {
        // 시작 시 현재 y값 저장
        previousY = transform.position.y;
    }

    void Update()
    {
        float currentY = transform.position.y;

        // 현재 y값과 이전 y값의 차이를 계산
        deltaY = currentY - previousY;

        if (deltaY > 0)
        {
            animatorManager?.SetUpBool(true);
            animatorManager?.SetDownBool(false);
        }
        else if (deltaY < 0)
        {
            animatorManager?.SetUpBool(false);
            animatorManager?.SetDownBool(true);
        }
        else
        {
            animatorManager?.SetUpBool(false);
            animatorManager?.SetDownBool(false);
        }

        // 현재 y값을 다음 프레임을 위한 이전값으로 저장
        previousY = currentY;
    }
}
