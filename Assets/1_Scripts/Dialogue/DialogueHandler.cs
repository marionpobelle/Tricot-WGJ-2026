using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Sirenix.OdinInspector;

public class DialogueHandler : MonoBehaviour
{
    [Title("PARAMETERS")]
    [SerializeField] private float _initialWaitTime = 6f;
    [SerializeField] private float _minWaitTime = 4f;
    [SerializeField] private float _maxWaitTime = 10f;
    [SerializeField] private float _dialogueWaitTime = 8f;

    [Title("REFERENCES")]
    public List<string> AvailableNodeTitles = new List<string>();
    public DialogueRunner DialogueRunner;

    private Coroutine _dialoguesCoroutine;

    public static DialogueHandler Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        GetAllDialogueNodes();
        _dialoguesCoroutine = StartCoroutine(PlayDialoguesCoroutine());
    }

    private void GetAllDialogueNodes()
    {
        IEnumerable<string> NodeNames = DialogueRunner.Dialogue.NodeNames;
        foreach (var item in NodeNames)
        {
            AvailableNodeTitles.Add(item.ToString());
        }
        AvailableNodeTitles.RemoveAll(x => x == "Dialogues");
    }

    private string GetRandomDialogue()
    {
        if(AvailableNodeTitles.Count <= 0)
        {
            Debug.LogError("No more dialogue available");
            return "No more dialogue available!";
        }
        string result = AvailableNodeTitles[UnityEngine.Random.Range(0, AvailableNodeTitles.Count)];
        AvailableNodeTitles.RemoveAll(x => x == result);
        return result;
    }

    private IEnumerator PlayDialoguesCoroutine()
    {
        string dialogueToCall = "";
        yield return new WaitForSeconds(_initialWaitTime);
        while (AvailableNodeTitles.Count > 0)
        {
            dialogueToCall = GetRandomDialogue();
            DialogueRunner.StartDialogue(dialogueToCall);
            yield return new WaitForSeconds(_dialogueWaitTime);
            if(DialogueRunner.IsDialogueRunning)
            {
                DialogueRunner.Dialogue.Stop();
                PopupHandler.Instance.SpawnPopup();
            }
            yield return new WaitForSeconds(UnityEngine.Random.Range(_minWaitTime, _maxWaitTime));
        }
        //TO DO: No more dialogue, texte affiche a l'ecran, on peut cliquer sur un bouton pour etre renvoyer au main menu
        Debug.Log("No more dialogue to be played, end of coroutine!");
    }

    [Title("DEBUG")]
    [Button("DEBUG: Remove specific dialogue title from list")]
    public void DebugRemoveSpecificDialogueFromList(string title)
    {
        if (AvailableNodeTitles.Count > 0)
        {
            AvailableNodeTitles.RemoveAll(x => x == title);
        }
    }

    [Button("DEBUG: Remove dialogue title from list")]
    public void DebugRemoveDialogueFromList()
    {
        if (AvailableNodeTitles.Count > 0)
        {
            AvailableNodeTitles.RemoveAt(0);
        }
    }
}
