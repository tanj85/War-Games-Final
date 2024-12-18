using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NightActionButton : MonoBehaviour
{
    public NightPanel nightPanel;
    public string dialogueName;
    public string charName;
    public TextMeshProUGUI text;

    public void OnClick()
    {
        nightPanel.handleConversation(charName, dialogueName);

        Destroy(gameObject);
    }
}
