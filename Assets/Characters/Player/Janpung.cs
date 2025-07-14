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

    // �÷��̾ �ٶ󺸴� ���� ���� (-1: ����, +1: ������)
    int Direction = 1;

    void Update()
    {
        // A/D �Ǵ� �¿� ȭ��ǥ Ű�� ���� ���
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Direction = -1;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Direction = 1;
        }

        // W Ű �Ǵ� �� ȭ��ǥ ���� ����
        isLaunchStandby = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        // ���콺 Ŭ�� �Ǵ� C Ű ���� ����
        isLaunch = Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.C);


        // �߻� ����
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
            // �� ���l �߻�
            direction = new Vector2(0, 1).normalized;
        }
        else
        {
            // ��/�� ���� �߻�
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