using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightTimer : MonoBehaviour
{
    public static DayNightTimer Instance;

    [Header("Settings")]
    [SerializeField] private float dayLength = 300f;
    [SerializeField] private float currentTime = 10f;
    public TMP_Text timeText;
    private bool isNight;
    
    [Header("Light")]
    public Light2D light;
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

    private void Start()
    {
        audioSource.clip = day;
        audioSource.Play();
    }

    void Update()
    {
        currentTime += (24f / dayLength) * Time.deltaTime;

        if (currentTime >= 24f)
        {
            currentTime = 0f;
        }
        
        UpdateDayTime();
        light.intensity = Mathf.Lerp(light.intensity, targetIntensity, lightChangeSpeed * Time.deltaTime);
        timeText.text = currentTimeText;
    }

    void Awake()
    {
        Instance = this;
    }

    void UpdateDayTime()
    {
        if (currentTime >= 6f && currentTime < 8f)
        {
            Morning();
        }
        else if (currentTime >= 8f && currentTime < 18f)
        {
            Day();
        }
        else if (currentTime >= 18f && currentTime < 21f)
        {
            Evening();
        }
        else
        {
            Night();
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
    
    void  SetNightMusic()
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
    