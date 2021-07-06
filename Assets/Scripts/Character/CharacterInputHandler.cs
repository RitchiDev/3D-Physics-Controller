using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInputHandler : MonoBehaviour
{
    private Vector3 m_MovementDirection;
    public Vector3 MovementDirection => m_MovementDirection;

    private Vector3 m_LookDirection;
    public Vector3 LookDirection => m_LookDirection;

    public void MovementContext(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            m_MovementDirection = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
        }

        if (context.canceled)
        {
            m_MovementDirection = Vector3.zero;
        }
    }

    public void LookContext(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            m_LookDirection = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
        }

        if (context.canceled)
        {
            m_LookDirection = Vector3.zero;
        }
    }
}
