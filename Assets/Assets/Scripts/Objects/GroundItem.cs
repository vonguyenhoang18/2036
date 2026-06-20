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
                GameObject go = UIManager.Instance.ShowPopup(Popup.Result);
                go.GetComponent<PopupResult>().ShowResult(true);
                break;
            case ObjectType.Medkit:
                CharacterManager.Instance.AddItemCount();
                Destroy(this.gameObject);
                break;
            case ObjectType.Obstacle:
                // Block(character);
                break;
            case ObjectType.Swamp:
                CharacterManager.Instance.SetDamagedState(true);
                break;
            case ObjectType.Smoke:
                CharacterManager.Instance.DamageOnce();
                Vector2 knockbackDir = ((Vector2)other.transform.position - (Vector2)transform.position).normalized;
                CharacterManager.Instance.Knockback(knockbackDir);
                break;
            default:
                Debug.LogWarning("Unknown item type encountered.");
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name != "Character") return;

        if (itemType == ObjectType.Swamp)
        {
            CharacterManager.Instance.SetDamagedState(false);
        }
    }
}
