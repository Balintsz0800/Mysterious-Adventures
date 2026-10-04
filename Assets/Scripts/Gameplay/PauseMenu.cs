using System;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !optionsMenu.activeSelf)
        {
            ESCMEnu();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && optionsMenu.activeSelf)
        {
            OptionsMenu();
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

    public void Resume()
    {
        ESCMEnu();
    }

    public void OptionsMenu()
    {
        if (!optionsMenu.activeSelf)
        {
            optionsMenu.SetActive(true);
        }
        else if (optionsMenu.activeSelf)
        {
            optionsMenu.SetActive(false);
        }
    }
    
    public void Quit()
    {    
        SaveManager.Instance.SaveWorld();
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
    }
}
