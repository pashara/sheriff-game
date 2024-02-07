using NaughtyCharacter;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.World.Declares
{
    public class EnterDeclaredInterfaceHandler : MonoBehaviour, IInteractableGame
    {
        [Inject] private EcsContextProvider _ecsContextProvider;
        [SerializeField] private DeclaredBagWorldUI declaredBagWorldUI;
        
        
        public bool CanInteract(Character character)
        {
            if (!_ecsContextProvider.Context.game.IsSheriffCheckState())
                return false;
            
            var player = _ecsContextProvider.Context.player.GetEntityWithPlayerId(character.PlayerId);
            
            if (player.isSheriff && declaredBagWorldUI.Owner == player.playerId.Value)
                return false;

            return true;
        }

        public void Interact(Character character)
        {
            var player = _ecsContextProvider.Context.player.GetEntityWithPlayerId(character.PlayerId);
            if (player.isSheriff)
            {
                declaredBagWorldUI.OpenAsSheriff(player);
            }
            else if (declaredBagWorldUI.Owner == player.playerId.Value)
            {
                declaredBagWorldUI.OpenAsOwner(player);
            }
            else
            {
                declaredBagWorldUI.OpenAsPlayer(player);
            }
        }
    }
}