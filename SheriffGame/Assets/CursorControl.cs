using UnityEngine;
using UnityEngine.InputSystem;

public class CursorControl : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true; // Установите true или false в зависимости от вашего желания начальной видимости курсора
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.ctrlKey.isPressed)
        {
            ToggleCursorVisibility();
        }
    }

    private void ToggleCursorVisibility()
    {
        Cursor.visible = !Cursor.visible;
        Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}