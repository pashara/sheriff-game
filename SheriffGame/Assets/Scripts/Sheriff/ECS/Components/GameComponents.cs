using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

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
    public class RoundComponent : IComponent
    {
        public int Value;
    }
    
    [Game]
    [Unique]
    public class GameIdComponent : IComponent
    {
        public GameSessionId Value;
    }


    [Game]
    public class PotentialPlayersSequence : IComponent
    {
        public List<PlayerEntityId> Value;
    }
}