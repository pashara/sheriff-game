using Cysharp.Threading.Tasks;
using Sheriff.DataBase;
using Sheriff.ECS.Components;
using Sheriff.GameFlow.CommandsApplier;
using Sheriff.GameResources;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow.PlayerUIControls
{
    public class UiToEcsController : MonoBehaviour
    {
        [Inject] private DiContainer _container;
        [Inject] private ICommandsApplyService _commandsApplyService;
        [Inject] private ICardConfigProvider _cardConfigProvider;
        
        public async UniTask<bool> TransferMoney(PlayerEntityId sourcePlayer, PlayerEntityId destinationPlayer, int amount)
        {
            var action = _container.Instantiate<TransferMoneyCommand>().Calculate(new TransferMoneyCommand.Params()
            {
                source = sourcePlayer,
                destination = destinationPlayer,
                count = amount
            });
            return await _commandsApplyService.Apply(action);
        }
        public async UniTask<bool> TransferResource(
            PlayerEntityId sourcePlayer, 
            PlayerEntityId destinationPlayer, 
            GameResourceType gameResourceType)
        {
            
            var category = _cardConfigProvider.Get(gameResourceType).Category;
            var action = _container.Instantiate<TransferResourceCommand>().Calculate(new TransferResourceCommand.Params()
            {
                source = sourcePlayer,
                destination = destinationPlayer,
                category = category,
                gameResource = gameResourceType
            });
            return await _commandsApplyService.Apply(action);
        }
    }
}