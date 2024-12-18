using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectableOption : MonoBehaviour
{
    public object element;
    public DialogueManager dm;

    public TextMeshProUGUI text;
    public void Decide()
    {
        dm.SetDecision(element);
    }

}