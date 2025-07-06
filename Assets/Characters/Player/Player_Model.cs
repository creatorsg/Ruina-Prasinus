using UnityEngine;

namespace Player
{
    public class Player_Model
    {
        [Header("— 조건에 따른 사용 가능 여부 —")]
        public bool canDash = true;         // 대쉬가능여부(쿨타임 등)
        public bool canWalk = true;         // 걷기가능여부
        public bool canJump = true;         // 점프가능여부

        [Header("— 플레이어 현재 상태 —")]
        public bool isDashing = false;      // 대쉬중
        public bool isHit = false;          // 피격당함
        public bool isGrounded = false;     // 땅에 있나 
        public bool inCinematic = false;    // 연출중
        public bool inGearSetting = false;  // 기어셋 설정중
        public bool inGearAction = false;   // 기어셋 발동중 
        public bool inUIControl = false;    // UI 세팅중 
        public bool isFalling = false;      // 떨어지는 중
    }
}
