using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using NaughtyCharacter;
using Sheriff.GameFlow;

namespace Sheriff.ECS.Components
{
    [Player]
    [ECSSerialize]
    public class PlayerIdComponent : IComponent
    {
        [PrimaryEntityIndex]
        public PlayerEntityId Value;
    }

    [Player]
    [ECSSerialize]
    public class PlayerPositionIdComponent : IComponent
    {
        public int Id;
    }

    [Player]
    public class PlayerControllerComponent : IComponent
    {
        public PlayerController Value;
    }


    [Player]
    [ECSSerialize]
    public class SheriffComponent : IComponent { }

    [Player]
    [ECSSerialize]
    public class DealerComponent : IComponent { }

    [Player]
    public class ActualStateProviderComponent : IComponent
    {
        public IActualStateProvider Value;
    }
    
    [Game]
    public class ActualStateProviderWritableComponent : IComponent
    {
        public IActualStateProviderWritable Value;
    }
    
    [Player]
    public class AllowedActionsComponent : IComponent
    {
        public IAllowedActionsProvider Value;
    }
    
    [Player]
    [ECSSerialize]
    public class OnHandCardsComponent : IComponent
    {
        public List<CardEntityId> Value;
    }
    
    [Player]
    [ECSSerialize]
    public class SelectedCardsComponent : IComponent
    {
        public List<CardEntityId> Value;
    }
    
    [Player]
    [ECSSerialize]
    public class DropCardsComponent : IComponent
    {
        public List<CardEntityId> Value;
    }
    
    [Player]
    [ECSSerialize]
    public class DeclareResourcesByPlayerComponent : IComponent
    {
        public ProductsDeclaration Value;
    }


    [Player]
    [ECSSerialize]
    public class GoldCashCurrencyComponent : IComponent
    {
        public int Value;
    }


    [Player]
    [ECSSerialize]
    public class ReadyForCheckComponent : IComponent
    {
        
    }


    [Player]
    [ECSSerialize]
    public class CardsPopPerStepComponent : IComponent
    {
        public int Count;
    }

    [Player]
    [ECSSerialize]
    public class MaxCardsPopPerStepComponent : IComponent
    {
        public int Count;
    }
}