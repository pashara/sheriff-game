using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame
{
    public class ClassicGameControllerWrapper : MonoBehaviour
    {
        [Inject] private CommandsApplyService _commandsApplyService;
        [Inject] private DiContainer _container;
        
        private ClassicGameController _classicGameController;
        private ClassicGameStateMachine _stateMachine;

        [TextArea(1, 20)]
        [SerializeField] private string initialCommands;

        private void Start()
        {
            _classicGameController = _container.Resolve<ClassicGameController>();
            _stateMachine = _container.Resolve<ClassicGameStateMachine>();
            _classicGameController.StartGame();
        }

        [Button]
        private void Emulate()
        {
            var elements = JsonConvert.DeserializeObject<List<CommandsApplyService.Data>>(initialCommands, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            foreach (var element in elements)
            {
                var e = element.Command;
                _container.Inject(e);
                _commandsApplyService.Apply(e);
            }
        }

        [Button]
        private void GoNext()
        {
            _classicGameController.OnReady(_stateMachine.ActualState.Value.GetType());
        }
    }
}