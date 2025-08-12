using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterSpawnerFromTilemap : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject waterPrefab;

    void Start()
    {
        BoundsInt bounds = tilemap.cellBounds;
        bool[,] visited = new bool[bounds.size.x, bounds.size.y];

        int offsetX = -bounds.xMin;
        int offsetY = -bounds.yMin;

        for (int y = bounds.yMin; y <= bounds.yMax; y++)
        {
            for (int x = bounds.xMin; x <= bounds.xMax; x++)
            {
                int ix = x + offsetX;
                int iy = y + offsetY;

                // 범위 검사 + 이미 방문했거나 타일이 없으면 스킵
                if (ix < 0 || ix >= visited.GetLength(0) ||
                    iy < 0 || iy >= visited.GetLength(1) ||
                    visited[ix, iy] || !tilemap.HasTile(new Vector3Int(x, y, 0)))
                    continue;

                // 직사각형 크기 탐색
                int width = 0;
                int height = 0;

                // 가로 길이
                while (x + width <= bounds.xMax)
                {
                    int checkX = x + width + offsetX;
                    if (checkX < 0 || checkX >= visited.GetLength(0)) break;
                    if (!tilemap.HasTile(new Vector3Int(x + width, y, 0)) ||
                        visited[checkX, iy]) break;
                    width++;
                }

                // 세로 길이
                bool canExtendY = true;
                while (y + height <= bounds.yMax && canExtendY)
                {
                    for (int dx = 0; dx < width; dx++)
                    {
                        int vx = x + dx + offsetX;
                        int vy = y + height + offsetY;

                        if (vx < 0 || vx >= visited.GetLength(0) ||
                            vy < 0 || vy >= visited.GetLength(1) ||
                            !tilemap.HasTile(new Vector3Int(x + dx, y + height, 0)) ||
                            visited[vx, vy])
                        {
                            canExtendY = false;
                            break;
                        }
                    }
                    if (canExtendY) height++;
                }

                // 방문 처리
                for (int dy = 0; dy < height; dy++)
                {
                    for (int dx = 0; dx < width; dx++)
                    {
                        int vx = x + dx + offsetX;
                        int vy = y + dy + offsetY;
                        if (vx >= 0 && vx < visited.GetLength(0) &&
                            vy >= 0 && vy < visited.GetLength(1))
                        {
                            visited[vx, vy] = true;
                        }
                    }
                }

                // 중앙 좌표 (타일 중앙 보정)
                float midX = x + (width / 2f) - 0.5f;
                float midY = y + (height / 2f) - 0.5f;
                Vector3 worldPos = tilemap.CellToWorld(new Vector3Int(Mathf.FloorToInt(midX), Mathf.FloorToInt(midY), 0))
                                  + (Vector3)tilemap.cellSize / 2f;
                worldPos.z = 0;

                // 물 프리팹 생성
                GameObject go = Instantiate(waterPrefab, worldPos, Quaternion.identity);
                if (go == null)
                {
                    Debug.LogError("프리팹 생성 실패!");
                    continue;
                }

                // Water 컴포넌트 설정
                var water = go.GetComponent<Water>();
                if (water != null)
                {
                    water.Quality = Mathf.Max(10, width * 20);
                    water.Width = width;
                    water.Height = height;
                    water.UpdateMesh();
                }
                else
                {
                    Debug.LogError("Water 컴포넌트 누락!");
                }
            }
        }
    }
}
