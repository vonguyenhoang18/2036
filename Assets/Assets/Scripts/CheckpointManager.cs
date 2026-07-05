using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance
    {
        get { return instance; }
    }

    private static CheckpointManager instance = null;

    public Checkpoint CurrentCheckpoint { get; private set; }

    private void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetCheckpoint(Checkpoint checkpoint)
    {
        CurrentCheckpoint = checkpoint;
    }

    public bool IsCheckpoint(Checkpoint checkpoint)
    {
        return CurrentCheckpoint == checkpoint;
    }

    public bool IsFromCheckpointToCheckpoint(Checkpoint start, Checkpoint end)
    {
        return CurrentCheckpoint >= start && CurrentCheckpoint <= end;
    }
}
