using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BG_STR
{
    [SerializeField] public Transform backgroundTransform;
    [SerializeField] public float parallaxFactorX;
    [SerializeField] public float parallaxFactorY;
}

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private List<BG_STR> BG_List;

    private void LateUpdate()
    {
        foreach (BG_STR bg in BG_List)
        bg.backgroundTransform.position = new Vector3(cameraTransform.position.x * bg.parallaxFactorX, cameraTransform.position.y * bg.parallaxFactorY, 0);
    }
}