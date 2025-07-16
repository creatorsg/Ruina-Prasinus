using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Player;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }
    private string savePath => Application.persistentDataPath + "/save";

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    [System.Serializable]
    class GameData
    {
        public Vector3 playerPosition;
        public int playerHP;
        // …필요한 데이터 추가
    }

    public void SaveToSlot(int slot)
    {
        GameData data = new GameData();
        // 1) 현재 플레이어 위치, 상태 등 수집
        var player = Player_Controller.Instance.transform;
        data.playerPosition = player.position;
        data.playerHP = Player_Controller.Instance.HP;
        // …다른 데이터도 채워넣기

        // 2) 파일로 직렬화
        string file = $"{savePath}_slot{slot}.dat";
        Directory.CreateDirectory(Path.GetDirectoryName(file));
        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream fs = new FileStream(file, FileMode.Create))
            bf.Serialize(fs, data);

        Debug.Log($"[SaveSystem] 슬롯 {slot}에 저장되었습니다.");
    }

    public bool LoadFromSlot(int slot)
    {
        string file = $"{savePath}_slot{slot}.dat";
        if (!File.Exists(file)) return false;

        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream fs = new FileStream(file, FileMode.Open))
        {
            GameData data = bf.Deserialize(fs) as GameData;
            // 3) 데이터 적용
            Player_Controller.Instance.transform.position = data.playerPosition;
            // …나머지 적용
        }

        Debug.Log($"[SaveSystem] 슬롯 {slot}에서 불러왔습니다.");
        return true;
    }
}
