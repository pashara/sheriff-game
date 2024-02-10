using Sheriff.ClientServer.Game;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Sheriff.ClientServer.Players
{
    public class PlayerInitialRoomUI : MonoBehaviour
    {
        [Inject] private IPunSender _punSender;
        
        private void Update()
        {
            if (_punSender == null)
                return;
            
            if (Keyboard.current != null && Keyboard.current.pageUpKey.wasPressedThisFrame)
            {
                _punSender.IncView();
            }
            else if (Keyboard.current != null && Keyboard.current.pageDownKey.wasPressedThisFrame)
            {
                _punSender.DecView();
            }
        }
    }
}
