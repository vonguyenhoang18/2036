using System.Collections;
using UnityEngine;

public class CutSceneUnit : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CutSceneAction[] actions;

    public CutSceneAction[] Actions => actions;

    public void Execute(CutSceneAction action)
    {
        if (action.type == CutSceneAction.ActionType.Move)
            StartCoroutine(MoveRoutine(action));
        else
            action.action?.Invoke();
    }

    private IEnumerator MoveRoutine(CutSceneAction action)
    {
        Vector3 s = transform.localScale;
        transform.localScale = new Vector3(Mathf.Abs(s.x) * action.scale, s.y, s.z);

        if (animator != null)
            animator.SetFloat("Speed", 1f);
        if (AudioManager.Instance != null && !AudioManager.Instance.IsLoopPlaying(AudioType.s_walking))
            AudioManager.Instance.PlayLoopSound(AudioType.s_walking);

        Vector2 start = transform.position;
        float elapsed = 0f;

        while (elapsed < action.duration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector2.Lerp(start, action.destination, elapsed / action.duration);
            yield return null;
        }

        transform.position = action.destination;

        if (animator != null)
            animator.SetFloat("Speed", 0f);
        if (AudioManager.Instance != null)
            AudioManager.Instance.StopLoopSound(AudioType.s_walking);
    }
}
