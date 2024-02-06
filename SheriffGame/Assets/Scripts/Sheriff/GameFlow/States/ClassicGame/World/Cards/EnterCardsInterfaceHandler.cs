using NaughtyCharacter;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.World.Cards
{
    public class EnterCardsInterfaceHandler : MonoBehaviour, IInteractableGame
    {
        [SerializeField] private WorldPlayerCardsController cardsController;
        [Inject] private EcsContextProvider _ecsContextProvider;
        
        public bool CanInteract(Character character)
        {
            return _ecsContextProvider.Context.game.IsShoppingState() && 
                   cardsController.Owner.EntityID == character.PlayerId;
        }

        public void Interact(Character character)
        {
            cardsController.Open();
            
        }
    }
}