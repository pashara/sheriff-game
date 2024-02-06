using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Sheriff.GameResources;

namespace Sheriff.ECS.Components
{
    public struct GameSessionId
    {
        public int id;

        public GameSessionId(int i)
        {
            id = i;
        }
    }
    
    [Game]
    [ECSSerialize]
    public class RoundComponent : IComponent
    {
        public int Value;
    }
    
    [Game]
    [Unique]
    [ECSSerialize]
    public class GameIdComponent : IComponent
    {
        public GameSessionId Value;
    }


    [Game]
    [ECSSerialize]
    public class PotentialPlayersSequence : IComponent
    {
        public List<PlayerEntityId> Value;
    }

    [Game]
    [ECSSerialize]
    public class AllowedToDeclareGameResourcesComponent : IComponent
    {
        public List<GameResourceType> Value;
    }
}