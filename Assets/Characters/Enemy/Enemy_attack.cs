using Player;
using UnityEngine;

namespace enemy
{
    public class EnemyAttack : MonoBehaviour
    {
        [Header("에셋으로 만든 적 데이터")]
        [SerializeField] private Enemy enemy;
        [SerializeField] private PlayerData playerData;

        private Player_Model model = new Player_Model();
        private Transform player;

        private float attackCooldown = 1f; // 공격 쿨타임(초)
        private float attackTimer = 0f;

        void Awake()
        {
            var playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null)
                player = playerGO.transform;
            else
                Debug.LogWarning("Player 태그 오브젝트를 찾지 못했습니다.");
        }

        void Update()
        {
            
        }

        void FixedUpdate()
        {
            if (enemy == null || player == null) return;

            attackTimer -= Time.fixedDeltaTime;

            float dist = Vector3.Distance(transform.position, player.position);
            if (dist <= enemy.attackRange && attackTimer <= 0f)
            {
                Attack();
                attackTimer = attackCooldown; // 쿨타임 초기화
            }
        }


        private void Attack()
        {
            if(enemy == null) return;

            var m = model;

            if(!m.isHit && !m.isInvincible)
            {
                if(playerData.hp > 0)
                Debug.Log($"플레이어에게 {enemy.attackPower}만큼 공격!");
                playerData.hp -= 3f;
            }
        }

        void OnDrawGizmosSelected()
        {
            if (enemy == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemy.attackRange);
        }
    }
}
