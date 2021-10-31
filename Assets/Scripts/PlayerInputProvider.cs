using System;
using Enums;
using Interfaces;
using UnityEngine;

public class PlayerInputProvider : MonoBehaviour, IInputProvider
{
    private const string JumpButton = "Jump";
    public float GetAxisInput(Axis axis)
    {

        return Input.GetAxisRaw(axis.ToUnityAxis());
    }

    public bool GetAction(InputAction action)
    {
        switch (action)
        {
            case InputAction.Jump:
                return Input.GetButton(JumpButton);
            default:
                throw new Exception($"{action.ToString()} is not implemented.");
        }

    }

    public bool GetActionPressed(InputAction action)
    {
        switch (action)
        {
            case InputAction.Jump:
                return Input.GetButtonDown(JumpButton);
            default:
                throw new Exception($"{action.ToString()} is not implemented.");
        }
    }
}
