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
    private string lastPeriod = "";

    [Header ("Season")]
    public string[] seasons = {"Spring", "Summer", "Autumn", "Winter"};
    public int currentSeason = 0;
    public int currentDay = 1;
    [SerializeField] private int daysPerSeason = 28;    

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

    public string CurrentSeason
    {
        get
        {
            return seasons[currentSeason];
        }
    }

    private void Start()
    {
        audioSource.clip = day;
        audioSource.Play();
        UpdateDayTime();
    }

    private void Update()
    {
        currentTime += (24f / dayLength) * Time.deltaTime;

        if (currentTime >= 24f)
        {
            currentTime = 0f;
            NextDay();
        }

        UpdateDayTime();
        targetLight.intensity = Mathf.Lerp(targetLight.intensity, targetIntensity, lightChangeSpeed * Time.deltaTime);
        timeText.text = currentTimeText;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        streetlights = FindObjectsByType<Streetlight>(FindObjectsSortMode.None);
    }

    private void NextDay()
    {
        currentDay++;

        if (currentDay > daysPerSeason)
        {
            currentDay = 1;
            currentSeason = (currentSeason + 1) % seasons.Length;
        }
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
            clockui.Sunrise();
            foreach (Streetlight streetlight in streetlights)
            {
                streetlight.TurnOff();
            }
        }
        else if (period == "MidDay")
        {
            Day();
            clockui.MidDay();
        }
        else if (period == "Evening")
        {
            Evening();
            clockui.Sunset();
            foreach (Streetlight streetlight in streetlights)
            {
                streetlight.TurnOn();
            }
        }
        else
        {
            Night();
            clockui.Night();
        }
    }

    private void Morning()
    {
        targetIntensity = 0.6f;
        SetDayMusic();
    }

    private void Day()
    {
        targetIntensity = 1f;
    }

    private void Evening()
    {
        targetIntensity = 0.4f;
        SetNightMusic();
    }

    private void Night()
    {
        targetIntensity = 0.13f;
    }

    private void SetDayMusic()
    {
        if (isNight)
        {
            isNight = false;
            StartCoroutine(ChangeMusic(day));
        }
    }

    private void SetNightMusic()
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
