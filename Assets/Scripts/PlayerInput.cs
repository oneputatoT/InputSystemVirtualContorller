using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "PlayerInputData")]
public class PlayerInput : ScriptableObject, InputActions.IGamePlayerActions
{
    InputActions inputActions;
    public event UnityAction<Vector2> onMove = delegate{};
    public event UnityAction onStopMove = delegate {};
    public event UnityAction<Vector2> onRotate = delegate { };
    public event UnityAction onStopRotate = delegate { };
    public event UnityAction onAttack = delegate { };

    void OnEnable()
    {
        inputActions = new InputActions();
        inputActions.GamePlayer.SetCallbacks(this);
    }

    void OnDisable()
    {
        DisableAllInput();
    }

    public void DisableAllInput() => inputActions.Disable();

    public void EnableGameplayerInput() => inputActions.GamePlayer.Enable();
    


    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onMove.Invoke(context.ReadValue<Vector2>());
        }
        if (context.canceled)
        {
            onStopMove.Invoke();
        }
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onRotate.Invoke(context.ReadValue<Vector2>());
        }
        if (context.canceled)
        {
            onStopRotate.Invoke();
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onAttack.Invoke();
        }
    }
}
