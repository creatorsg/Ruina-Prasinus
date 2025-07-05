using UnityEngine;

[CreateAssetMenu(fileName = "ComponentGraphData", menuName = "Graph/Component Graph Data")]
public class ComponentGraphData : ScriptableObject
{
    [TextArea(10, 100)]
    public string graphJson;
}
