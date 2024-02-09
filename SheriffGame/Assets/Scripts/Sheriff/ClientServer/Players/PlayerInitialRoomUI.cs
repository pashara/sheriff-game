using Sheriff.ClientServer.Game;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sheriff.ClientServer.Players
{
    public class PlayerInitialRoomUI : MonoBehaviour
    {
        [SerializeField] private PunGameManager punManager;
        
        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.pageUpKey.wasPressedThisFrame)
            {
                punManager.IncView();
            }
            else if (Keyboard.current != null && Keyboard.current.pageDownKey.wasPressedThisFrame)
            {
                punManager.DecView();
            }
        }
    }
}
