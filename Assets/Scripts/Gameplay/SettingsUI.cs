using System;
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
    
    [SerializeField] private TMP_Dropdown outputDeviceDropdown;
    
    [Header("Graphics UI")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown displayModeDropdown;
    [SerializeField] private TMP_Dropdown fpsLimitDropdown;
    [SerializeField] private Toggle vSyncToggle;
    
    private SettingsManager settingsManager;

    private void Start()
    {
        settingsManager = SettingsManager.Instance;
        
        if (settingsManager == null)
        {
            return;
        }
        
        SetupResolutionOptions();
        SetupDisplayModeOptions();
        SetupFPSOptions();
        SetupAudioDeviceOptions();
        SetupAudioSliders();

        if (vSyncToggle != null)
        {
            vSyncToggle.SetIsOnWithoutNotify(settingsManager.vSyncEnabled);
        }
    }

    private void SetupAudioSliders()
    {
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.SetValueWithoutNotify(SettingsManager.Instance.masterVolume);
            masterVolumeSlider.onValueChanged.AddListener(SettingsManager.Instance.SetMasterVolume);
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.SetValueWithoutNotify(SettingsManager.Instance.musicVolume);
            musicVolumeSlider.onValueChanged.AddListener(SettingsManager.Instance.SetMusicVolume);
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.SetValueWithoutNotify(SettingsManager.Instance.soundsVolume);
            sfxVolumeSlider.onValueChanged.AddListener(SettingsManager.Instance.SetSoundsVolume);
        }
    }

    private void SetupResolutionOptions()
    {
        if (resolutionDropdown == null)
        {
            return;
        }
        
        resolutionDropdown.ClearOptions();
        
        List<string> options = new List<string>();
        List<Resolution> resolutions = settingsManager.availableResolutions;

        for (int i = 0; i < resolutions.Count; i++)
        {
            options.Add(resolutions[i].width + " x " + resolutions[i].height);
        }
        
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.SetValueWithoutNotify(settingsManager.selectedResolution);
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
        
        displayModeDropdown.SetValueWithoutNotify(settingsManager.selectedDisplayMode);
        displayModeDropdown.RefreshShownValue();
    }

    private void SetupFPSOptions()
    {
        if (fpsLimitDropdown == null)
        {
            return;
        }
        
        fpsLimitDropdown.ClearOptions();
        fpsLimitDropdown.AddOptions(new List<string> {"30 FPS", "60 FPS", "120 FPS", "144 FPS", "240 FPS", "Unlimited"});
        
        fpsLimitDropdown.SetValueWithoutNotify(settingsManager.GetSavedFPSLimitIndex());
        fpsLimitDropdown.RefreshShownValue();
    }

    private void SetupAudioDeviceOptions()
    {
        if (outputDeviceDropdown == null)
        {
            return;
        }
        
        outputDeviceDropdown.ClearOptions();
        outputDeviceDropdown.AddOptions(new List<string> {"System Default"});
        
        outputDeviceDropdown.SetValueWithoutNotify(0);
        outputDeviceDropdown.RefreshShownValue();
        outputDeviceDropdown.interactable = false;
    }

    public void ApplyChanges()
    {
        if (settingsManager == null)
        {
            return;
        }
        
        int resIndex = resolutionDropdown != null ? resolutionDropdown.value : settingsManager.selectedResolution;
        int displayMode = displayModeDropdown != null ? displayModeDropdown.value : settingsManager.selectedDisplayMode;
        bool vSync = vSyncToggle != null ? vSyncToggle.isOn : settingsManager.vSyncEnabled;
        int fpsLimit = fpsLimitDropdown != null ? fpsLimitDropdown.value : settingsManager.selectedFPSLimit;

        settingsManager.SaveGraphicsSettings(resIndex, displayMode, vSync, fpsLimit);
    }

    public void ResetSettings()
    {
        if (settingsManager == null)
        {
            return;
        }
        
        settingsManager.ResetSettings();

        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.SetValueWithoutNotify(settingsManager.masterVolume);
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.SetValueWithoutNotify(settingsManager.musicVolume);
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.SetValueWithoutNotify(settingsManager.soundsVolume);
        }

        if (resolutionDropdown != null)
        {
            resolutionDropdown.SetValueWithoutNotify(settingsManager.selectedResolution);
            resolutionDropdown.RefreshShownValue();
        }

        if (displayModeDropdown != null)
        {
            displayModeDropdown.SetValueWithoutNotify(settingsManager.selectedDisplayMode);
            displayModeDropdown.RefreshShownValue();
        }

        if (fpsLimitDropdown != null)
        {
            fpsLimitDropdown.SetValueWithoutNotify(settingsManager.selectedFPSLimit);
            fpsLimitDropdown.RefreshShownValue();
        }

        if (vSyncToggle != null)
        {
            vSyncToggle.SetIsOnWithoutNotify(settingsManager.vSyncEnabled);
        }
    }

    private void OnDestroy()
    {
        if (settingsManager == null)
        {
            return;
        }
        
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
    }
}