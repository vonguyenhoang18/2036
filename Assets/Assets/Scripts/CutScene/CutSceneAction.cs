using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CutSceneAction
{
    public enum ActionType { Move, Show }

    public float triggerTime;
    public ActionType type;
    public Vector2 destination;
    public int scale = 1;
    public float duration;
    public UnityEvent action;
}
