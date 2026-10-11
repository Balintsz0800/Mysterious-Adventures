using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Loading : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    [SerializeField] private float timeToNextScene = 11.7f;
    [SerializeField] private float timer = 0f;
    
    void Start()
    {
        videoPlayer.Play();
        timer = 0f;
    }
    
    void Update()
    {
        if (timer <= timeToNextScene)
        {
            timer += Time.deltaTime;
        }

        if (timer >= timeToNextScene)
        {
            timer = 0f;
            UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
        }
    }
}
