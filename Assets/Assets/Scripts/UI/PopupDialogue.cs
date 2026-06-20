using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PopupDialogue : MonoBehaviour
{
    [SerializeField] private Image charLeft;
    [SerializeField] private Image charLeftShadow;
    [SerializeField] private Image charRight;
    [SerializeField] private Image charRightShadow;
    [SerializeField] private TextMeshProUGUI titleTxt;
    [SerializeField] private TextMeshProUGUI messageTxt;
    [SerializeField] private DialogueConfig dialogueConfig;

    private List<DialogueEntry> _curDialogue = new();
    private int _curDialogueIndex = 0;
    private int _curDialogueCount = 0;
    private int _curConversationIndex = 0;
    private int _curConversationCount = 0;
    private int _curMessageIndex = 0;
    private int _curMessageCount = 0;

    private void OnEnable()
    {
        CharacterManager.Instance.SetPause(true);
    }

    private void OnDisable()
    {
        CharacterManager.Instance.SetPause(false);
    }
    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            OnContinueBtn();
        }
    }

    public void SetDialogue(Dialogue dialogue)
    {
        _curDialogue = dialogueConfig.GetDialogue(dialogue);
        if (_curDialogue == null)
        {
            UIManager.Instance.HidePopup();
            return;
        }
        _curDialogueCount = _curDialogue.Count;
        _curDialogueIndex = 0;
        ShowDialogue();
    }

    private void ShowDialogue()
    {
        var dialogueEntry = _curDialogue[_curDialogueIndex];
        _curConversationCount = dialogueEntry.messages.Length;
        _curConversationIndex = 0;

        charLeft.gameObject.SetActive(dialogueEntry.charLeft != null);
        charLeft.sprite = dialogueEntry.charLeft;
        charLeftShadow.sprite = dialogueEntry.charLeft;
        charRight.gameObject.SetActive(dialogueEntry.charRight != null);
        charRight.sprite = dialogueEntry.charRight;
        charRightShadow.sprite = dialogueEntry.charRight;

        ShowMessage();
    }

    private void ShowMessage()
    {
        var messageEntry = _curDialogue[_curDialogueIndex].messages[_curConversationIndex];
        _curMessageCount = messageEntry.messages.Length;
        _curMessageIndex = 0;

        ShowUI();
    }

    private void ShowUI()
    {
        var messageEntry = _curDialogue[_curDialogueIndex].messages[_curConversationIndex];
        charLeftShadow.gameObject.SetActive(!messageEntry.isLeft);
        charRightShadow.gameObject.SetActive(messageEntry.isLeft);
        titleTxt.text = messageEntry.title;
        messageTxt.text = messageEntry.messages[_curMessageIndex];
    }

    public void OnContinueBtn()
    {
        AudioManager.Instance.PlaySound(AudioType.s_click);
        _curMessageIndex++;
        if (_curMessageIndex == _curMessageCount)
        {
            _curConversationIndex++;
            if (_curConversationIndex == _curConversationCount)
            {
                _curDialogueIndex++;
                if (_curDialogueIndex == _curDialogueCount)
                {
                    UIManager.Instance.HidePopup();
                    return;
                }
                ShowDialogue();
            }
            else
            {
                ShowMessage();
            }
        }
        else
        {
            ShowUI();
        }
    }
}
