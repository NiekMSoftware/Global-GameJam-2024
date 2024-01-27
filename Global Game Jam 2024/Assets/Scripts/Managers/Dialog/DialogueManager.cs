using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject skipText;
    [SerializeField] private int skipTextLength;
    [SerializeField] private float normalTextSpeed;
    [SerializeField] private float textSpeedMultiplier;

    [SerializeField] Dialogues[] dialogues;

    private string currentText;
    private string currentText2;

    private bool canActivate = true;
    private bool renderText = true;

    int currentDialogue;

    private float textSpeed;

    [Serializable]
    class Dialogues
    {
        [TextArea(1, 3)]
        public string text;
        public int textLength;
        public bool autoHide;
        public bool startNextDialogue;
        public NPCDialogue npc;
    }


    private void Start()
    {
        dialoguePanel.SetActive(false);
        skipText?.SetActive(false);

        textSpeed = normalTextSpeed;
    }

    public void StartDialogue(int index)
    {
        if (!canActivate)
            return;

        RemoveText();

        canActivate = false;

        currentDialogue = index;

        renderText = true;

        dialoguePanel.SetActive(true);
        StartCoroutine(RenderText(index));

        float textDuration = dialogues[index].text.ToCharArray().Length * textSpeed + dialogues[index].textLength + 3;

        if (dialogues[index].autoHide)
        {
            Invoke(nameof(RemoveText), textDuration);
        }
        else
        {
            Invoke(nameof(ShowSkipText), textDuration);
        }
    }

    private void RemoveText()
    {
        dialoguePanel.SetActive(false);
        currentText = string.Empty;
        currentText2 = string.Empty;
        dialogueText.text = string.Empty;
    }

    private void RemoveSkipText()
    {
        skipText?.SetActive(false);
    }

    private void ShowSkipText()
    {
        skipText?.SetActive(true);
        Invoke(nameof(RemoveSkipText), skipTextLength);
    }

    IEnumerator RenderText(int index)
    {
        while (renderText)
        {
            for (int i = 0; i < dialogues[index].text.Length; i++)
            {
                if (!renderText)
                {
                    yield return null;
                    break;
                }

                currentText = dialogues[index].text;
                currentText2 += currentText[i];
                dialogueText.text = currentText2;

                yield return new WaitForSeconds(textSpeed);
            }

            renderText = false;
            canActivate = true;
            break;
        }

        yield return null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!canActivate && renderText)
            {
                textSpeed = normalTextSpeed / textSpeedMultiplier;
            }
            else
            {
                NormalSpeed();
                if (dialogues[currentDialogue].startNextDialogue)
                {
                    if (currentDialogue + 1 < dialogues[currentDialogue].text.Length && canActivate)
                    {
                        currentDialogue++;
                        StartDialogue(currentDialogue);
                    }
                }
                else
                {
                    dialogues[currentDialogue].npc.OnEndDialogue();
                    renderText = false;
                    canActivate = true;
                    RemoveText();
                    CancelInvoke();
                }
            }
        }
    }

    private void NormalSpeed()
    {
        textSpeed = normalTextSpeed;
    }

    public void StopDialogue()
    {
        dialogues[currentDialogue].npc.OnEndDialogue();
        renderText = false;
        canActivate = true;
        RemoveText();
        CancelInvoke();
    }
}