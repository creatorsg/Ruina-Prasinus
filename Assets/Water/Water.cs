using System;
using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider2D))] // 박스 콜라이더 생성@@@@@@@@@@@@@@@
public class Water : MonoBehaviour
{
    // 물의 표면 정점(픽셀?) 갯수, 이 값이 높을수록 물결이 부드럽게 표현됨(300~600 추천쓰)
    [Range(100, 1200), SerializeField] private int _quality;
    public int Quality
    {
        get => _quality;
        set
        {
            if (value <= 0) throw new ArgumentException("Quality는 0보다 커야 합니다.");
            if (value == _quality) return;
            _quality = value;
            UpdateMesh();
        }
    }

    // 물의 너비
    [Range(0.1f, 1000), SerializeField] private float _width;
    public float Width
    {
        get => _width;
        set
        {
            if (value < 0) throw new ArgumentException("Width는 0보다 커야 합니다.");
            if (value == _width) return;
            _width = value;
            UpdateMesh();
            col.size = new Vector2(_width, _height);
        }
    }

    // 물의 높이입니다.
    [Range(0.1f, 100), SerializeField] private float _height;
    public float Height
    {
        get => _height;
        set
        {
            if (value < 0) throw new ArgumentException("Height는 0보다 커야 합니다.");
            if (value == _height) return;
            _height = value;
            UpdateMesh();
            col.size = new Vector2(_width, _height);
        }
    }

    // 물의 물결이 사라지는 속도, 이 값이 높을수록 물결이 빨리 사라짐
    [Range(0.001f, 10f), SerializeField] private float _waveDecay;
    public float WaveDecay
    {
        get => _waveDecay;
        set
        {
            if (value < 0) throw new ArgumentException("WaveDecay는 0보다 커야 합니다.");
            if (value == _waveDecay) return;
            _waveDecay = value;
        }
    }

    // 물의 버텍스 위치를 표현하는 배열입니다.
    private Vector3[] vertices;

    // 물의 표면의 y속력을 표현하는 배열입니다.
    private float[] velocities;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private BoxCollider2D col;

    private void Awake()
    {
        if (meshFilter == null) meshFilter = GetComponent<MeshFilter>();
        if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
        if (col == null) col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
        if (IsMeshUpdateNeeded()) UpdateMesh();
        FindNeighbors();
    }

    // 물체가 활성화 될때, 버텍스를 초기화
    private void OnEnable()
    {
        vertices = GetVertices();
        velocities = new float[_quality + 1];
        meshFilter.mesh.SetVertices(vertices);
    }

    void FixedUpdate()
    {
        if (IsWaveCalculationNeeded())
        {
            CalculateTension();
            CalculateRestoringForce();
            PropagateWaveToNeighbors(); // 파동을 이웃에게 전달
            meshFilter.mesh.SetVertices(vertices);
        }
    }


    // 물체가 충돌했을때, 물의 표면을 이루는 정점들에 힘을 가함
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger || other.attachedRigidbody == null) return;

        // 물체가 가하는 힘이 미미할때는 무시
        var otherRb = other.attachedRigidbody;
        float forceY = otherRb.linearVelocity.y * otherRb.mass * 0.06f;
        if (Math.Abs(forceY) < 0.03f) return;
            

        // 물체의 위치에 따라 힘을 가하는 정점의 인덱스를 계산
        float interval = _width / _quality;
        float xMin = transform.position.x - _width / 2;
        int xPosIndex = (int)((other.transform.position.x - xMin) / interval);

        // 위에서 계산한 인덱스를 기준으로, 반경 10개의 주변 정점에 힘을 가함
        int startIndex = Math.Max(xPosIndex - 10, 0);
        int endIndex = Math.Min(xPosIndex + 10, _quality);

        for (int j = startIndex; j <= endIndex; j++)
        {
            velocities[j] += forceY;
        }
    }

    [SerializeField, Range(0f, 1f)] private float _horizontalForceMultiplier = 0.05f; // 좌우 속도에 따른 파동 강도
    private void OnTriggerStay2D(Collider2D other)
    {
        
        if (other.isTrigger || other.attachedRigidbody == null) return;

        var rb = other.attachedRigidbody;

        // 수평 이동 감지
        float forceX = rb.linearVelocity.x * rb.mass * _horizontalForceMultiplier;
        Debug.Log($"forceX = {forceX}");
        if (Mathf.Abs(forceX) < 0.01f) return;
        

        float interval = _width / _quality;
        float xMin = transform.position.x - _width / 2;
        int xPosIndex = (int)((other.transform.position.x - xMin) / interval);

        int startIndex = Mathf.Max(xPosIndex - 5, 0);
        int endIndex = Mathf.Min(xPosIndex + 5, _quality);

        for (int i = startIndex; i <= endIndex; i++)
        {
            velocities[i] += forceX;
        }
    }






