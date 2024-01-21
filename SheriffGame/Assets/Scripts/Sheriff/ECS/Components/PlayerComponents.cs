using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Sheriff.ECS.Components
{
    [Player]
    public class PlayerIdComponent : IComponent
    {
        [PrimaryEntityIndex]
        public PlayerEntityId Value;
    }


    [Player]
    public class SheriffComponent : IComponent { }

    [Player]
    public class DealerComponent : IComponent { }
}