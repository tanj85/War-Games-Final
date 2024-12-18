using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject contextPanel;

    public void OpenContext()
    {
        contextPanel.SetActive(true);
    }

    public void GotoGameScene()
    {
        SceneManager.LoadScene("OutsideGameplay");
    }
}
