using Entitas;
using Sheriff.ECS;

namespace Sheriff.ECS
{
    public interface IIdContext
    {
        IEntity GetEntityWithId(long ID);
    }
}

public sealed partial class CardContext : IIdContext
{
    IEntity IIdContext.GetEntityWithId(long id) => this.GetEntityWithId(id);
}
public sealed partial class PlayerContext : IIdContext
{
    IEntity IIdContext.GetEntityWithId(long id) => this.GetEntityWithId(id);
}
public sealed partial class GameContext : IIdContext
{
    IEntity IIdContext.GetEntityWithId(long id) => this.GetEntityWithId(id);
}

public sealed partial class InputContext : IIdContext
{
    IEntity IIdContext.GetEntityWithId(long id) => this.GetEntityWithId(id);
}