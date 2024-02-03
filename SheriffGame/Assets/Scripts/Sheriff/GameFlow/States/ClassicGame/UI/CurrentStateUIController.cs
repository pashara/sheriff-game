using Entitas;
using Sheriff.ECS;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.UI
{
    public class CurrentStateUIController : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        
        [Inject]
        private void Construct(EcsContextProvider ecsContextProvider)
        {
            ListenCurrentGame(ecsContextProvider);
        }

        void ListenCurrentGame(EcsContextProvider ecsContextProvider)
        {
            if (ecsContextProvider.Context.game.gameIdEntity == null)
            {
                ecsContextProvider.Context.game.OnEntityCreated += GameOnOnEntityCreated;
            }
            else
            {
                Subscribe(ecsContextProvider.Context.game.gameIdEntity);
            }
        }

        void Subscribe(GameEntity gameEntity)
        {
            gameEntity.OnActualStateProviderWritable().Subscribe(x =>
            {
                x?.Value?.ActualState?.Subscribe(y =>
                {
                    label.SetText(y?.GetType()?.Name ?? "null");
                }).AddTo(this);
            }).AddTo(this);
        }
        
        private void GameOnOnEntityCreated(IContext context, IEntity entity)
        {
            if (entity is GameEntity gameEntity)
            {
                Subscribe(gameEntity);
            }
        }
    }
}