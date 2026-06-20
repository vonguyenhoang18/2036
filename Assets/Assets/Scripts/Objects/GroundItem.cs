using System.Collections;
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
                if (MapManager.Instance.CurrentLevel == 5)
                {
                    GameObject dialogue5 = UIManager.Instance.ShowPopup(Popup.Dialogue);
                    dialogue5.GetComponent<PopupDialogue>().SetDialogue(Dialogue.Level5End, () =>
                    {
                        GameObject go = UIManager.Instance.ShowPopup(Popup.Result);
                        go.GetComponent<PopupResult>().ShowResult(true);
                    });
                }
                else {
                    GameObject go = UIManager.Instance.ShowPopup(Popup.Result);
                        go.GetComponent<PopupResult>().ShowResult(true);
                }
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
            case ObjectType.NPCA:
                float dirX = other.transform.position.x > transform.position.x ? 1f : -1f;
                transform.localScale = new Vector3(dirX, transform.localScale.y, transform.localScale.z);
                Transform characterTransform = other.transform;
                GameObject dialogue4 = UIManager.Instance.ShowPopup(Popup.Dialogue);
                dialogue4.GetComponent<PopupDialogue>().SetDialogue(Dialogue.Level4NPCA, () =>
                {
                    StartCoroutine(MoveToCharacterAndDestroy(characterTransform));
                });
                break;
            default:
                Debug.LogWarning("Unknown item type encountered.");
                break;
        }
    }

    private IEnumerator MoveToCharacterAndDestroy(Transform target)
    {
        float speed = 5f;
        Vector2 targetPos = new Vector2(target.position.x, target.position.y + 1f);
        while (target != null && Vector2.Distance(transform.position, targetPos) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
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
