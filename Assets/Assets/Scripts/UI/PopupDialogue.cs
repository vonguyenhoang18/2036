using System;
using System.Collections;
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
    [SerializeField] private float startDelay = 0.5f;

    private Coroutine _typeCoroutine;
    private bool _isTyping;

    private List<DialogueEntry> _curDialogue = new();
    private int _curDialogueIndex = 0;
    private int _curDialogueCount = 0;
    private int _curConversationIndex = 0;
    private int _curConversationCount = 0;
    private int _curMessageIndex = 0;
    private int _curMessageCount = 0;
    private Action _onEndDialogue;

    private void OnEnable()
    {
        if (CharacterManager.Instance != null)
            CharacterManager.Instance.SetPause(true);
    }

    private void OnDisable()
    {
        if (CharacterManager.Instance != null)
            CharacterManager.Instance.SetPause(false);
    }
    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            OnContinueBtn();
        }
    }

    public void SetDialogue(Checkpoint dialogue, Action onEndDialogue = null)
    {
        _onEndDialogue = null;
        if (onEndDialogue != null)
        {
            _onEndDialogue = onEndDialogue;
        }

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

        float scale = GameConstant.DIALOGUE_SCALE;
        charLeft.transform.localScale = messageEntry.isLeft ? new Vector3(scale, scale, 1f) : new Vector3(1f, 1f, 1f);
        charRight.transform.localScale = messageEntry.isLeft ? new Vector3(-1f, 1f, 1f) : new Vector3(-scale, scale, 1f);

        TypeMessage(messageEntry.messages[_curMessageIndex]);
    }

    private void TypeMessage(string message)
    {
        if (_typeCoroutine != null)
            StopCoroutine(_typeCoroutine);
        _typeCoroutine = StartCoroutine(TypeMessageRoutine(message));
    }

    private IEnumerator TypeMessageRoutine(string message)
    {
        _isTyping = true;
        messageTxt.text = message;
        messageTxt.ForceMeshUpdate();

        int totalChars = messageTxt.textInfo.characterCount;
        messageTxt.maxVisibleCharacters = 0;

        if (startDelay > 0f)
            yield return new WaitForSeconds(startDelay);

        for (int visible = 1; visible <= totalChars; visible++)
        {
            messageTxt.maxVisibleCharacters = visible;
            yield return null;
        }

        _isTyping = false;
        _typeCoroutine = null;
    }

    private void CompleteTyping()
    {
        if (_typeCoroutine != null)
        {
            StopCoroutine(_typeCoroutine);
            _typeCoroutine = null;
        }
        messageTxt.maxVisibleCharacters = int.MaxValue;
        _isTyping = false;
    }

    public void OnContinueBtn()
    {
        AudioManager.Instance.PlaySound(AudioType.s_click);

        // First press while typing reveals the whole line instead of advancing.
        if (_isTyping)
        {
            CompleteTyping();
            return;
        }

        _curMessageIndex++;
        if (_curMessageIndex == _curMessageCount)
        {
            _curConversationIndex++;
            if (_curConversationIndex == _curConversationCount)
            {
                _curDialogueIndex++;
                if (_curDialogueIndex == _curDialogueCount)
                {
                    if (UIManager.Instance != null)
                        UIManager.Instance.HidePopup();
                    _onEndDialogue?.Invoke();
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
