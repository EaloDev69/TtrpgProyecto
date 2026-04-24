using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerDialogue : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;

    private NPC nearbyNPC;
    private bool isDialogueActive;
    private bool isTyping;
    private int lineIndex;

    private float typingSpeed = 0.05f;

    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (nearbyNPC != null && !isDialogueActive)
            {
                StartDialogue();
            }
            else if (isDialogueActive)
            {
                // Si todavia esta escribiendo, muestra todo el texto de golpe
                if (isTyping)
                {
                    StopAllCoroutines();
                    dialogueText.text = nearbyNPC.DialogueLines[lineIndex];
                    isTyping = false;
                }
                else
                {
                    NextLine();
                }
            }
        }
    }

    public void SetNearbyNPC(NPC npc)
    {
        if (npc == null && isDialogueActive)
        {
            StopAllCoroutines();
            isDialogueActive = false;
            isTyping = false;
            dialoguePanel.SetActive(false);
        }
        nearbyNPC = npc;
    }

    private void StartDialogue()
    {
        isDialogueActive = true;
        lineIndex = 0;
        dialoguePanel.SetActive(true);
        StartCoroutine(TypeLine());
    }

    private void NextLine()
    {
        lineIndex++;

        if (lineIndex < nearbyNPC.DialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            isDialogueActive = false;
            dialoguePanel.SetActive(false);
            nearbyNPC = null;
        }
    }

    private IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = string.Empty;

        foreach (char c in nearbyNPC.DialogueLines[lineIndex])
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}
