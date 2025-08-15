using UnityEngine;

public class ExplodeHandler : MonoBehaviour
{
    PurpleMushrooms _enemy3;
    private explosion explosion;
    private float explodeTimer;

    public void Initilaize(PurpleMushrooms enemy3)
    {
        _enemy3 = enemy3;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Wall"))
        {
            _enemy3.ChangeState(Enemy3Behaviour.Die);
        }
    }

    public void Explore()
    {
        explodeTimer = 0f;

        Destroy(gameObject);
        GameObject obj = Instantiate(explosion.explosionPrefab, transform.position, Quaternion.identity);
        
        while(explodeTimer >= explosion.ExplodeTimer)
        {
            obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, explosion.FinalScale, explodeTimer / explosion.ExplodeTimer   );
            explodeTimer += Time.deltaTime;
        }
        Destroy(obj);
    }
}
