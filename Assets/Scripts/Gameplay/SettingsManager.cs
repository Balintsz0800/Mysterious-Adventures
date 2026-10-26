using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.Rendering;

public class SettingsManager : MonoBehaviour
{
    public static  SettingsManager Instance;
    
    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;

    public float masterVolume = 1f;
    public float musicVolume = 0.6f;
    public float soundsVolume = 0.4f;
    public int selectedResolution = -1;
    public int selectedDisplayMode;
    public bool vSyncEnabled = true;
    public int selectedFPSLimit = 1;
    
    public List<Resolution> availableResolutions =  new List<Resolution>();
    
    private const string MasterVolumeKey = "Master";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SfxVolumeKey = "SoundsVolume";
    private const string ResolutionWidthKey = "ResolutionWidth";
    private const string ResolutionHeightKey = "ResolutionHeight";
    private const string DisplayModeKey = "DisplayMode";
    private const string VSyncKey = "VSync";
    private const string FPSLimitKey = "FPSLimit";

    private int savedResolutionWidth;
    private int savedResolutionHeight;

    private readonly int[] fpsOptions = { 30, 60, 120, 144, 240, 0 };

    private void Awake()
    {
        if (Instance != null &&  Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        BuildResolutionList();
        LoadSettings();
        FindSelectedResolution();
        ApplyAudioSettings();
        ApplyGraphicsSettings();
    }
    
    private void LoadSettings()
    {
        masterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, 1f);
        musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.6f);
        soundsVolume = PlayerPrefs.GetFloat(SfxVolumeKey, 0.4f);
        
        savedResolutionWidth = PlayerPrefs.GetInt(ResolutionWidthKey, Screen.width);
        savedResolutionHeight = PlayerPrefs.GetInt(ResolutionHeightKey, Screen.height);
        
        selectedDisplayMode = Mathf.Clamp(PlayerPrefs.GetInt(DisplayModeKey, 0), 0, 2);
        vSyncEnabled = PlayerPrefs.GetInt(VSyncKey, 1) == 1;
        selectedFPSLimit = Mathf.Clamp(PlayerPrefs.GetInt(FPSLimitKey, 1), 0, 5);
    }

    private void BuildResolutionList()
    {
        availableResolutions.Clear();
        
        Resolution[] sysresolutions = Screen.resolutions;

        for (int i = 0; i < sysresolutions.Length; i++)
        {
            Resolution resolution = sysresolutions[i];
            bool duplicate = false;

            for (int x = 0; x < availableResolutions.Count; x++)
            {
                if (availableResolutions[x].width == resolution.width && availableResolutions[x].height == resolution.height)
                {
                    duplicate = true;
                    break;
                }
            }

            if (!duplicate)
            {
                availableResolutions.Add(resolution);
            }
        }
        
        if (availableResolutions.Count == 0)
        {
            availableResolutions.Add(Screen.currentResolution);
        }
    }

    private void FindSelectedResolution()
    {
        selectedResolution = 0;

        for (int i = 0; i < availableResolutions.Count; i++)
        {
            if (availableResolutions[i].width == savedResolutionWidth &&
                availableResolutions[i].height == savedResolutionHeight)
            {
                selectedResolution = i;
                return;
            }
        }
        
        selectedResolution = availableResolutions.Count - 1;
    }

    public void SetMasterVolume(float value)
    {
        masterVolume = Mathf.Clamp01(value);
        SetMixerVolume("Master", masterVolume);
        SaveAudioSettings();
    }
    
    public void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Clamp01(value);
        SetMixerVolume("MusicVolume", musicVolume);
        SaveAudioSettings();
    }

    public void SetSoundsVolume(float value)
    {
        soundsVolume = Mathf.Clamp01(value);
        SetMixerVolume("SoundsVolume", soundsVolume);
        SaveAudioSettings();
    }

    private void SetMixerVolume(string key, float value)
    {
        if (audioMixer == null)
        {
            return;
        }

        float decibels = value > 0.00001f ? Mathf.Log10(value) * 20f : -80f;
        audioMixer.SetFloat(key, decibels);
    }

    public void ApplyAudioSettings()
    {
        SetMixerVolume(MasterVolumeKey, masterVolume);
        SetMixerVolume(MusicVolumeKey, musicVolume);
        SetMixerVolume(SfxVolumeKey, soundsVolume);
    }

    private void SaveAudioSettings()
    {
        PlayerPrefs.SetFloat(MasterVolumeKey, masterVolume);
        PlayerPrefs.SetFloat(MusicVolumeKey, musicVolume);
        PlayerPrefs.SetFloat(SfxVolumeKey, soundsVolume);
        PlayerPrefs.Save();
    }

    public void SaveGraphicsSettings(int resolutionIndex, int displayMode,  bool vSync, int fpsLimitIndex)
    {
        if (availableResolutions.Count == 0)
        {
            return;
        }
        
        selectedResolution = Mathf.Clamp(resolutionIndex, 0, availableResolutions.Count - 1);
        selectedDisplayMode = Mathf.Clamp(displayMode, 0, 2);
        vSyncEnabled = vSync;
        selectedFPSLimit = Mathf.Clamp(fpsLimitIndex, 0, fpsOptions.Length -1);
        
        Resolution resolution = availableResolutions[selectedResolution];
        
        savedResolutionWidth = resolution.width;
        savedResolutionHeight = resolution.height;
        
        PlayerPrefs.SetInt(ResolutionWidthKey, savedResolutionWidth);
        PlayerPrefs.SetInt(ResolutionHeightKey, savedResolutionHeight);
        PlayerPrefs.SetInt(DisplayModeKey, selectedDisplayMode);
        PlayerPrefs.SetInt(VSyncKey, vSyncEnabled ? 1 : 0);
        PlayerPrefs.SetInt(FPSLimitKey, selectedFPSLimit);
        PlayerPrefs.Save();
        
        ApplyGraphicsSettings();
    }
    
    private void ApplyGraphicsSettings()
    {
        if (availableResolutions.Count == 0)
        {
            return;
        }

        selectedResolution = Mathf.Clamp(selectedResolution, 0, availableResolutions.Count - 1);

        FullScreenMode mode = FullScreenMode.ExclusiveFullScreen;

        if (selectedDisplayMode == 1)
        {
            mode = FullScreenMode.FullScreenWindow;
        }
        else if (selectedDisplayMode == 2)
        {
            mode = FullScreenMode.Windowed;
        }
        
        Resolution resolution = availableResolutions[selectedResolution];
        Screen.SetResolution(resolution.width, resolution.height, mode);
        QualitySettings.vSyncCount = vSyncEnabled ? 1 : 0;
        Application.targetFrameRate = vSyncEnabled ? -1 : GetTargetFrameRate();
    }

    private int GetTargetFrameRate()
    {
        int fps = fpsOptions[selectedFPSLimit];
        return fps == 0 ? -1 : fps;
    }

    public void ResetSettings()
    {
        masterVolume = 1f;
        musicVolume = 0.6f;
        soundsVolume = 0.3f;
        selectedDisplayMode = 0;
        vSyncEnabled = true;
        selectedFPSLimit = 1;
        
        savedResolutionWidth = Screen.currentResolution.width;
        savedResolutionHeight = Screen.currentResolution.height;
        
        FindSelectedResolution();
        ApplyAudioSettings();
        SaveAudioSettings();
        SaveGraphicsSettings(selectedResolution, selectedDisplayMode, vSyncEnabled, selectedFPSLimit);
    }

    public int GetSavedFPSLimitIndex()
    {
        return selectedFPSLimit;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
    
}
