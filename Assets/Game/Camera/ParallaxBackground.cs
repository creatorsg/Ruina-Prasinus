using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float parallaxFactor = 0.5f;

    [SerializeField] private bool moveX;
    [SerializeField] private bool moveY;
    [SerializeField] private bool isTest;

    private void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(cameraTransform.position.x * (moveX ? parallaxFactor : 1), cameraTransform.position.y * (moveY ? parallaxFactor : 1), 0);
        if (isTest)
            Debug.Log(transform.position.x);
    }
}