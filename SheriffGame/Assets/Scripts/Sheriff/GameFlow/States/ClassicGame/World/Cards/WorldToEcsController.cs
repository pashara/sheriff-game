using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sheriff.ECS.Components;
using Sheriff.GameFlow.States.ClassicGame.View;
using Sheriff.GameResources;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.World.Cards
{
    public class WorldToEcsController : MonoBehaviour
    {
        [Inject] private DiContainer _container;
        [Inject] private CommandsApplyService _commandsApplyService;
        
        private PlayerEntity _playerEntity;

        public void Link(PlayerEntity playerEntity)
        {
            _playerEntity = playerEntity;
        }
        
        public void Declare(GameResourceType resourceType, int count)
        {
            var declarations = new List<ProductDeclaration>() { new(count, resourceType) };
            
            var action = _container.Instantiate<DeclareCommand>().Calculate(new DeclareCommand.Params()
            {
                playerEntityId = _playerEntity.playerId.Value,
                declarations = new ProductsDeclaration(declarations)
            });
            _commandsApplyService.Apply(action);
        }
        
        public void MarkSelected(List<CardView> cards)
        {
            // var declarations = new List<ProductDeclaration>() { new(count, resourceType) };
            
            var action = _container.Instantiate<PutCardsInBagCommand>().Calculate(new PutCardsInBagCommand.Params()
            {
                playerEntityId = _playerEntity.playerId.Value,
                cardEntityIds = cards.Where(x => x != null).Select(x => x.CardEntity.cardId.Value).ToList(),
            });
            _commandsApplyService.Apply(action);
        }

        public async UniTask ReleaseCard(CardView cardView)
        {
            var action = _container.Instantiate<ReleasePlayerCardCommand>().Calculate(new ReleasePlayerCardCommand.Params()
            {
                playerEntityId = _playerEntity.playerId.Value,
                cardEntityId = cardView.CardEntity.cardId.Value
            });
            _commandsApplyService.Apply(action);
        }

        public int CanReleaseCards()
        {
            var hasLimits = _playerEntity.hasMaxCardsPopPerStep && _playerEntity.maxCardsPopPerStep.Count >= 0;
            if (!hasLimits)
            {
                return 999;
            }

            var maxCount = 999;
            if (_playerEntity.hasMaxCardsPopPerStep)
            {
                maxCount = _playerEntity.maxCardsPopPerStep.Count;
            }

            if (_playerEntity.hasCardsPopPerStep)
            {
                return maxCount - _playerEntity.cardsPopPerStep.Count;
            }
            else
            {
                return maxCount;
            }
        }

        public async UniTask<IReadOnlyList<CardEntityId>> GetNewCard()
        {
            int cardsToRelease = CanReleaseCards();
            if (cardsToRelease <= 0)
                return new List<CardEntityId>();
            
            var action = _container.Instantiate<GetCardsFromDeckCommand>().Calculate(new GetCardsFromDeckCommand.Params()
            {
                cardsCount = 1,
                playerEntityId = _playerEntity.playerId.Value
            });
            _commandsApplyService.Apply(action);
            return action.CalculatedCards;
        }

        public void PutInBag(IReadOnlyList<CardView> cards)
        {
            var action = _container.Instantiate<PutCardsInBagCommand>().Calculate(new PutCardsInBagCommand.Params()
            {
                playerEntityId = _playerEntity.playerId.Value,
                cardEntityIds = cards.Select(x => x.CardEntity.cardId.Value).ToList()
            });
            _commandsApplyService.Apply(action);
        }
    }
}