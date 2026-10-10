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

    private const string MasterVolumeKey = "Master";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SfxVolumeKey = "SoundsVolume";
    private const string ResolutionKey = "Resolution";
    private const string DisplayModeKey = "DisplayMode";
    private const string VSyncKey = "VSync";
    
    private float masterVolume = 1f;
    private float musicVolume = 0.6f;
    private float soundsVolume = 0.4f;
    private int selectedResolution = -1;
    private int selectedDisplayMode;
    private bool vSyncEnabled = true;

    private void Awake()
    {
        if (Instance != null &&  Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadSettings();
        ApplyAudioSettings();
        ApplyGraphicsSettings();
    }
    
    private void LoadSettings()
    {
        masterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, 1f);
        musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.6f);
        soundsVolume = PlayerPrefs.GetFloat(SfxVolumeKey, 0.4f);
        selectedResolution = PlayerPrefs.GetInt(ResolutionKey, -1);
        selectedDisplayMode = PlayerPrefs.GetInt(DisplayModeKey, 0);
        vSyncEnabled = PlayerPrefs.GetInt(VSyncKey, 1) == 1;
    }

    public void SetMasterVolume(float value)
    {
        masterVolume = Mathf.Clamp01(value);
        SetMixerVolume("Master", masterVolume);
        PlayerPrefs.SetFloat(MasterVolumeKey, masterVolume);
        PlayerPrefs.Save();
    }
    
    public void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Clamp01(value);
        SetMixerVolume("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat(MusicVolumeKey, musicVolume);
        PlayerPrefs.Save();
    }

    public void SetSoundsVolume(float value)
    {
        soundsVolume = Mathf.Clamp01(value);
        SetMixerVolume("SoundsVolume", soundsVolume);
        PlayerPrefs.SetFloat(SfxVolumeKey, soundsVolume);
        PlayerPrefs.Save();
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

    public void SaveGraphicsSettings(int resolutionIndex, int displayMode,  bool vSync)
    {
        selectedResolution = resolutionIndex;
        selectedDisplayMode = displayMode;
        vSyncEnabled = vSync;
        
        PlayerPrefs.SetInt(ResolutionKey, selectedResolution);
        PlayerPrefs.SetInt(DisplayModeKey, selectedDisplayMode);
        PlayerPrefs.SetInt(VSyncKey, vSyncEnabled ? 1 : 0);
        PlayerPrefs.Save();
        
        ApplyAudioSettings();
    }
    
    private void ApplyGraphicsSettings()
    {
        Resolution[] systemResolutions = Screen.resolutions;
        if (systemResolutions.Length == 0)
        {
            QualitySettings.vSyncCount = vSyncEnabled ? 1 : 0;
            return;
        }

        if (selectedResolution < 0 || selectedResolution >= systemResolutions.Length)
        {
            selectedResolution = systemResolutions.Length - 1;
        }
        
        FullScreenMode mode = FullScreenMode.ExclusiveFullScreen;

        if (selectedDisplayMode == 1)
        {
            mode = FullScreenMode.FullScreenWindow;
        }
        else if (selectedDisplayMode == 2)
        {
            mode = FullScreenMode.Windowed;
        }
        
        Resolution resolution = systemResolutions[selectedResolution];
        Screen.SetResolution(resolution.width, resolution.height, mode);
        QualitySettings.vSyncCount = vSyncEnabled ? 1 : 0;
    }
    
}
