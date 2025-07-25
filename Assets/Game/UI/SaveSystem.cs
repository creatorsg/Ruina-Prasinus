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
        // ���ʿ��� ������ �߰�
    }

    public void SaveToSlot(int slot)
    {
        GameData data = new GameData();
        // 1) ���� �÷��̾� ��ġ, ���� �� ����
        var player = Player_Controller.Instance.transform;
        data.playerPosition = player.position;
        data.playerHP = Player_Controller.Instance.HP;
        // ���ٸ� �����͵� ä���ֱ�

        // 2) ���Ϸ� ����ȭ
        string file = $"{savePath}_slot{slot}.dat";
        Directory.CreateDirectory(Path.GetDirectoryName(file));
        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream fs = new FileStream(file, FileMode.Create))
            bf.Serialize(fs, data);

        Debug.Log($"[SaveSystem] ���� {slot}�� ����Ǿ����ϴ�.");
    }

    public bool LoadFromSlot(int slot)
    {
        string file = $"{savePath}_slot{slot}.dat";
        if (!File.Exists(file)) return false;

        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream fs = new FileStream(file, FileMode.Open))
        {
            GameData data = bf.Deserialize(fs) as GameData;
            // 3) ������ ����
            Player_Controller.Instance.transform.position = data.playerPosition;
            // �������� ����
        }

        Debug.Log($"[SaveSystem] ���� {slot}���� �ҷ��Խ��ϴ�.");
        return true;
    }
}
