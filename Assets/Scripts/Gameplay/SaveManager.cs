using System;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private int slotCount = 3;
    
    private float playtimeTimer;
    public int CurrentSlot = -1;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (CurrentSlot != -1)
        {
            playtimeTimer += Time.deltaTime;
        }
    }

    public void CreateWorld(int slot)
    {
        if (!isValidSlot(slot))
        {
            return;
        }

        SaveData data = new SaveData();
        data.slot = slot;
        data.worldName = "World" + (slot + 1);
        data.lastPlayed = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
        data.playtime = 0f;
        
        SaveDataToFile(data);

        CurrentSlot = slot;
        playtimeTimer = 0f;
    }

    public void LoadWorld(int slot)
    {
        if (!isValidSlot(slot))
        {
            return;
        }
        
        SaveData data = LoadDataFromFile(slot);

        if (data == null)
        {
            return;
        }
        
        CurrentSlot = slot;
        playtimeTimer = data.playtime;
        data.lastPlayed = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
        
        SaveDataToFile(data);
    }

    public void SaveWorld()
    {
        if (CurrentSlot == -1)
        {
            return;
        }
        
        SaveData data = LoadDataFromFile(CurrentSlot);

        if (data == null)
        {
            return;
        }
        
        data.playtime = playtimeTimer;
        data.lastPlayed = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
        
        SaveDataToFile(data);
    }

    private SaveData LoadDataFromFile(int slot)
    {
        string path = GetSavePath(slot);

        if (!File.Exists(path))
        {
            return null;
        }
        
        string json =  File.ReadAllText(path);
        return JsonUtility.FromJson<SaveData>(json);
    }

    private void SaveDataToFile(SaveData data)
    {
        string json =  JsonUtility.ToJson(data, true);
        File.WriteAllText(GetSavePath(data.slot), json);
    }

    private string GetSavePath(int slot)
    {
        return Path.Combine(Application.persistentDataPath, "save" + slot + ".json");
    }

    public SaveData GetSlotData(int slot)
    {
        if (!isValidSlot(slot))
        {
            return null;
        }
        
        return LoadDataFromFile(slot);
    }

    private bool isValidSlot(int slot)
    {
        return slot >= 0 && slot < slotCount;
    }
}

public class SaveData
{
    public int slot;
    public string worldName;
    public string lastPlayed;
    public float playtime;
}
