using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider2D))] // 박스 콜라이더 생성@@@@@@@@@@@@@@@
public class Water : MonoBehaviour
{
    // 물의 표면 정점(픽셀?) 갯수, 이 값이 높을수록 물결이 부드럽게 표현됨(300~600 추천쓰)
    [Header("물 정점 개수")]
    [Range(10, 1200), SerializeField] private int _quality;
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
    private float _width;
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

    private float _height;
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
    [Header("물 물결 사라지는 속도(조정 필요하다면 최소한으로 조정)")]
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
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;

        UpdateMesh(); // 반드시 실행하여 meshFilter.mesh를 보장


    }

    // 물체가 활성화 될때, 버텍스를 초기화
    private void OnEnable()
    {
        if (meshFilter.mesh == null)
            UpdateMesh(); // 메시가 없는 경우 새로 생성

        vertices = GetVertices();
        velocities = new float[_quality + 1];

        var mesh = meshFilter.mesh;
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        meshFilter.mesh = mesh;
    }



    [SerializeField, Range(0.01f, 4f)]
    private float _timeScale = 1.0f;  // 1.0 = 정상, 0.1 = 10배 느림

    private float _localTime = 0f;
    private float _fixedStep = 0.02f; // FixedUpdate와 동일한 시간 간격


    

    void FixedUpdate()
    {

        // 로컬 시간 누적 (전체 시간은 그대로 유지)
        _localTime += Time.fixedDeltaTime * _timeScale;

        // 느려진 시간에 맞춰 계산 실행 (고정 간격으로 처리)
        if (_localTime >= _fixedStep)
        {
            _localTime -= _fixedStep;


            if (IsWaveCalculationNeeded())
            {
                CalculateTension();
                CalculateRestoringForce();

                


                var mesh = meshFilter.mesh;
                mesh.vertices = vertices;
                mesh.RecalculateNormals();
                mesh.RecalculateBounds();
                meshFilter.mesh = mesh;
            }
        }
    }

    private void Start()
    {
        if (meshFilter.mesh == null)
            UpdateMesh();

        vertices = GetVertices();
        velocities = new float[_quality + 1];

        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.RecalculateNormals();
        meshFilter.mesh.RecalculateBounds();

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


    [Header("X축 이동시 추가되는 파동 강도")]
    [SerializeField, Range(0f, 1f)] private float _horizontalForceMultiplier = 0.01f; // 파동 강도 (고정값)
    private Dictionary<Rigidbody2D, float> lastXPositions = new Dictionary<Rigidbody2D, float>();

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.isTrigger || other.attachedRigidbody == null) return;

        var rb = other.attachedRigidbody;

        // 이전 위치 없으면 초기화
        if (!lastXPositions.ContainsKey(rb))
            lastXPositions[rb] = rb.position.x;

        float previousX = lastXPositions[rb];
        float currentX = rb.position.x;
        float deltaX = currentX - previousX;

        // ---- 파동이 완전히 없는 상태 체크 ----
        bool noWave = true;
        for (int i = 0; i < velocities.Length; i++)
        {
            if (Mathf.Abs(velocities[i]) > 0.01f) // 약간이라도 흔들림이 있으면 false
            {
                noWave = false;
                break;
            }
        }

        // 거의 안 움직였고 파동도 없으면 그냥 통과
        if (Mathf.Abs(deltaX) < 0.01f && !noWave)
        {
            lastXPositions[rb] = currentX;
            return;
        }

        // 이동 여부와 상관없이 (noWave면) 일정한 힘 적용
        float forceX = rb.mass * _horizontalForceMultiplier * 0.1f;

        // 파동 인덱스 계산
        float interval = _width / _quality;
        float xMin = transform.position.x - _width / 2;
        int xPosIndex = (int)((other.transform.position.x - xMin) / interval);

        int startIndex = Mathf.Max(xPosIndex - 5, 0);
        int endIndex = Mathf.Min(xPosIndex + 5, _quality);

        for (int i = startIndex; i <= endIndex; i++)
        {
            velocities[i] += forceX;
        }

        lastXPositions[rb] = currentX;

        HandleSmallWaves(other);
    }

    // 잔물결 설정값
    [Header("오브젝트 물안에 있을시 생성하는 작은 파동")]
    [SerializeField] private float smallWaveForce = 0.015f;       // 작은 파동 세기
    [SerializeField] private float smallWaveInterval = 0.2f;      // 생성 간격
    [SerializeField] private float waveThreshold = 0.01f;         // "파동이 없다" 기준

    // 오브젝트별 타이머 관리
    private Dictionary<Rigidbody2D, float> smallWaveTimers = new Dictionary<Rigidbody2D, float>();

    // 새로운 메서드: 잔물결 처리
    private void HandleSmallWaves(Collider2D other)
    {
        if (other.isTrigger || other.attachedRigidbody == null) return;

        var rb = other.attachedRigidbody;

        // 타이머가 없으면 초기화
        if (!smallWaveTimers.ContainsKey(rb))
            smallWaveTimers[rb] = 0f;

        // 타이머 감소
        smallWaveTimers[rb] -= Time.fixedDeltaTime;

        // 현재 파동이 있는지 확인
        bool noWave = true;
        for (int i = 0; i < velocities.Length; i++)
        {
            if (Mathf.Abs(velocities[i]) > waveThreshold)
            {
                noWave = false;
                break;
            }
        }

        // 파동이 남아있으면 새 파동 생성 안 함
        if (!noWave) return;

        // 파동이 없고, 타이머 끝나면 새 파동 생성
        if (smallWaveTimers[rb] <= 0f)
        {
            smallWaveTimers[rb] = smallWaveInterval;

            float interval = _width / _quality;
            float xMin = transform.position.x - _width / 2;
            int xPosIndex = (int)((other.transform.position.x - xMin) / interval);

            int startIndex = Mathf.Max(xPosIndex - 3, 0);
            int endIndex = Mathf.Min(xPosIndex + 3, _quality);

            for (int i = startIndex; i <= endIndex; i++)
                velocities[i] += smallWaveForce;
        }
    }

    // 사용 방법: OnTriggerStay2D 안에서 호출
    // HandleSmallWaves(other);










