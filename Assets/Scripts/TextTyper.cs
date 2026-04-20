using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class TextTyper : MonoBehaviour
{
    public Text textUI;
    public float typingSpeed = 0.05f;
    public bool isDone = false;

    public IEnumerator TypeText(string message)
    {
        isDone = false;
        textUI.text = "";
        foreach (char letter in message)
        {
            textUI.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isDone = true;
    }
}