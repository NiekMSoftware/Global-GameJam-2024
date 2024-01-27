using TMPro;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private Inventory inventory;
    [SerializeField] private TMP_Text interactionText;

    private bool interactable;

    private GameObject interactedObject;
    public Interactables interactables;

    public enum Interactables
    {
        npc,
        item,
        hedge
    }

    private void Awake()
    {
        interactionText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            interactionText.text = "E to talk with NPC";
            interactables = Interactables.npc;

            SetupInteractable(other);
        }
        else if (other.CompareTag("Item"))
        {
            interactionText.text = "E to pickup item";
            interactables = Interactables.item;

            SetupInteractable(other);
        }
        else if (other.CompareTag("Hedge"))
        {
            interactionText.text = "E to cut hedge";
            interactables = Interactables.hedge;

            SetupInteractable(other);
        }
    }

    private void SetupInteractable(Collider2D other)
    {
        interactionText.gameObject.SetActive(true);
        interactedObject = other.gameObject;
        interactable = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactable && interactedObject)
        {
            interactionText.text = string.Empty;
            interactionText.gameObject.SetActive(false);

            if (interactables == Interactables.npc)
            {
                dialogueManager.StartDialogue(interactedObject.GetComponent<NPCDialogue>().GetDialogueToActivate());
            }
            else if (interactables == Interactables.item)
            {
                inventory.AddItem(interactedObject.GetComponent<QuestItem>());
                Destroy(interactedObject);
            }
            else if (interactables == Interactables.hedge)
            {
                interactedObject.GetComponent<Hedge>().Use();
            }
            interactable = false;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            if (!interactable)
            {
                dialogueManager.StopDialogue();
            }

            ResetInteractables();
        }
        else if (other.CompareTag("Item"))
        {
            ResetInteractables();
        }
        else if (other.CompareTag("Hedge"))
        {
            ResetInteractables();
        }
    }

    private void ResetInteractables()
    {
        interactedObject = null;
        interactable = false;
        interactionText.text = string.Empty;
        interactionText.gameObject.SetActive(false);
    }
}