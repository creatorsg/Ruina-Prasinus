using Player;
using System.Collections;
using UnityEngine;

public class Jangpung : MonoBehaviour
{
    public float janpungSpeed = 20f;
    public float janpungStandbyTime = 0.2f;
    public float destroyTime = 0.3f;
    public GameObject JanpungPrefab;

    bool isLaunchStandby = false;
    bool isLaunch = false;
    bool isDownLaunch = false;
    bool isOnCooldown = false;

    private Player_Model model = new Player_Model();

    // �÷��̾ �ٶ󺸴� ���� ���� (-1: ����, +1: ������)
    int Direction = 1;

    void Update()
    {
        // A/D �Ǵ� �¿� ȭ��ǥ Ű�� ���� ���
        if (InputManager.GetKey("MoveLeft"))
        {
            Direction = -1;
        }
        else if (InputManager.GetKey("MoveRight"))
        {
            Direction = 1;
        }

        // W Ű �Ǵ� �� ȭ��ǥ ���� ����
        isLaunchStandby = InputManager.GetKey("LookUP");

        isDownLaunch = InputManager.GetKey("LieDown");

        // ���콺 Ŭ�� �Ǵ� C Ű ���� ����
        isLaunch = InputManager.GetKey("Attack");



        if (!model.isGrounded)
        {
            if (isLaunch && !isOnCooldown)
            {
                ShootJangpung(isLaunchStandby, isDownLaunch);
                StartCoroutine(JangpungCooldown());
            }
        }

        // 발사 조건
        if (isLaunch && !isOnCooldown)
        {
            ShootJangpung(isLaunchStandby, isDownLaunch);
            StartCoroutine(JangpungCooldown());
        }

    }


    void ShootJangpung(bool upward, bool downward)
    {
        Vector3 spawnPosition = transform.position;
        Vector2 direction;

        if (upward)
        {
            direction = Vector2.up;
        }
        else if (downward)
        {
            direction = Vector2.down;
        }
        else
        {
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