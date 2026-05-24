using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, PlayerControls.IGameplayActions
{
    public enum Actions
    {
        LOOK,
        MOVE,
        JUMP,
        SPRINT,
        CROUCH,
        STEER,
    }

    public enum Interactions
    {
        PRESS,
        RELEASE,
    }

    public struct InputData
    {
        public Actions action;
        public Interactions interaction;
    }

    private PlayerControls _inputActions;

    public Vector2 Look {get; private set;}
    public Vector2 Move {get; private set;} 
    public float Steer {get; private set;}

    public event Action<InputData> OnInput;

    private void Awake()
    {
        _inputActions = new();
        _inputActions.Gameplay.SetCallbacks(this);
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        Look = ctx.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        Move = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        SendInput(ctx, Actions.JUMP);
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        SendInput(ctx, Actions.SPRINT);
    }

    public void OnCrouch(InputAction.CallbackContext ctx)
    {
        SendInput(ctx, Actions.CROUCH);
    }

    public void OnSteer(InputAction.CallbackContext ctx)
    {
        Steer = ctx.ReadValue<float>(); 
        SendInput(ctx, Actions.STEER);
    }

    private void SendInput(InputAction.CallbackContext ctx, Actions action)
    {
        if(ctx.performed)
        {
            OnInput?.Invoke(new InputData
            {
                action = action,
                interaction = Interactions.PRESS,
            });
        } else if (ctx.canceled)
        {
            OnInput?.Invoke(new InputData
            {
                action = action,
                interaction = Interactions.RELEASE,
            });
        }
    }
}