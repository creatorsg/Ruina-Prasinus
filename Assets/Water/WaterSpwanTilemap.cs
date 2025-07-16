using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterSpawnerFromTilemap : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject waterPrefab;

    void Start()
    {
        BoundsInt bounds = tilemap.cellBounds;

        for (int y = bounds.yMin; y <= bounds.yMax; y++)
        {
            int startX = -1;

            for (int x = bounds.xMin; x <= bounds.xMax + 1; x++) // +1 for edge
            {
                Vector3Int currentPos = new Vector3Int(x, y, 0);
                bool isWaterTile = tilemap.HasTile(currentPos); // �̸� ���� ����

                if (isWaterTile)
                {
                    if (startX == -1)
                        startX = x;
                }
                else if (startX != -1)
                {
                    int endX = x - 1;
                    int length = endX - startX + 1;

                    float midX = (startX + endX + 1) / 2f;
                    Vector3 worldPos = tilemap.CellToWorld(new Vector3Int((int)midX, y, 0));
                    worldPos.z = 0;

                    GameObject go = Instantiate(waterPrefab, worldPos, Quaternion.identity);
                    if (go == null)
                    {
                        Debug.LogError("������ ���� ����!");
                        startX = -1;
                        continue;
                    }

                    var water = go.GetComponent<Water>();
                    if (water != null)
                    {
                        water.Quality = Mathf.Max(10, length * 20);
                        water.Width = length;
                        water.Height = 3f;
                        water.UpdateMesh();
                    }
                    else
                    {
                        Debug.LogError("Water ������Ʈ ����!");
                    }

                    startX = -1;
                }
            }
        }
    }
}
