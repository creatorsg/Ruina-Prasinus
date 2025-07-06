using UnityEngine;

namespace enemy
{
    public class EnemyAttack : MonoBehaviour
    {
        [Header("�������� ���� �� ������")]
        [SerializeField] private Enemy enemy;
        [SerializeField] private PlayerData playerData;

        private Transform player;

        void Awake()
        {
            var playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null)
                player = playerGO.transform;
            else
                Debug.LogWarning("Player �±� ������Ʈ�� ã�� ���߽��ϴ�.");
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
                Debug.Log($"�÷��̾�� {enemy.attackPower}��ŭ ����!");
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
