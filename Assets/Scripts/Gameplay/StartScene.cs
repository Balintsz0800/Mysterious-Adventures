using System;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject currentWindow;
    
    [Header("Play button")]
    [SerializeField] private GameObject mainButtons;
    [SerializeField] private GameObject playUI;
    
    [Header("Options button")]
    [SerializeField] private GameObject optionsMenu;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        anim.SetTrigger("Start");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (playUI.activeSelf && optionsMenu.activeSelf)
            {
                currentWindow.SetActive(false);
                mainButtons.SetActive(true);
            }
        }
    }

    public void Play()
    {
        mainButtons.SetActive(false);
        playUI.SetActive(true);
        currentWindow = playUI;
    }

    public void Options()
    {
        mainButtons.SetActive(false);
        optionsMenu.SetActive(true);
        currentWindow = optionsMenu;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
