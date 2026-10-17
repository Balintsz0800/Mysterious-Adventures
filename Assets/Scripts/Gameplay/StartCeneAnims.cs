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
        textAnim.SetTrigger("Play");
    }
    public void TextAnimEnd()
    {
        mainButtonsAnim.SetTrigger("Play");
    }
}
