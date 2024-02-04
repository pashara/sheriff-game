using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sheriff.Rules
{
    public class InputSender : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        private InputActionMap _element;

        private void OnEnable()
        {
            if (_element == null)
                _element = playerInput.currentActionMap;
            LocalInputManager.Instance.PushInputMap(_element);
        }

        private void OnDisable()
        {
            LocalInputManager.Instance.PopInputMap(_element);
        }
    }
}