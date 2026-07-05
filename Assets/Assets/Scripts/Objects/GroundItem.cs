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
                if (CheckpointManager.Instance.IsCheckpoint(Checkpoint.Level4_Start))
                {
                    GameObject exitNoti = UIManager.Instance.ShowPopup(Popup.Dialogue);
                    exitNoti.GetComponent<PopupDialogue>().SetDialogue(Checkpoint.Level4_Exit, () =>
                    {
                        Vector2 knockbackDir = ((Vector2)other.transform.position - (Vector2)transform.position).normalized;
                        CharacterManager.Instance.Knockback(knockbackDir);
                        CheckpointManager.Instance.SetCheckpoint(Checkpoint.Level4_NPCA);
                    });
                    return;
                }
                else if (CheckpointManager.Instance.IsCheckpoint(Checkpoint.Level5_Start))
                {
                    GameObject dialogue5 = UIManager.Instance.ShowPopup(Popup.Dialogue);
                    dialogue5.GetComponent<PopupDialogue>().SetDialogue(Checkpoint.Level5_End, () =>
                    {
                        GameObject go = UIManager.Instance.ShowPopup(Popup.Result);
                        go.GetComponent<PopupResult>().ShowResult(true);
                    });
                    return;
                }

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
            case ObjectType.NPCA:
                float dirX = other.transform.position.x > transform.position.x ? 1f : -1f;
                transform.localScale = new Vector3(dirX, transform.localScale.y, transform.localScale.z);

                if (CheckpointManager.Instance.IsFromCheckpointToCheckpoint(Checkpoint.Level4_Start, Checkpoint.Level4_NPCA))
                {
                    Transform characterTransform = other.transform;
                    GameObject dialogue4 = UIManager.Instance.ShowPopup(Popup.Dialogue);
                    dialogue4.GetComponent<PopupDialogue>().SetDialogue(Checkpoint.Level4_NPCA, () =>
                    {
                        CheckpointManager.Instance.SetCheckpoint(Checkpoint.Level4_Exit);
                        StartCoroutine(MoveToCharacterAndDestroy(characterTransform));
                    });
                }
                else if (CheckpointManager.Instance.IsFromCheckpointToCheckpoint(Checkpoint.SafeZone_Exit, Checkpoint.Level6))
                {
                    GameObject dialogueSZ = UIManager.Instance.ShowPopup(Popup.Dialogue);
                    dialogueSZ.GetComponent<PopupDialogue>().SetDialogue(Checkpoint.SafeZone_NPCA, () =>
                    {
                        CheckpointManager.Instance.SetCheckpoint(Checkpoint.Level6);
                    });
                }
                break;
            case ObjectType.ExitSafeZone:
                if (CheckpointManager.Instance.IsFromCheckpointToCheckpoint(Checkpoint.SafeZone_Exit, Checkpoint.SafeZone_NPCA))
                {
                    GameObject dialogueSZ = UIManager.Instance.ShowPopup(Popup.Dialogue);
                    dialogueSZ.GetComponent<PopupDialogue>().SetDialogue(Checkpoint.SafeZone_Exit, () =>
                    {
                        Vector2 knockbackDir = ((Vector2)other.transform.position - (Vector2)transform.position).normalized;
                        CharacterManager.Instance.Knockback(knockbackDir);
                    });
                    return;
                }
                UIManager.Instance.ShowPopup(Popup.ExitSafeZone);
                CharacterManager.Instance.SetPause(true);
                break;
            default:
                break;
        }
    }

    private IEnumerator MoveToCharacterAndDestroy(Transform target)
    {
        AudioManager.Instance.PlaySound(AudioType.s_thud);
        float speed = 5f;
        Vector2 targetPos = new Vector2(target.position.x, target.position.y);
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
