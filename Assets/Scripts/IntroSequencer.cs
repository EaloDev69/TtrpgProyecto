using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSequencer : MonoBehaviour
{
    public TextTyper typer;
    public TextAsset introText;
    private string[] lines;

    private bool skip = false;

    void Start()
    {
        lines = introText.text.Split('\n');
        StartCoroutine(PlaySequence());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
            skip = true;
    }

    IEnumerator PlaySequence()
    {
        foreach (string line in lines)
        {
            skip = false;

            Coroutine typing = StartCoroutine(typer.TypeText(line));

            yield return new WaitUntil(() => typer.isDone || skip);

            if (skip)
            {
                StopCoroutine(typing);
                typer.textUI.text = line;
                typer.isDone = true;
            }

            skip = false;
            yield return new WaitUntil(() => skip);

            yield return new WaitForSeconds(0.3f);
        }

        SceneManager.LoadScene("MainScene");
    }
}