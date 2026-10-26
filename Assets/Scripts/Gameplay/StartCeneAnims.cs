using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartCeneAnims : MonoBehaviour
{
    [Header("Animations")]
    [SerializeField] private Animator backgroundAnim;
    [SerializeField] private Animator mainButtonsAnim;
    [SerializeField] private Animator textAnim;
    
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    private IEnumerator Start()
    {
        SetButtonValue(false);
        
        backgroundAnim.SetTrigger("Play");
        yield return new WaitForSeconds(1.5f);
        
        textAnim.SetTrigger("Play");
        yield return new WaitForSeconds(1.5f);
        
        mainButtonsAnim.SetTrigger("Play");
        yield return new WaitForSeconds(1.5f);
        
        SetButtonValue(true);
    }

    private void SetButtonValue(bool value)
    {
        playButton.interactable = value;
        settingsButton.interactable = value;
        exitButton.interactable = value;
    }
}
