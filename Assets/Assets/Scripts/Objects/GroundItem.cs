using UnityEngine;

public class GroundItem : MonoBehaviour
{
    [SerializeField] private ObjectType itemType = ObjectType.Unknown;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CharacterManager characterManager = other.GetComponent<CharacterManager>();
        if (characterManager == null) return;

        switch (itemType)
        {
            case ObjectType.Exit:
                characterManager.TriggerWinLevel();
                break;
            case ObjectType.Medkit:
                characterManager.AddItemCount();
                Destroy(this.gameObject);
                break;
            case ObjectType.Obstacle:
                // Block(character);
                break;
            default:
                Debug.LogWarning("Unknown item type encountered.");
                break;
        }
    }
}
