using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NightReport : MonoBehaviour
{
    public GameObject reportPrefab;
    public Transform reportParent;

    public GameObject closeButton;

    public List<GameObject> reportObjects = new List<GameObject>();
    public int reportIndex;
    // Start is called before the first frame update
    void OnEnable()
    {
        reportObjects.Clear();
        reportIndex = 0;
        AddReport(FormatEarnings());
        AddReport(FormatHealth());
        if (GameManager.Instance.numMealsSkipped == 1)
        {
            AddReport("You skipped last night's meal. That'll take a toll on your health");
        }
        else if (GameManager.Instance.numMealsSkipped > 1)
        {
            AddReport($"You've skipped {GameManager.Instance.numMealsSkipped} meals. That'll take a toll on your health");
        }
        AddReport(FormatManagementHappiness());

        foreach (string rep in GameManager.Instance.ReportsToAdd)
        {
            AddReport(rep);
        }
        GameManager.Instance.ReportsToAdd.Clear();
    }

    public void AddReport(string text)
    {
        reportObjects.Add(Instantiate(reportPrefab, reportParent));
        reportObjects[reportObjects.Count - 1].GetComponent<TextMeshProUGUI>().SetText(text);
    }

    string FormatEarnings()
    {
        string s = "Money\n" +
               "---------------\n" +
               $"+{GameManager.Instance.baseMoney} base\n" +
               $"+{GameManager.Instance.moneyPerKill} x{GameManager.Instance.infectedKilled} infected killed\n";
        foreach ((string, int) p in GameManager.Instance.extraMoneySource)
        {
            s += $"+{p.Item2} from {p.Item1}\n";
        }
        s += $"Total ${GameManager.Instance.playerMoney}";

        return s;
    }

    string FormatManagementHappiness()
    {
        if (GameManager.Instance.managementHappiness >= 10)
        {
            return "The Higher-Ups are happy with your performance.";
        }
        else if (GameManager.Instance.managementHappiness >= 5)
        {
            return "The Higher-Ups are questioning your capability.";
        }
        else if (GameManager.Instance.managementHappiness >= 1)
        {
            return "You're on thin ice.";
        }
        else
        {
            return "You've been fired";
        }
    }

    string FormatHealth()
    {
        if (GameManager.Instance.playerHealth >= 10)
        {
            return "You're feeling healthy!";
        }
        else if (GameManager.Instance.playerHealth >= 5)
        {
            return "You're feeling a little sick.";
        }
        else if (GameManager.Instance.playerHealth >= 1)
        {
            return "You can feel your body is in critical condition.";
        }
        else
        {
            return "You died.";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (reportIndex >= reportObjects.Count)
            {
                closeButton.SetActive(true);
            }
            else
            {
                reportObjects[reportIndex].SetActive(true);
            }

            reportIndex++;
        }
    }
}
