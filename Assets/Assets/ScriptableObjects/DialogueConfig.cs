using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueConfig", menuName = "Scriptable Objects/DialogueConfig")]
public class DialogueConfig : ScriptableObject
{
    public List<DialogueSequence> allDialogues = new();

    private Dictionary<Dialogue, List<DialogueEntry>> _lookup;

    private void BuildLookup()
    {
        _lookup = new Dictionary<Dialogue, List<DialogueEntry>>();
        foreach (var sequence in allDialogues)
            _lookup[sequence.key] = sequence.entries;
    }

    public List<DialogueEntry> GetDialogue(Dialogue key)
    {
        if (_lookup == null) BuildLookup();
        return _lookup.TryGetValue(key, out var entries) ? entries : null;
    }
}

[System.Serializable]
public class DialogueSequence
{
    public Dialogue key;
    public List<DialogueEntry> entries = new();
}

[System.Serializable]
public class DialogueEntry
{
    public Sprite charLeft;
    public Sprite charRight;
    public MessageEntry[] messages;
}

[System.Serializable]
public class MessageEntry
{
    public bool isLeft;
    public string title;
    public string[] messages;
}
