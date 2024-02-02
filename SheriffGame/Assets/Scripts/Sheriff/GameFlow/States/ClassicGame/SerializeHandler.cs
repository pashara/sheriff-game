using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame
{
    public class SerializeHandler : MonoBehaviour
    {
        [Inject] private CommandsApplyService _commandsApplyService;

        [Button]
        string Serialize()
        {
            return _commandsApplyService.Serialize();
        }
    }
}