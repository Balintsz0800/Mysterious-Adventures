using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class DayNightTimer : MonoBehaviour
{
    public static DayNightTimer Instance;

    [Header("Settings")]
    [SerializeField] private float dayLength = 300f;
    [SerializeField] private float currentTime = 10f;
    public TMP_Text timeText;
    private bool isNight;
    private string lastPeriod = "";

    public ClockUI clockui;
    private Streetlight[] streetlights;

    [Header("Light")]
    public Light2D targetLight;
    private float targetIntensity;
    [SerializeField] private float lightChangeSpeed = 1;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip day;
    public AudioClip night;
    [SerializeField] private float audioChangeSpeed = 0.5f;

    public string currentTimeText
    {
        get
        {
            int hour = Mathf.FloorToInt(currentTime);
            int minute = Mathf.FloorToInt((currentTime - hour) * 60);
            return hour.ToString("00") + ":" + minute.ToString("00");
        }
    }

    public int CurrentHour
    {
        get
        {
            return Mathf.FloorToInt(currentTime);
        }
    }

    public int CurrentMinute
    {
        get
        {
            return Mathf.FloorToInt((currentTime - CurrentHour) * 60f);
        }
    }

    private void Start()
    {
        if (audioSource != null)
        {
            audioSource.clip = day;
            audioSource.Play();
        }

        UpdateDayTime();
    }

    void Update()
    {
        currentTime += (24f / dayLength) * Time.deltaTime;

        if (currentTime >= 24f)
        {
            currentTime = 0f;
        }

        UpdateDayTime();
        if (targetLight != null)
            targetLight.intensity = Mathf.Lerp(targetLight.intensity, targetIntensity, lightChangeSpeed * Time.deltaTime);

        if (timeText != null)
            timeText.text = currentTimeText;
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        RefreshSceneReferences();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Do not carry gameplay-only objects back into the title screen.
        // Their scene references are destroyed during the load.
        if (scene.name == "StartScene")
        {
            Destroy(gameObject);
            return;
        }

        RefreshSceneReferences();
    }

    private void RefreshSceneReferences()
    {
        streetlights = FindObjectsByType<Streetlight>(FindObjectsSortMode.None);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (Instance == this)
            Instance = null;
    }

    void UpdateDayTime()
    {
        string period;

        if (currentTime >= 6f && currentTime < 8f)
        {
            period = "Morning";
        }
        else if (currentTime >= 8f && currentTime < 18f)
        {
            period = "MidDay";
        }
        else if (currentTime >= 18f && currentTime < 21f)
        {
            period = "Evening";
        }
        else
        {
            period = "Night";
        }

        if (period == lastPeriod)
        {
            return;
        }

        lastPeriod = period;

        if (period == "Morning")
        {
            Morning();
            if (clockui != null) clockui.Sunrise();
            foreach (Streetlight streetlight in streetlights)
            {
                streetlight.TurnOff();
            }
        }
        else if (period == "MidDay")
        {
            Day();
            if (clockui != null) clockui.MidDay();
        }
        else if (period == "Evening")
        {
            Evening();
            if (clockui != null) clockui.Sunset();
            foreach (Streetlight streetlight in streetlights)
            {
                streetlight.TurnOn();
            }
        }
        else
        {
            Night();
            if (clockui != null) clockui.Night();
        }
    }

    void Morning()
    {
        targetIntensity = 0.6f;
        SetDayMusic();
    }

    void Day()
    {
        targetIntensity = 1f;
    }

    void Evening()
    {
        targetIntensity = 0.4f;
        SetNightMusic();
    }

    void Night()
    {
        targetIntensity = 0.13f;
    }

    void SetDayMusic()
    {
        if (isNight)
        {
            isNight = false;
            StartCoroutine(ChangeMusic(day));
        }
    }

    void SetNightMusic()
    {
        if (!isNight)
        {
            isNight = true;
            StartCoroutine(ChangeMusic(night));
        }
    }

    private IEnumerator ChangeMusic(AudioClip newClip)
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= Time.deltaTime * audioChangeSpeed;
            yield return null;
        }

        audioSource.clip = newClip;
        audioSource.Play();

        while (audioSource.volume < 0.5f)
        {
            audioSource.volume += Time.deltaTime * audioChangeSpeed;
            yield return null;
        }
    }
}
