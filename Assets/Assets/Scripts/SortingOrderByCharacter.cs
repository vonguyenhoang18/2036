using UnityEngine;

public class SortingOrderByCharacter : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;

    private void Update()
    {
        if (targetRenderer == null || CharacterManager.Instance == null)
            return;

        float characterY = CharacterManager.Instance.Character.position.y;
        targetRenderer.sortingOrder = transform.position.y < characterY ? 0 : -3;
    }
}