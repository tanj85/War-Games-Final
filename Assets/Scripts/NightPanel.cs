using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NightPanel : MonoBehaviour
{
    public GameObject nightPanel;
    public GameObject nightReport;

    public TextMeshProUGUI statsText;

    public int dailyEnergy = 2;

    public GameObject losePanel;
    public TextMeshProUGUI loseText;

    public Transform nightActionsParent;

    public GameObject charTalkPrefab;

    public Dictionary<string, int> charDialogueProgress = new Dictionary<string, int>();

    public void ClosePanel()
    {
        nightPanel.SetActive(false);
        GameManager.Instance.StartNewDay();
    }

    private void Update()
    {
        statsText.SetText($"Money: {GameManager.Instance.playerMoney}\n" +
            $"Energy: {dailyEnergy}");
    }

    public void handleConversation(string character, string convoId)
    {
        Character foundChar = GameManager.Instance.charactersChosen.Find(x => { return x.charName == character; });

        if (foundChar)
        {
            DialogueManager.Instance.LoadNewInkFile(foundChar.dictInkStories[convoId]);
        }
    }

    public void ExtraWork()
    {
        if (dailyEnergy > 0)
        {
            dailyEnergy--;
            GameManager.Instance.playerMoney += 3;
            GameManager.Instance.managementHappiness += 1;
            GameManager.Instance.playerHealth -= 1;
            DialogueManager.Instance.OverrideTypeSentence("You earn $3 doing extra work.\n" +
                "The continuous work takes a toll on your body, but the higher-ups are pleased to see you so diligent.");
        }
    }

    public void Rest()
    {
        if (dailyEnergy > 0)
        {
            dailyEnergy--;
            GameManager.Instance.playerHealth += 2;
            DialogueManager.Instance.OverrideTypeSentence("You get some extra rest, and your body feels refreshed.");
        }
    }
    public void EatFood()
    {
        if (GameManager.Instance.playerMoney > 5)
        {
            GameManager.Instance.playerMoney -= 5;
            GameManager.Instance.numMealsSkipped = 0;
            DialogueManager.Instance.OverrideTypeSentence("The meal cost you 5 dollars. It's good to have a full stomach.");
        }
    }

    void OnEnable()
    {
        nightReport.SetActive(true);
        for (int i = nightActionsParent.childCount - 1; i >= 3; i--)
        {
            Transform child = nightActionsParent.GetChild(i);
            Destroy(child.gameObject);
        }

        string sentence = "Characters\n";
        foreach (Character c in GameManager.Instance.charactersChosen)
        {
            sentence += c.charName;
            sentence += "\n";

            if (!charDialogueProgress.ContainsKey(c.charName))
            {
                charDialogueProgress.Add(c.charName, 0);
            }

            GameObject go = Instantiate(charTalkPrefab, nightActionsParent);
            go.GetComponent<NightActionButton>().charName = c.charName;
            go.GetComponent<NightActionButton>().dialogueName = 
                c.nightConversationsProgression[charDialogueProgress[c.charName]];
            go.GetComponent<NightActionButton>().nightPanel = this;
            go.GetComponent<NightActionButton>().text.SetText($"Talk to {c.charName}");

        }
        DialogueManager.Instance.OverrideTypeSentence(sentence);



        dailyEnergy = 2;

        GameManager.Instance.playerHealth -= GameManager.Instance.numMealsSkipped;

        if (GameManager.Instance.playerHealth <= 0)
        {
            losePanel.SetActive(true);
            loseText.SetText("Weak and weary, you finally succumb to your injuries. Game Over");
        }

        if (GameManager.Instance.managementHappiness <= 0)
        {
            losePanel.SetActive(true);
            loseText.SetText("You've misstepped one too many times. The Commanders have decided to exile you, leaving" +
                "you to fend for yourself. Game Over.");
        }

        GameManager.Instance.numMealsSkipped += 1;
    }
}
