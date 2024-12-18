using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Game/Character")]
public class Character : ScriptableObject
{
    public string charName;
    public Sprite charImage;

    public bool isInfected;

    public List<string> nightConversationsProgression = new List<string>();

    [SerializeField]
    private List<InkKeyValue> inkStoriesList = new List<InkKeyValue>();

    private Dictionary<string, TextAsset> _dictInkStories;

    public Dictionary<string, TextAsset> dictInkStories
    {
        get
        {
            if (_dictInkStories == null)
            {
                _dictInkStories = new Dictionary<string, TextAsset>();
                foreach (var entry in inkStoriesList)
                {
                    _dictInkStories[entry.key] = entry.value;
                }
            }
            return _dictInkStories;
        }
    }
}


[System.Serializable]
public struct InkKeyValue
{
    public string key;
    public TextAsset value;
}