#if UNITY_EDITOR

    // 인스펙터의 값을 변경할때 발생하는 워닝을 피하기 위해서 delayCall을 사용합니다.
    void OnValidate() { UnityEditor.EditorApplication.delayCall += _OnValidate; }

    // 인스펙터의 값을 변경할때, 메시가 재생성되어야 하는지 확인합니다.
    private void _OnValidate()
    {
        if (this == null || gameObject == null) return;

        if (meshFilter == null) meshFilter = GetComponent<MeshFilter>();
        if (col == null) col = GetComponent<BoxCollider2D>();
        if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();

        if (_width < 0.01f) _width = 0.1f;
        if (_height < 0.01f) _height = 0.1f;
        if (_quality < 20) _quality = 20;

        if (!IsMeshUpdateNeeded()) return;

        UpdateMesh();
        if (col != null)
            col.size = new Vector2(_width, _height);
    }

#endif


    // 메시가 업데이트 될 필요가 있는지 확인합니다.
    // 메시의 정점 갯수와, 폭 그리고 높이가 현재 설정된 값과 다른지 확인합니다.
    private bool IsMeshUpdateNeeded()
    {
        if (meshFilter == null || meshFilter.sharedMesh == null) return true;

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
    [Header("전파 거리")]
    [SerializeField, Range(1, 10)] private int _waveSpreadRange = 2; // 전파 거리 (몇 칸까지 영향 주는지)
    [Header("전파 속도")]
    [SerializeField, Range(0.01f, 10f)] private float _waveSpreadSpeed = 1.0f;


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
    [Header("최대 높낮이 조정")]
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
    public void UpdateMesh()
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

    

}
