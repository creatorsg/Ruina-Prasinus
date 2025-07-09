using UnityEngine;
using System.Collections;

public class Jangpung : MonoBehaviour
{
    public float janpungSpeed = 20f;
    public float janpungStandbyTime = 0.2f;
    public float destroyTime = 0.3f;
    public GameObject JanpungPrefab;

    bool isLaunchStandby = false;
    bool isLaunch = false;
    bool isOnCooldown = false;

    // 플레이어가 바라보는 방향 저장 (-1: 왼쪽, +1: 오른쪽)
    int Direction = 1;

    void Update()
    {
        // A/D 또는 좌우 화살표 키로 방향 기억
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Direction = -1;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Direction = 1;
        }

        // W 키 또는 위 화살표 눌림 여부
        isLaunchStandby = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        // 마우스 클릭 또는 C 키 눌림 여부
        isLaunch = Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.C);


        // 발사 조건
        if (isLaunch && !isOnCooldown)
        {
            ShootJangpung(isLaunchStandby);
            StartCoroutine(JangpungCooldown());
        }
    }

    void ShootJangpung(bool upward)
    {
        Vector3 spawnPosition = transform.position;

        Vector2 direction;

        if (upward)
        {
            // 위 방햘 발사
            direction = new Vector2(0, 1).normalized;
        }
        else
        {
            // 좌/우 방향 발사
            direction = new Vector2(Direction, 0);
        }

        GameObject jangpung = Instantiate(JanpungPrefab, spawnPosition, Quaternion.identity);

        Rigidbody2D rb = jangpung.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * janpungSpeed * 3f;
        }

        Destroy(jangpung, destroyTime);
    }

    IEnumerator JangpungCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(janpungStandbyTime);
        isOnCooldown = false;
    }
}