using Sheriff.GameFlow.CommandsApplier;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame
{
    public class SerializeHandler : MonoBehaviour
    {
        [Inject] private ICommandsSerializable _commandsApplyService;

        [Button]
        string Serialize()
        {
            return _commandsApplyService.Serialize();
        }
    }
}