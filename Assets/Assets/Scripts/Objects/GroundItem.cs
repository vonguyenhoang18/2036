using UnityEngine;

public class GroundItem : MonoBehaviour
{
    [SerializeField] private ObjectType itemType = ObjectType.Unknown;

    private void OnTriggerEnter2D(Collider2D other)
    {
        string objectName = other.gameObject.name;
        if (objectName != "Character") return;

        switch (itemType)
        {
            case ObjectType.Exit:
                CharacterManager.Instance.TriggerWinLevel();
                break;
            case ObjectType.Medkit:
                CharacterManager.Instance.AddItemCount();
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
