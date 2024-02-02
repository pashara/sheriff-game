using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Sheriff.GameFlow;

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

    [Player]
    public class ActualStateProviderComponent : IComponent
    {
        public UserActionsList Value;
    }
    
    [Player]
    public class AllowedActionsComponent : IComponent
    {
        public IAllowedActionsProvider Value;
    }
    
    [Player]
    public class SelectedCardsComponent : IComponent
    {
        public List<CardEntityId> Value;
    }
    
    [Player]
    public class DeclareResourcesByPlayerComponent : IComponent
    {
        public ProductsDeclaration Value;
    }


    [Player]
    public class GoldCashCurrencyComponent : IComponent
    {
        public int Value;
    }


    [Player]
    public class ReadyForCheckComponent : IComponent
    {
        
    }
}