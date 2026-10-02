using UnityEngine;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private GameObject mainButtons;
    [SerializeField] private GameObject playUI;
    
    public void Play()
    {
        mainButtons.SetActive(false);
        playUI.SetActive(true);
    }
}
