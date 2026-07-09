using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneTime : MonoBehaviour
{
    [SerializeField] private CutSceneUnit[] units;
    [SerializeField] private Transform popupContent;
    [SerializeField] private GameObject[] popups;

    // CutScene1
    [SerializeField] private Animation clipCutScene1;

    public bool isPaused;

    private float timer;
    private List<(CutSceneUnit unit, CutSceneAction action)> sortedActions;
    private int nextIndex;

    private void Start()
    {
        timer = 0f;
        sortedActions = new List<(CutSceneUnit, CutSceneAction)>();

        foreach (CutSceneUnit unit in units)
            if (unit.Actions != null)
                foreach (CutSceneAction action in unit.Actions)
                    sortedActions.Add((unit, action));

        sortedActions.Sort((a, b) => a.action.triggerTime.CompareTo(b.action.triggerTime));
        nextIndex = 0;
    }

    private void Update()
    {
        if (isPaused) return;

        timer += Time.deltaTime;

        if (nextIndex < sortedActions.Count && timer >= sortedActions[nextIndex].action.triggerTime)
        {
            var (unit, action) = sortedActions[nextIndex];
            nextIndex++;

            if (action.type == CutSceneAction.ActionType.Show)
                Pause();

            unit.Execute(action);
        }
    }

    public void Pause() => isPaused = true;

    public void Resume() => isPaused = false;

    private GameObject ShowPopup(Popup popup)
    {
        ClearPopup();
        GameObject go = Instantiate(popups[(int)popup], popupContent);
        return go;
    }

    private void HidePopup()
    {
        ClearPopup();
    }

    private void ClearPopup()
    {
        foreach (Transform child in popupContent)
        {
            Destroy(child.gameObject);
        }
    }

    // CutScene1
    public void RunCutScene1_1()
    {
        GameObject go = ShowPopup(Popup.Dialogue);
        go.GetComponent<PopupDialogue>().SetDialogue(Checkpoint.CutScene1_1, () =>
        {
            HidePopup();
            Resume();
        });
    }

    public void RunCutScene1_ShowHeart()
    {
        clipCutScene1.Play();
        StartCoroutine(ShowHeartRoutine());
    }

    private IEnumerator ShowHeartRoutine()
    {
        yield return new WaitForSeconds(1f);
        AudioManager.Instance.PlaySound(AudioType.m_happy);
        Resume();
    }

    public void RunCutScene1_2()
    {
        GameObject go = ShowPopup(Popup.Dialogue);
        go.GetComponent<PopupDialogue>().SetDialogue(Checkpoint.CutScene1_2, () =>
        {
            HidePopup();
            Resume();
            void OnGameLoaded(Scene scene, LoadSceneMode mode)
            {
                SceneManager.sceneLoaded -= OnGameLoaded;
                GameObject loading = UIManager.Instance.ShowPopup(Popup.Loading);
                loading.GetComponent<LoadingPanel>().EndLoading(1f, () => {
                    UIManager.Instance.HidePopup();
                    MapManager.Instance.InitSafeZoneMap();
                    CheckpointManager.Instance.SetCheckpoint(Checkpoint.SafeZone_Exit);
                });
            }

            SceneManager.sceneLoaded += OnGameLoaded;
            SceneManager.LoadScene("Game");
        });
    }
}
