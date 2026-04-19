using UnityEngine;

public class GroundItem : MonoBehaviour
{
    [SerializeField] private ObjectType itemType = ObjectType.Unknown;

    private CharacterManager _characterManager => GameManager.Instance.CharacterManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        string objectName = other.gameObject.name;
        if (objectName != "Character") return;

        switch (itemType)
        {
            case ObjectType.Exit:
                _characterManager.TriggerWinLevel();
                break;
            case ObjectType.Medkit:
                _characterManager.AddItemCount();
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
