using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public Dictionary<string, Dictionary<string, object>> vars = new Dictionary<string, Dictionary<string, object>>();

    public int playerHealth;
    public int playerMaxHealth = 10;
    public int playerMoney;

    public int day;

    public int numMealsSkipped = 0;
    public int infectedKilled = 0;
    public bool didLetInfectedIn = false;
    public int managementHappiness = 10;

    public int baseMoney = 5;
    public int moneyPerKill = 4;
    public List<(string, int)> extraMoneySource = new List<(string, int)>();

    public List<string> ReportsToAdd = new List<string>();

    public List<CharListWrapper> dailyCharacters = new List<CharListWrapper>();

    public List<Character> charactersChosen = new List<Character>();

    public GameObject winPanel;
    public GameplayManager gameplayManager;

    void Start()
    {
        playerHealth = playerMaxHealth;
    }

    public void GotoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void StartNewDay()
    {
        day++;
        if (day >= dailyCharacters.Count)
        {
            winPanel.SetActive(true);
        }
        else
        {
            gameplayManager.gameState = GameplayManager.GameState.WAITING_NEW;
            gameplayManager.charIndex = 0;
        }
    }
}

[System.Serializable]
public class CharListWrapper
{
    public List<Character> characters; 
    
    public Character this[int index]
    {
        get => characters[index];
        set => characters[index] = value;
    }
}

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            /*
            if (_instance == null)
                Debug.LogWarning(typeof(T).ToString() + " is NULL.");
            */
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            if (_instance != this)
            {
                Debug.LogWarning("Multiple mono-singleton instantiated.");
                Destroy(this.gameObject);
            }
            return;
        }
        else
        {
            _instance = this as T;
        }
    }
}