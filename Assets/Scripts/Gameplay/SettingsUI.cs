using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("Audio Sliders")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    
    [Header("Graphics UI")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown displayModeDropdown;
    [SerializeField] private Toggle vSyncToggle;
    
    private List<Resolution> resolutions = new List<Resolution>();

    private void Start()
    {
        SetupResolutionOptions();
        SetupDisplayModeOptions();
        SetupAudioSliders();

        if (vSyncToggle != null)
        {
            vSyncToggle.SetIsOnWithoutNotify(SettingsManager.Instance.VSyncEnabled);
            vSyncToggle.onValueChanged.AddListener(OnVSyncChanged);
        }
    }

    private void SetupAudioSliders()
    {
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.SetValueWithoutNotify(SettingsManager.Instance.MasterVolume);
            masterVolumeSlider.onValueChanged.AddListener(SettingsManager.Instance.SetMasterVolume);
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.SetValueWithoutNotify(SettingsManager.Instance.MusicVolume);
            musicVolumeSlider.onValueChanged.AddListener(SettingsManager.Instance.SetMusicVolume);
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.SetValueWithoutNotify(SettingsManager.Instance.SoundsVolume);
            sfxVolumeSlider.onValueChanged.AddListener(SettingsManager.Instance.SetSoundsVolume);
        }
    }

    private void SetupResolutionOptions()
    {
        if (resolutionDropdown == null)
        {
            return;
        }
        
        resolutions.Clear();
        resolutionDropdown.ClearOptions();
        
        Resolution[] systemResolutions = Screen.resolutions;
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < systemResolutions.Length; i++)
        {
            Resolution resolution = systemResolutions[i];
            bool duplicate = false;

            for (int x = 0; x < resolutions.Count; x++)
            {
                if (resolutions[x].width == resolution.width && resolutions[x].height == resolution.height)
                {
                    duplicate = true;
                    break;
                }
            }

            if (duplicate)
            {
                continue;
            }
            
            resolutions.Add(resolution);
            options.Add(resolution.width + " x " + resolution.height);

            if (resolution.width == Screen.width && resolution.height == Screen.height)
            {
                currentResolutionIndex = resolutions.Count - 1;
            }
        }
        
        resolutionDropdown.AddOptions(options);

        int savedRes = SettingsManager.Instance.SelectedResolution;
        if (savedRes < 0 || savedRes >= resolutions.Count)
        {
            savedRes = currentResolutionIndex;
        }
        
        resolutionDropdown.SetValueWithoutNotify(savedRes);
        resolutionDropdown.RefreshShownValue();
    }

    private void SetupDisplayModeOptions()
    {
        if (displayModeDropdown == null)
        {
            return;
        }
        
        displayModeDropdown.ClearOptions();
        displayModeDropdown.AddOptions(new List<string>{"Fullscreen", "BorderLess", "Windowed"});
        
        int savedMode = Mathf.Clamp(SettingsManager.Instance.SelectedDisplayMode, 0, 2);
        displayModeDropdown.SetValueWithoutNotify(savedMode);
        displayModeDropdown.RefreshShownValue();
    }

    public void ApplyChanges()
    {
        int resIndex = resolutionDropdown != null ? resolutionDropdown.value : SettingsManager.Instance.SelectedResolution;
        int displayMode = displayModeDropdown != null ? displayModeDropdown.value : SettingsManager.Instance.SelectedDisplayMode;
        bool vSync = vSyncToggle != null ? vSyncToggle.isOn : SettingsManager.Instance.VSyncEnabled;

        SettingsManager.Instance.SaveGraphicsSettings(resIndex, displayMode, vSync);
    }

    private void OnVSyncChanged(bool enabled)
    {
        ApplyChanges();
    }

    private void OnDestroy()
    {
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.onValueChanged.RemoveListener(SettingsManager.Instance.SetMasterVolume);
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.RemoveListener(SettingsManager.Instance.SetMusicVolume);
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.onValueChanged.RemoveListener(SettingsManager.Instance.SetSoundsVolume);
        }

        if (vSyncToggle != null)
        {
            vSyncToggle.onValueChanged.RemoveListener(OnVSyncChanged);
        }
    }
}