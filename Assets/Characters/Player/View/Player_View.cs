using UnityEngine;
using System.Collections;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]

    public class Player_View : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private SpriteRenderer sr;
        private Coroutine blinkCoroutine;
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        private Animator animator;

        public System.Action<bool> OnMoveStateChanged;

        private bool lastMoveState = false;
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
            //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            animator = GetComponent<Animator>();
            //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        }

        

        // 이동 관련 부분 
        public void UpdateWalk(float inputX, float speed)
        {
            var v = rb.linearVelocity;
            v.x = inputX * speed;
            rb.linearVelocity = v;


            bool isMoving = Mathf.Abs(inputX) > 0.01f;
            animator.SetBool("isMove", isMoving);

            if (isMoving != lastMoveState)
            {
                lastMoveState = isMoving;
                OnMoveStateChanged?.Invoke(isMoving); // 애니메이터 매니저에 알려줌
            }

        }


        public void UpdateDash(float speed)
        {
            transform.position += Vector3.right * speed * Time.fixedDeltaTime;
        }

        public void JumpImpulse(float speed)
        {
            if (rb == null) return;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
        }

        public void ContinueJump(float speed)
        {
            if (rb == null) return;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, speed);
        }

        public void StartBlink()
        {
            if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
            blinkCoroutine = StartCoroutine(BlinkEffect());
        }

        public void StopBlink()
        {
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
                blinkCoroutine = null;
            }

            if (sr != null)
            {
                Color c = sr.color;
                c.a = 1f;
                sr.color = c;
            }
        }

        private IEnumerator BlinkEffect()
        {
            Color color = sr.color;

            while (true)
            {
                // 투명하게
                color.a = 0.3f;
                sr.color = color;
                yield return new WaitForSeconds(0.2f);

                // 불투명하게
                color.a = 1f;
                sr.color = color;
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
