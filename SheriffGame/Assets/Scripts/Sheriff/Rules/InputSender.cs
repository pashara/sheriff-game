using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sheriff.Rules
{
    public class InputSender : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private bool hideInput;
        private InputActionMap _element;

        public bool HideInput => hideInput;

        private void OnEnable()
        {
            if (_element == null)
                _element = playerInput.currentActionMap;
            LocalInputManager.Instance.PushInputMap(_element, this);
        }

        private void OnDisable()
        {
            LocalInputManager.Instance.PopInputMap(_element, this);
        }
    }
}