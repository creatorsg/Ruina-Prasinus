using UnityEngine;

public abstract class FindChildObject : MonoBehaviour
{  
    protected Transform FindChildWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                return child;
            }
        }
        Debug.LogWarning($"Child with tag '{tag}' not found on {parent.name}");
        return null;
    }
}