#if UNITY_EDITOR

    // 인스펙터의 값을 변경할때 발생하는 워닝을 피하기 위해서 delayCall을 사용합니다.
    void OnValidate() { UnityEditor.EditorApplication.delayCall += _OnValidate; }

    // 인스펙터의 값을 변경할때, 메시가 재생성되어야 하는지 확인합니다.
    private void _OnValidate()
    {
        if (meshFilter == null) meshFilter = GetComponent<MeshFilter>();
        if (col == null) col = GetComponent<BoxCollider2D>();
        if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
        if (_width < 0.01f) _width = 0.1f;
        if (_height < 0.01f) _height = 0.1f;
        if (_quality < 100) _quality = 100;
        if (!IsMeshUpdateNeeded()) return;

        UpdateMesh();
        col.size = new Vector2(_width, _height);
    }
#endif


    // 메시가 업데이트 될 필요가 있는지 확인합니다.
    // 메시의 정점 갯수와, 폭 그리고 높이가 현재 설정된 값과 다른지 확인합니다.
    private bool IsMeshUpdateNeeded()
    {
        if (meshFilter.sharedMesh.vertexCount != (_quality + 1) * 2) return true;
        var bounds = meshFilter.sharedMesh.bounds;
        if (!Mathf.Approximately(bounds.size.x, _width) || !Mathf.Approximately(bounds.size.y, _height)) return true;
        return false;
    }

    // 물결 계산이 필요한지 확인합니다.
    // 모든 정점의 속력이 0이면, 표면에 물결이 없다고 판단합니다.
    private bool IsWaveCalculationNeeded()
    {
        for (int i = 0; i < _quality; i++)
        {
            if (velocities[i] != 0) return true;
        }
        return false;
    }

    // 장력에 의한 물결파의 전달을 계산합니다.
    // 물결 표면을 이루는 정점의 위치가 주변 정점의 위치에 비해 높거나 낮을때, 서로간에 힘을 전달합니다.
    // 평균 500 ticks 소요됩니다. (Quality 300)
    // private void CalculateTension()
    // {
    //     int len = _quality + 1;

    //     for(int i = 0; i < len; i++)
    //     {
    //         float yPos = vertices[i*2].y;
    //         float vel = velocities[i];

    //         for (int j = -2; j <= 2; j++)
    //         {
    //             if(j == 0) continue;
    //             int idx = i + j;
    //             if(idx >= 0 && idx < len)
    //             {
    //                 float yPos2 = vertices[idx*2].y;
    //                 float vel2 = velocities[idx];
    //                 float yPosDiff = yPos - yPos2;
    //                 float tensionVel = yPosDiff / 8;
    //                 float tensionPos = yPosDiff / 16;

    //                 vel -= tensionVel;
    //                 velocities[i] = vel;

    //                 vel2 += tensionVel;
    //                 velocities[idx] = vel2;

    //                 yPos -= tensionPos;
    //                 vertices[i*2].y = yPos;

    //                 yPos2 += tensionPos;
    //                 vertices[idx*2].y = yPos2;
    //             }
    //         }
    //     }
    // }


    // 장력에 의한 물결파의 전달을 계산합니다.
    // 물결 표면을 이루는 정점의 위치가 주변 정점의 위치에 비해 높거나 낮을때, 서로간에 힘을 전달합니다.
    // 평균 200 ticks 소요됩니다. (Quality 300)

    [SerializeField, Range(1, 10)] private int _waveSpreadRange = 2; // 전파 거리 (몇 칸까지 영향 주는지)
    [SerializeField, Range(0.01f, 10f)] private float _waveSpreadSpeed = 1.0f;
    private float waveStep = 0f;

    private void CalculateTension()
    {
        int len = _quality + 1;

        for (int i = 0; i < len; i++)
        {
            ref float yPos = ref vertices[i * 2].y;
            ref float vel = ref velocities[i];

            for (int j = -_waveSpreadRange; j <= _waveSpreadRange; j++)
            {
                if (j == 0) continue;
                int idx = i + j;
                if (idx >= 0 && idx < len)
                {
                    ref float yPos2 = ref vertices[idx * 2].y;
                    ref float vel2 = ref velocities[idx];

                    float yPosDiff = yPos - yPos2;

                    // 속도 조절은 기존대로 유지
                    float tensionVel = yPosDiff * _waveSpreadSpeed * 0.125f;
                    float tensionPos = yPosDiff * _waveSpreadSpeed * 0.0625f;

                    vel -= tensionVel;
                    vel2 += tensionVel;
                    yPos -= tensionPos;
                    yPos2 += tensionPos;
                }
            }
        }
    }




    // 물 표면을 이루는 각 정점을 스프링으로 생각하고, 정점의 속력과 위치를 계산합니다.
    // 기준 위치로부터 떨어진 정도에 따라 복원력을 계산하고, 이를 속력에서 뺀 다음, 위치에 속력을 더합니다.
    // 이때, 속력과 위치가 일정 값 이하로 떨어지면, 해당 정점은 안정적인 상태로 간주하고, 위치와 속력을 초기화합니다.
    // 평균 120 ticks 소요됩니다. (Quality 300)
    // private void CalculateRestoringForce()
    // {
    //     float maxY = _height / 2;
    //     int len = _quality + 1;
    //     float decayFactor = 1 - _waveDecay / 10;

    //     for(int i = 0; i < len; i++)
    //     {
    //         float yPos = vertices[i*2].y;
    //         float vel = velocities[i];

    //         if((Math.Abs(vel) + Math.Abs(yPos - maxY)) < 0.01f)
    //         {
    //             yPos = maxY;
    //             vel = 0;
    //         }
    //         else
    //         {
    //             float springForce = (yPos - maxY) / 100;
    //             vel -= springForce;
    //             vel *= decayFactor;
    //             yPos += vel;
    //         }

    //         vertices[i*2].y = yPos;
    //         velocities[i] = vel;
    //     }
    // }


    // 물 표면을 이루는 각 정점을 스프링으로 생각하고, 정점의 속력과 위치를 계산합니다.
    // 기준 위치로부터 떨어진 정도에 따라 복원력을 계산하고, 이를 속력에서 뺀 다음, 위치에 속력을 더합니다.
    // 이때, 속력과 위치가 일정 값 이하로 떨어지면, 해당 정점은 안정적인 상태로 간주하고, 위치와 속력을 초기화합니다.
    // 평균 80 ticks 소요됩니다. (Quality 300)
    [SerializeField, Range(0.01f, 10f)] private float _waveHeightLimit = 0.5f; // 최대 파동 높이 (절대값 기준)
    private void CalculateRestoringForce()
    {
        float defaultY = _height / 2;
        int len = _quality + 1;
        float decayFactor = 1 - _waveDecay / 10;
        const float stableThreshold = 0.01f;

        for (int i = 0; i < len; i++)
        {
            ref float yPos = ref vertices[i * 2].y;
            ref float vel = ref velocities[i];
            float yPosDiff = yPos - defaultY;

            if ((Math.Abs(vel) + Math.Abs(yPosDiff)) < stableThreshold)
            {
                yPos = defaultY;
                vel = 0;
            }
            else
            {
                vel -= yPosDiff * 0.01f;
                vel *= decayFactor;
                yPos += vel;

                // ✅ 파동의 높이를 제한하는 부분
                float maxY = defaultY + _waveHeightLimit;
                float minY = defaultY - _waveHeightLimit;
                yPos = Mathf.Clamp(yPos, minY, maxY);
            }
        }
    }


    // 컴포넌트의 값을 초기화합니다.
    void Reset()
    {
        _quality = 300;
        _width = 10;
        _height = 3;
        _waveDecay = 0.1f;

        transform.localScale = Vector3.one;
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
        UpdateMesh();
    }


    // 메시를 업데이트 합니다.
    private void UpdateMesh()
    {
        var mesh = new Mesh();
        vertices = GetVertices();
        mesh.vertices = vertices;
        mesh.uv = GetUV(vertices);
        mesh.triangles = GetTris();
        velocities = new float[_quality + 1];
        meshFilter.mesh = mesh;
    }

    // 물체의 너비와 높이를 기준으로 정점을 생성합니다.
    private Vector3[] GetVertices()
    {
        Vector3 left_top = new Vector3(-_width / 2, _height / 2, 0);
        Vector3 left_bottom = new Vector3(-_width / 2, -_height / 2, 0);
        Vector3 right_top = new Vector3(_width / 2, _height / 2, 0);
        Vector3 right_bottom = new Vector3(_width / 2, -_height / 2, 0);

        Vector3[] result = new Vector3[(_quality + 1) * 2];
        for (int i = 0; i < _quality + 1; i++)
        {
            // 0포함 짝수 인덱스는 위쪽, 홀수 인덱스는 아래쪽 정점을 의미함
            result[i * 2] = Vector3.Lerp(left_top, right_top, (float)i / _quality);
            result[i * 2 + 1] = Vector3.Lerp(left_bottom, right_bottom, (float)i / _quality);
        }
        return result;
    }

    // 정점의 위치를 기준으로 UV를 생성합니다.
    private Vector2[] GetUV(Vector3[] vertices)
    {
        var uvs = new Vector2[vertices.Length];
        var size = new Vector2(_width, _height);
        var half = new Vector2(0.5f, 0.5f);

        for (int i = 0; i < vertices.Length; i++)
        {
            uvs[i] = vertices[i] / size + half;
        }

        return uvs;
    }

    // 정점을 잇는 삼각형을 생성합니다.
    private int[] GetTris()
    {
        int[] result = new int[_quality * 6];
        for (int i = 0; i < _quality; i++)
        {
            // 왼쪽 삼각형
            result[i * 6] = i * 2;
            result[i * 6 + 1] = i * 2 + 2;
            result[i * 6 + 2] = i * 2 + 1;

            // 오른쪽 삼각형
            result[i * 6 + 3] = i * 2 + 2;
            result[i * 6 + 4] = i * 2 + 3;
            result[i * 6 + 5] = i * 2 + 1;
        }
        return result;
    }

    public Water LeftNeighbor { get; set; }
    public Water RightNeighbor { get; set; }
    private void PropagateWaveToNeighbors()
    {
        int range = 5; // 양 옆 경계에서 몇 개 정점을 평균낼지
        int lastIndex = _quality;

        if (LeftNeighbor != null)
        {
            int leftNeighborLast = LeftNeighbor._quality;

            for (int i = 0; i < range; i++)
            {
                int thisIndex = i;
                int neighborIndex = leftNeighborLast - (range - 1) + i;

                if (neighborIndex < 0 || thisIndex >= velocities.Length) continue;

                float avgVelocity = (velocities[thisIndex] + LeftNeighbor.velocities[neighborIndex]) * 0.5f;
                float avgY = (vertices[thisIndex * 2].y + LeftNeighbor.vertices[neighborIndex * 2].y) * 0.5f;

                velocities[thisIndex] = avgVelocity;
                vertices[thisIndex * 2].y = avgY;

                LeftNeighbor.velocities[neighborIndex] = avgVelocity;
                LeftNeighbor.vertices[neighborIndex * 2].y = avgY;
            }
        }

        if (RightNeighbor != null)
        {
            for (int i = 0; i < range; i++)
            {
                int thisIndex = lastIndex - (range - 1) + i;
                int neighborIndex = i;

                if (thisIndex < 0 || neighborIndex >= RightNeighbor.velocities.Length) continue;

                float avgVelocity = (velocities[thisIndex] + RightNeighbor.velocities[neighborIndex]) * 0.5f;
                float avgY = (vertices[thisIndex * 2].y + RightNeighbor.vertices[neighborIndex * 2].y) * 0.5f;

                velocities[thisIndex] = avgVelocity;
                vertices[thisIndex * 2].y = avgY;

                RightNeighbor.velocities[neighborIndex] = avgVelocity;
                RightNeighbor.vertices[neighborIndex * 2].y = avgY;
            }
        }
    }




    private void FindNeighbors()
    {
        foreach (var water in FindObjectsOfType<Water>())
        {
            if (water == this) continue;

            float dx = water.transform.position.x - transform.position.x;
            if (Mathf.Approximately(dx, -_width))
                LeftNeighbor = water;
            else if (Mathf.Approximately(dx, _width))
                RightNeighbor = water;
        }
    }

}
