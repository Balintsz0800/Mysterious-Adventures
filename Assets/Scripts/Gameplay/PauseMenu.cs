using System;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ESCMEnu();
        }
    }

    void ESCMEnu()
    {
        if (!pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
