using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotUI : MonoBehaviour
{
    private int slot;

    [SerializeField] private TMP_Text worldName;
    [SerializeField] private TMP_Text playtimeText;
    [SerializeField] private TMP_Text lastPlayedText;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Button button;

    private void Start()
    {
        UpdateSlotUI();
    }

    private void UpdateSlotUI()
    {
        SaveData data = SaveManager.instance.GetSlotData(slot);

        if (data == null)
        {
            worldName.text = "World" + (slot + 1);
            playtimeText.text = "";
            lastPlayedText.text = "";
            buttonText.text = "CREATE NEW WORLD";
            
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(CreateWorld);
        }
        else
        {
            worldName.text = data.worldName;
            playtimeText.text = "Played:  " + data.playtime.ToString("F2");
            lastPlayedText.text = "Last played: "  + data.lastPlayed;
            buttonText.text = "LOAD WORLD";
            
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(LoadWorld);
        }
    }

    public void CreateWorld()
    {
        SaveManager.instance.CreateWorld(slot);
        LoadGameScene();
    }

    public void LoadWorld()
    {
        SaveManager.instance.LoadWorld(slot);
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}
