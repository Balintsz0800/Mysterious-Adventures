using System;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    private GameObject player;
    public GameObject inv;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
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
            inv.SetActive(false);
            player.GetComponent<PlayerMovement>().enabled = false;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (pauseMenu.activeSelf)
        {
            inv.SetActive(true);
            player.GetComponent<PlayerMovement>().enabled = true;
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

    public void SaveButton()
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.SaveWorld();
        }
    }
    
    public void Quit()
    {    
        SaveManager.Instance.SaveWorld();
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
    }
}
