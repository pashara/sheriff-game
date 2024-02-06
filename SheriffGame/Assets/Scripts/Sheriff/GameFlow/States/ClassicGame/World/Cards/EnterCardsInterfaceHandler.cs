using NaughtyCharacter;
using UnityEngine;

namespace Sheriff.GameFlow.States.ClassicGame.World.Cards
{
    public class EnterCardsInterfaceHandler : MonoBehaviour, IInteractableGame
    {
        [SerializeField] private WorldPlayerCardsController cardsController;
        public bool CanInteract(Character character)
        {
            return true;
        }

        public void Interact(Character character)
        {
            cardsController.Open();
            
        }
    }
}