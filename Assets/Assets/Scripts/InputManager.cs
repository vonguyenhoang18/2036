using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private CharacterManager _characterManager => GameManager.Instance.CharacterManager;

    // Update is called once per frame
    void Update()
    {
        CharacterMove();
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
}
