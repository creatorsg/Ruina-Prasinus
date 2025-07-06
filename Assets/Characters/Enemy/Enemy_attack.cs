using UnityEngine;

namespace enemy
{
    public class EnemyAttack : MonoBehaviour
    {
        [Header("에셋으로 만든 적 데이터")]
        [SerializeField] private Enemy enemy;
        [SerializeField] private PlayerData playerData;

        private Transform player;

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

        private void FixedUpdate()
        {
            if (enemy == null || player == null) return;
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist <= enemy.attackRange)
                Attack();
        }

        private void Attack()
        {
            if(enemy == null) return;

            if (playerData.hp > 0)
            {
                Debug.Log($"플레이어에게 {enemy.attackPower}만큼 공격!");
                playerData.hp -= 5f;
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
