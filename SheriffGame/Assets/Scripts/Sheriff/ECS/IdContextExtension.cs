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