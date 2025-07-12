using Player;
using UnityEngine;

namespace enemy
{
    public class EnemyAttack : MonoBehaviour
    {
        [Header("�������� ���� �� ������")]
        [SerializeField] private Enemy enemy;
        [SerializeField] private PlayerData playerData;

        private Player_Model model = new Player_Model();
        private Transform player;

        private float attackCooldown = 1f; // ���� ��Ÿ��(��)
        private float attackTimer = 0f;

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

        void FixedUpdate()
        {
            if (enemy == null || player == null) return;

            attackTimer -= Time.fixedDeltaTime;

            float dist = Vector3.Distance(transform.position, player.position);
            if (dist <= enemy.attackRange && attackTimer <= 0f)
            {
                Attack();
                attackTimer = attackCooldown; // ��Ÿ�� �ʱ�ȭ
            }
        }


        private void Attack()
        {
            if (enemy == null) return;

            var m = model;

            if (!m.isHit && !m.isInvincible)
            {
                if (playerData.hp > 0)
                    Debug.Log($"�÷��̾�� {enemy.attackPower}��ŭ ����!");
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
