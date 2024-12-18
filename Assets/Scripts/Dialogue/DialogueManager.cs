using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class DialogueManager : MonoSingleton<DialogueManager>
{
    public TextAsset inkFile;

    public Story story;

    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public bool isTalking = false;

    public Choice selectedChoice;

    public GameObject dialogueOptionObject;
    public GameObject optionPanel;

    public Sprite progressionOngoingSprite;
    public Sprite progressionWaitingSprite;

    public float characterSpeed = .1f;

    private void Start()
    {
        if (inkFile)
        {
            LoadNewInkFile(inkFile);
        }
    }

    public void LoadNewInkFile(TextAsset f)
    {
        ClearOptions();
        LoadStory(new Story(f.text));
        ProcessAdvanceAction();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ProcessAdvanceAction();
        }
    }

    public void ProcessAdvanceAction()
    {
        if (story.canContinue)
        {
            AdvanceDialogue();

            if (story.currentChoices.Count != 0)
            {
                StartCoroutine(ShowChoices());
            }
        }
        else if (story)
        {
            FinishDialogue();
        }
    }

    public void ClearAll()
    {
        dialogueText.SetText("");
        ClearOptions();
        StopAllCoroutines();
    }

    public void AdvanceDialogue()
    {
        string currentSentence = story.Continue();
        ParseTags();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));
    }

    public void OverrideTypeSentence(string sentence)
    {
        ClearOptions();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
        yield return null;
    }

    IEnumerator ShowChoices()
    {
        Debug.Log("There are choices need to be made here!");
        List<Choice> _choices = story.currentChoices;

        for (int i = 0; i < _choices.Count; i++)
        {
            GameObject temp = Instantiate(dialogueOptionObject, optionPanel.transform);
            temp.GetComponent<SelectableOption>().text.text = _choices[i].text; 
            temp.GetComponent<SelectableOption>().dm = this;
            temp.GetComponent<SelectableOption>().element = _choices[i];
            temp.GetComponent<Button>().onClick.AddListener(() => { temp.GetComponent<SelectableOption>().Decide(); });
        }

        optionPanel.SetActive(true);

        yield return new WaitUntil(() => { return selectedChoice != null; });

        AdvanceFromDecision();
    }

    public void FinishDialogue()
    {
        Debug.Log("end");
    }


    public void SetDecision(object element)
    {
        selectedChoice = (Choice)element;
        story.ChooseChoiceIndex(selectedChoice.index);
    }
    
    public void AdvanceFromDecision()
    {
        optionPanel.SetActive(false);
        for (int i = 0; i < optionPanel.transform.childCount; i++)
        {
            Destroy(optionPanel.transform.GetChild(i).gameObject);
        }
        selectedChoice = null;
        ProcessAdvanceAction();
        ProcessAdvanceAction();
    }

    public void ClearOptions()
    {
        StopAllCoroutines();
        optionPanel.SetActive(false);
        for (int i = 0; i < optionPanel.transform.childCount; i++)
        {
            Destroy(optionPanel.transform.GetChild(i).gameObject);
        }
        selectedChoice = null;
    }

    private void LoadStory(Story _story)
    {
        story = _story;
    }

    public void ParseTags()
    {
        foreach (string t in story.currentTags)
        {
            string prefix = t.Split(' ')[0];
            string param = t.Split(' ')[1];
            string param2 = "";
            string param3 = "";

            if (t.Split(' ').Length > 2)
            {
                param2 = t.Split(' ')[2];
            }

            if (t.Split(' ').Length > 3)
            {
                param3 = t.Split(' ')[3];
            }

            switch (prefix.ToLower())
            {
                case "addMoney":
                    GameManager.Instance.playerMoney += int.Parse(param);
                    break;
                case "subMoney":
                    GameManager.Instance.playerMoney -= int.Parse(param);
                    if (GameManager.Instance.playerMoney < 0)
                    {
                        GameManager.Instance.playerMoney = 0;
                    }
                    break;
                case "addHealth":
                    GameManager.Instance.playerHealth += int.Parse(param);
                    break;
                case "subHealth":
                    GameManager.Instance.playerHealth -= int.Parse(param);
                    break;
                case "addManagement":
                    GameManager.Instance.managementHappiness += int.Parse(param);
                    break;
                case "subManagement":
                    GameManager.Instance.managementHappiness -= int.Parse(param);
                    break;
                case "set":
                    GameManager.Instance.vars[param][param2] = int.Parse(param3);
                    break;
                case "add":
                    GameManager.Instance.vars[param][param2] = (int)GameManager.Instance.vars[param][param2] + int.Parse(param3);
                    break;
                default:
                    break;
            }
        }
    }
}
