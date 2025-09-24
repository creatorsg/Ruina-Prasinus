using UnityEngine;

public class obstacle : MonoBehaviour
{
    private bool isActive = true;
    private playerHpHandler playerHpHandler;

    private void Awake()
    {
        playerHpHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<playerHpHandler>();
    }

    private void Update()
    {
        if(isActive)
        {
            playerHpHandler.Damaged(10);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isActive = false;
        }
    }
}
