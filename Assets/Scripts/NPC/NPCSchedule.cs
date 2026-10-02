using System;
using System.Collections.Generic;
using UnityEngine;

public class NPCSchedul : MonoBehaviour
{
    [Serializable]
    public class ScheduleEntry
    {
        [Range(0, 23)]
        public int hour;

        [Range(0, 59)]
        public int minute;

        public Transform destination;
        public bool randomMovement;
        public float randomRadius = 2f;
    }

    [Header("Schedule")]
    public List<ScheduleEntry> schedule = new List<ScheduleEntry>();

    private NPCMovement movement;
    private ScheduleEntry currentEntry;

    private void Start()
    {
        movement = GetComponent<NPCMovement>();
    }

    private void Update()
    {
        if (DayNightTimer.Instance == null)
        {
            return;
        }

        int currentHour = DayNightTimer.Instance.CurrentHour;
        int currentMinute = DayNightTimer.Instance.CurrentMinute;

        ScheduleEntry newEntry = GetCurrentSchedule(currentHour, currentMinute);

        if (newEntry != currentEntry)
        {
            currentEntry = newEntry;

            if (currentEntry != null)
            {
                movement.GoTo(
                    currentEntry.destination,
                    currentEntry.randomMovement,
                    currentEntry.randomRadius
                );
            }
        }
    }

    private ScheduleEntry GetCurrentSchedule(int hour, int minute)
    {
        ScheduleEntry result = null;
        int currentTime = hour * 60 + minute;

        foreach (ScheduleEntry entry in schedule)
        {
            int entryTime = entry.hour * 60 + entry.minute;

            if (entryTime <= currentTime)
            {
                result = entry;
            }
        }

        return result;
    }
}