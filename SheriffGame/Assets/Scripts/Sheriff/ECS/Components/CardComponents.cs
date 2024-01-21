using Entitas;
using Entitas.CodeGeneration.Attributes;
using MyNamespace.CodeGeneration.Attributes;
using Sheriff.GameResources;

namespace Sheriff.ECS.Components
{

    [Card]
    [EntityIndex]
    public class InDecComponent : IComponent
    {
    }
    
    [Card]
    [Game]
    [Input]
    [Player]
    public class IdComponent : IComponent
    {
        [PrimaryEntityIndex]
        public long ID;
    }
    
    [Card]
    public class ResourceTypeComponent : IComponent
    {
        public GameResourceType Value;
    }
    
    [Card]
    public class ResourceCategoryComponent : IComponent
    {
        public GameResourceCategory Value;
    }

    public struct PlayerEntityId
    {
        public readonly long EntityID;

        public PlayerEntityId(long id)
        {
            EntityID = id;
        }

        public override string ToString()
        {
            return $"PlayerId: {EntityID}";
        }

        public static bool operator ==(PlayerEntityId a, PlayerEntityId b)
        {
            return a.EntityID == b.EntityID;
        }

        public static bool operator !=(PlayerEntityId a, PlayerEntityId b)
        {
            return !(a == b);
        }
        
        public static implicit operator PlayerEntityId(long value)
        {
            return new PlayerEntityId(value);
        }
    }

    public struct CardEntityId
    {
        public readonly long EntityID;

        public CardEntityId(long id)
        {
            EntityID = id;
        }
        

        public static bool operator ==(CardEntityId a, CardEntityId b)
        {
            return a.EntityID == b.EntityID;
        }

        public static bool operator !=(CardEntityId a, CardEntityId b)
        {
            return !(a == b);
        }
        
        public static implicit operator CardEntityId(long value)
        {
            return new CardEntityId(value);
        }
    }

    [Card]
    public class CardOwnerComponent : IComponent
    {
        [EntityIndex]
        public PlayerEntityId Value;
    }

    [Card]
    public class CardIdComponent : IComponent
    {
        [PrimaryEntityIndex]
        public CardEntityId Value;
    }
}