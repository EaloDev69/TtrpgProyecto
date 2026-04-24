using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private string[] dialogueLines;

    public string[] DialogueLines => dialogueLines;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerDialogue>().SetNearbyNPC(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerDialogue>().SetNearbyNPC(null);
        }
    }
}