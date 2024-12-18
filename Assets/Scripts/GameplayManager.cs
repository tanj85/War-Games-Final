using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    public Character currentCharacter;

    public int charIndex = 0;

    public GameObject dayEndButton;

    public GameObject nightPanel;

    public Image charPortrait;

    public float shootMaxCD = 5f;
    public float shootCurrentTimer;

    public enum GameState
    {
        WAITING_NEW,
        IN_DIALOGUE,
        SHOOTING,
        DAY_END,
        NIGHT,
    }

    public GameState gameState;

    void Start()
    {
        charIndex = -1;
        gameState = GameState.WAITING_NEW;
    }

    void Update()
    {
        if (gameState == GameState.DAY_END)
        {
            dayEndButton.SetActive(true);
        }
        else
        {
            dayEndButton.SetActive(false);
        }

        if (currentCharacter)
        {
            charPortrait.sprite = currentCharacter.charImage;
        }
        else
        {
            charPortrait.sprite = null;
        }

        if (gameState == GameState.WAITING_NEW && 
            charIndex >= GameManager.Instance.dailyCharacters[GameManager.Instance.day].characters.Count - 1)
        {
            gameState = GameState.DAY_END;
        }

        if (gameState == GameState.SHOOTING)
        {
            shootCurrentTimer -= Time.deltaTime;

            if (shootCurrentTimer <= 0)
            {
                gameState = GameState.WAITING_NEW;
                currentCharacter = null;
                DialogueManager.Instance.OverrideTypeSentence("They ran away...");
            }
        }

        //switch (gameState)
        //{
        //    case GameState.DAY_END:
        //        buttonText.transform.parent.gameObject.SetActive(true);
        //        gunButton.SetActive(false);
        //        buttonText.SetText("end day");
        //        break;
        //    case GameState.IN_DIALOGUE:
        //        buttonText.transform.parent.gameObject.SetActive(false);
        //        gunButton.SetActive(true);
        //        break;
        //    case GameState.SHOOTING:
        //        buttonText.transform.parent.gameObject.SetActive(true);
        //        gunButton.SetActive(false);
        //        buttonText.SetText("let them go");
        //        break;
        //    case GameState.WAITING_NEW:
        //        buttonText.transform.parent.gameObject.SetActive(true);
        //        gunButton.SetActive(false);
        //        buttonText.SetText("call next");
        //        break;
        //    default:
        //        break;

        //}
    }

    public void NextCharacter()
    {
        charIndex++; 
        if (charIndex >= GameManager.Instance.dailyCharacters[GameManager.Instance.day].characters.Count)
        {
            gameState = GameState.DAY_END;
            return;
        }
        else
        {
            currentCharacter = GameManager.Instance.dailyCharacters[GameManager.Instance.day][charIndex];
            BeginConvo();
        }
    }

    public void BeginConvo()
    {
        if (currentCharacter)
        {
            DialogueManager.Instance.LoadNewInkFile(currentCharacter.dictInkStories["checkpoint"]);
        }
    }

    public void DecisionButton(bool didAccept)
    {
        if (gameState == GameState.IN_DIALOGUE && currentCharacter)
        {
            if (didAccept)
            {
                DialogueManager.Instance.LoadNewInkFile(currentCharacter.dictInkStories["accept"]);
                GameManager.Instance.charactersChosen.Add(currentCharacter);
            }
            else
            {
                DialogueManager.Instance.LoadNewInkFile(currentCharacter.dictInkStories["deny"]);
            }

            StartCoroutine(CharacterLeave());
        }
    }

    public IEnumerator CharacterLeave()
    {
        gameState = GameState.WAITING_NEW;
        yield return new WaitForSeconds(2f);
        currentCharacter = null;
    }

    public void BellButton()
    {
        if (gameState == GameState.WAITING_NEW)
        {
            NextCharacter();
            gameState = GameState.IN_DIALOGUE;
        }
    }

    public void GunButton()
    {
        if (gameState == GameState.IN_DIALOGUE)
        {
            gameState = GameState.SHOOTING;
            shootCurrentTimer = shootMaxCD;
            if (currentCharacter)
            {
                DialogueManager.Instance.LoadNewInkFile(currentCharacter.dictInkStories["shoot"]);
            }
        }
    }

    public void ShootPerson()
    {
        if (gameState == GameState.SHOOTING)
        {
            if (currentCharacter.isInfected)
            {
                GameManager.Instance.infectedKilled++;
            }

            currentCharacter = null;
            gameState = GameState.WAITING_NEW;
            DialogueManager.Instance.ClearAll();
        }
    }

    public void EndDayButton()
    {
        if (gameState == GameState.DAY_END)
        {
            gameState = GameState.NIGHT;
            nightPanel.SetActive(true);
            DialogueManager.Instance.ClearAll();
        }
    }
}
