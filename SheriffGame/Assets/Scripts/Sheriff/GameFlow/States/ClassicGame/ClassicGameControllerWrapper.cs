using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame
{
    public class ClassicGameControllerWrapper : MonoBehaviour
    {
        [Inject] private DiContainer _container;
        private ClassicGameController _classicGameController;
        private ClassicGameStateMachine _stateMachine;

        private void Start()
        {
            _classicGameController = _container.Resolve<ClassicGameController>();
            _stateMachine = _container.Resolve<ClassicGameStateMachine>();
            _classicGameController.StartGame();
        }

        [Button]
        void GoNext()
        {
            _classicGameController.OnReady(_stateMachine.ActualState.Value.GetType());
        }
    }
}