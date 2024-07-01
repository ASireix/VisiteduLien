using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Dialogue : MonoBehaviour
{
    [Header("Visual representation")]
    [SerializeField] TextMeshProUGUI uiText;
    public DialogueBox dialogueBox;
    [SerializeField] float typeSpeed;

    [Header("Events")]
    [SerializeField] CustomDialogueEvent[] customEvents;
    Dictionary<string, UnityEvent<Dialogue>> _customEventDico = new Dictionary<string, UnityEvent<Dialogue>>();

    [SerializeField] DialogueData dialogueData;
    Queue<string> _dialogue = new Queue<string>();

    bool _isTyping;
    string _textTyped;
    Coroutine _typingCoroutine;

    [System.NonSerialized]
    public UnityEvent onSentenceTyped = new UnityEvent();
    public UnityEvent onDialogueComplete = new UnityEvent();

    // Start is called before the first frame update
    void Awake()
    {
        #region Init Dico
        for (int i = 0; i < customEvents.Length; i++)
        {
            CustomDialogueEvent cEvent = customEvents[i];
            _customEventDico.TryAdd(cEvent.eventIdentifier, cEvent.eventToTrigger);
        }
        #endregion
        _dialogue.Clear();

    }

    public void TriggerDialogue()
    {
        _isTyping = false;
        dialogueBox.UseDialogueBox(this);
        ReadFile();
        uiText.text = "";
        OpenDialogueBox();
        PrintDialogue();
    }

    public void AdvanceDialogue()
    {
        if (_dialogue.Count == 0) { return; }
        PrintDialogue();
    }

    void PrintDialogue()
    {
        
        if (_isTyping)
        {
            StopCoroutine(_typingCoroutine);
            uiText.text = _textTyped;
            uiText.maxVisibleCharacters = uiText.textInfo.characterCount;
            _isTyping = false;
            onSentenceTyped?.Invoke();
            return;
        }

        if (_dialogue.Peek().Contains("EndQueue"))
        {
            _dialogue.Dequeue();
            EndDialogue();
        }
        else if (_dialogue.Peek().Contains("[EVENT="))
        {
            string eventName = _dialogue.Peek();
            eventName = _dialogue.Dequeue().Substring(eventName.IndexOf("=") + 1, eventName.IndexOf("]") - (eventName.IndexOf("=") + 1));
            if (_customEventDico.TryGetValue(eventName, out UnityEvent<Dialogue> v))
            {
                v?.Invoke(this);
            }
            PrintDialogue();
        }
        else
        {
            _typingCoroutine = StartCoroutine(TypeSentence(_dialogue.Dequeue()));
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        _textTyped = sentence;
        _isTyping = true;

        uiText.text = sentence;
        uiText.maxVisibleCharacters = 0;

        char[] letters = sentence.ToCharArray();
        int index = 0;
        while (index < letters.Length)
        {
            uiText.maxVisibleCharacters++;
            index++;
            yield return new WaitForSeconds((1f / typeSpeed) / 10f);
        }

        _isTyping = false;
        onSentenceTyped?.Invoke();
    }

    public void EndDialogue()
    {
        CloseDialogueBox();
        dialogueBox.DisposeDialogueBox(this);
        onDialogueComplete?.Invoke();
    }

    void ReadFile(string newText = "")
    {
        string txt = "";
        if (!string.IsNullOrEmpty(newText))
        {
            txt = newText;
        }
        else
        {
            txt = dialogueData.GetText();
        }
        txt = txt.Replace("\r", "");
        string[] lines = txt.Split(System.Environment.NewLine.ToCharArray());

        string lastLine = "";

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            if (string.IsNullOrWhiteSpace(lastLine) && !string.IsNullOrWhiteSpace(line))
            {
                if (lines[i].StartsWith("["))
                {
                    string special = line.Substring(0, line.IndexOf("]") + 1);
                    string curr = line.Substring(line.IndexOf("]") + 1);
                    _dialogue.Enqueue(special);
                    lastLine += curr;
                }
                else
                {
                    lastLine += line;
                }
            }
            else if (string.IsNullOrWhiteSpace(line) && !string.IsNullOrWhiteSpace(lastLine))
            {
                _dialogue.Enqueue(lastLine);
                lastLine = "";
            }
            else
            {
                lastLine += line;
            }
        }

        if (!string.IsNullOrWhiteSpace(lastLine))
        {
            _dialogue.Enqueue(lastLine);
            lastLine = "";
        }

        _dialogue.Enqueue("EndQueue");
    }

    public void OpenDialogueBox()
    {
        dialogueBox.gameObject.SetActive(false);
        LeanTween.scale(dialogueBox.gameObject, Vector3.zero, 0f).setOnComplete(() =>
        {
            dialogueBox.gameObject.SetActive(true);
            LeanTween.scale(dialogueBox.gameObject, Vector3.one, 1f).setEaseInBounce();
        });
    }

    public void CloseDialogueBox()
    {
        LeanTween.scale(dialogueBox.gameObject, Vector3.zero, 1f).setEaseOutBounce().setOnComplete(() =>
        {
            dialogueBox.gameObject.SetActive(false);
        });

    }
}
