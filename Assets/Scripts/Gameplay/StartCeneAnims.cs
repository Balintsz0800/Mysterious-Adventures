using UnityEngine;

public class StartCeneAnims : MonoBehaviour
{
    [Header("Animations")]
    [SerializeField] private Animator backgroundAnim;
    [SerializeField] private Animator mainButtonsAnim;
    [SerializeField] private Animator textAnim;
    
    void Start()
    {
        backgroundAnim.SetTrigger("Play");
    }

    public void BackgroundAnimEnd()
    {
        mainButtonsAnim.SetTrigger("Play");
    }
    public void MainButtonAnimEnd()
    {
        textAnim.SetTrigger("Play");
    }
}
