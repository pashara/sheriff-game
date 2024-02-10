using Cysharp.Threading.Tasks;

namespace Sheriff.GameFlow.CommandsApplier
{
    public interface ICommandsApplyService
    {
        UniTask<bool> Apply(IGameCommand gameCommand);
    }
}