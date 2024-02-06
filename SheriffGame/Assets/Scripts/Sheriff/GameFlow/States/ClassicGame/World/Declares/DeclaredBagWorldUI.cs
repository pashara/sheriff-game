using Sheriff.ECS.Components;
using UnityEngine;

namespace Sheriff.GameFlow.States.ClassicGame.World.Declares
{
    public class DeclaredBagWorldUI : MonoBehaviour
    {
        [SerializeField] private GameObject inputHandler;
        [SerializeField] private GameObject mainUI;
        [SerializeField] private GameObject sheriffUI;
        [SerializeField] private GameObject ownerUI;
        [SerializeField] private GameObject playerUI;
        
        
        public PlayerEntityId Owner { get; private set; }

        public void Link(PlayerEntity playerEntity)
        {
            Owner = playerEntity.playerId.Value;
        }


        public void OpenAsSheriff()
        {
            sheriffUI.SetActive(true);
            ownerUI.SetActive(false);
            playerUI.SetActive(false);
            ActivateController();
        }

        public void OpenAsOwner()
        {
            sheriffUI.SetActive(false);
            ownerUI.SetActive(true);
            playerUI.SetActive(false);
            ActivateController();
        }

        public void OpenAsPlayer()
        {
            sheriffUI.SetActive(false);
            ownerUI.SetActive(false);
            playerUI.SetActive(true);
            ActivateController();
        }


        public void Close()
        {
            DeactivateController();
        }
        

        private void ActivateController()
        {
            inputHandler.SetActive(true);
            mainUI.SetActive(true);
        }

        private void DeactivateController()
        {
            inputHandler.SetActive(false);
            mainUI.SetActive(false);
        }

    }
}