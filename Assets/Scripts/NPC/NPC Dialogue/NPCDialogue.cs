using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class NPCDialogueEntry
{
    public string dialogueName;
    [TextArea(2, 5)] public string[] dialogueLines;
    public DialogueQuestRequirement questRequirement;
    public QuestData requiredQuest;
    public bool givesQuest;
    public QuestData questToGive;
}

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private float dialogueSpeed = 0.03f;
    [SerializeField] private List<NPCDialogueEntry> dialogues = new List<NPCDialogueEntry>();

    private int currentDialogue;
    private int currentLine;
    private bool dialogueActive;
    private bool isTyping;
    private Coroutine typingCoroutine;

    public bool IsDialogueActive => dialogueActive;

    private void Start()
    {
        if (dialogueUI != null)
        {
            dialogueUI.SetActive(false);
        }
    }

    public void StartDialogue()
    {
        if (dialogueActive)
        {
            return;
        }

        int dialogueIndex = GetAvailableDialogue();

        if (dialogueIndex == -1)
        {
            Debug.Log("No dialogue available for " + gameObject.name);
            return;
        }

        currentDialogue = dialogueIndex;
        currentLine = 0;
        dialogueActive = true;

        dialogueUI.SetActive(true);
        ShowLine();
    }

    private void Update()
    {
        if (!dialogueActive)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Next();
        }
    }

    private int GetAvailableDialogue()
    {
        for (int i = 0; i < dialogues.Count; i++)
        {
            if (IsDialogueAvailable(dialogues[i]))
            {
                return i;
            }
        }

        return -1;
    }

    private bool IsDialogueAvailable(NPCDialogueEntry dialogue)
    {
        if (dialogue.questRequirement == DialogueQuestRequirement.None)
        {
            return true;
        }

        if (dialogue.requiredQuest == null)
        {
            return false;
        }

        if (QuestManager.Instance == null)
        {
            return false;
        }

        QuestState state = QuestManager.Instance.GetQuestState(dialogue.requiredQuest);

        if (dialogue.questRequirement == DialogueQuestRequirement.NotStarted)
        {
            return state == QuestState.NotStarted;
        }

        if (dialogue.questRequirement == DialogueQuestRequirement.Active)
        {
            return state == QuestState.Active;
        }

        if (dialogue.questRequirement == DialogueQuestRequirement.Complated)
        {
            return state == QuestState.Complated;
        }

        if (dialogue.questRequirement == DialogueQuestRequirement.Failed)
        {
            return state == QuestState.Failed;
        }

        return false;
    }

    private void Next()
    {
        if (isTyping)
        {
            FinishTyping();
            return;
        }

        NPCDialogueEntry dialogue = dialogues[currentDialogue];

        if (currentLine < dialogue.dialogueLines.Length - 1)
        {
            currentLine++;
            ShowLine();
        }
        else
        {
            FinishDialogue();
        }
    }

    private void ShowLine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText(dialogues[currentDialogue].dialogueLines[currentLine]));
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

    private void FinishTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        dialogueText.text = dialogues[currentDialogue].dialogueLines[currentLine];
        isTyping = false;
    }

    private void FinishDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        NPCDialogueEntry dialogue = dialogues[currentDialogue];

        dialogueActive = false;
        isTyping = false;

        dialogueUI.SetActive(false);
        dialogueText.text = "";

        if (dialogue.givesQuest && dialogue.questToGive != null)
        {
            QuestManager.Instance.StartQuest(dialogue.questToGive);
        }
    }
}