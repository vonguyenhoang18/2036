using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance
    {
        get { return instance; }
    }

    private static InputManager instance = null;

    private void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private CharacterManager _characterManager => CharacterManager.Instance;

    // Update is called once per frame
    void Update()
    {
        CharacterMove();
        ChangeMaskState();
        UseHealing();
    }

    private void CharacterMove()
    {
        float x = 0f;
        float y = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            x = -1f;
            _characterManager.ChangeDirection(Direction.Left);
        }
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            x = 1f;
            _characterManager.ChangeDirection(Direction.Right);
        }
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) y = 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) y = -1f;

        _characterManager.ChangeSpeed(x, y);
    }

    private void ChangeMaskState()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            _characterManager.ChangeMaskState();
        }
    }

    private void UseHealing()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            _characterManager.UseHealing();
        }
    }
}
