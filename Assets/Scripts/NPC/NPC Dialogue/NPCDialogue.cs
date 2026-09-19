using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public GameObject dialogueUI;
    [SerializeField] private TMP_Text dialogueText;
    
    [SerializeField] private float dialogueSpeed = 3f;
    
    [Header("Dialogue")]
    [TextArea(2, 5)]
    [SerializeField] private string[] dialogueLines;
    
    private int currentLine = 0;
    private bool isTyping = false;
    public bool dialogueActive = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        if (dialogueUI != null)
        {
            dialogueUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (!dialogueActive)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Next();
        }
    }

    public void StartDialogue()
    {
        if (dialogueLines == null || dialogueLines.Length == 0)
        {
            return;
        }
        
        dialogueActive  = true;
        currentLine = 0;
        
        dialogueUI.SetActive(true);

        ShowCurrentLine();
    }

    private void Next()
    {
        if (isTyping)
        {
            FinishTyping();
            return;
        }

        if (currentLine < dialogueLines.Length - 1)
        {
            currentLine++;
            ShowCurrentLine();
        }
        else
        {
            CloseDialogue();
        }
    }

    private void FinishTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        
        dialogueText.text = dialogueLines[currentLine];
        isTyping = false;
    }

    private void ShowCurrentLine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText(dialogueLines[currentLine]));
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in text)
        {
            dialogueText.text += letter;
            
            yield return new WaitForSeconds(dialogueSpeed);
        }
        
        isTyping = false;
        typingCoroutine = null;
    }

    private void CloseDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        
        isTyping = false;
        dialogueUI.SetActive(false);
        dialogueText.text = "";
    }
}