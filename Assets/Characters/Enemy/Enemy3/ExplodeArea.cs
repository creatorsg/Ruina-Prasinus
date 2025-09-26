using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    [SerializeField] private CharacterEnemy3 characterData;

    private void OnDrawGizmosSelected()
    {
        if (characterData == null) return;

        // ExplodeDistance를 빨간색 원으로 표시
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, characterData.ExplodeDistance);
    }
}
