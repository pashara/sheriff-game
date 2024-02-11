using System;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using NaughtyCharacter;
using Newtonsoft.Json;
using Sheriff.GameFlow;
using Sheriff.GameFlow.GameStatistics;
using Sheriff.GameFlow.Players;
using Sheriff.GameFlow.States.ClassicGame;
using Sheriff.GameFlow.States.ClassicGame.World;
using Sheriff.GameFlow.States.ClassicGame.World.Cards;
using Sheriff.GameResources;

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
    public class PlayerNetworkIdComponent : IComponent
    {
        [PrimaryEntityIndex]
        public string Value;
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

    [Player]
    [ECSSerialize]
    public class SheriffCheckResultComponent : IComponent
    {
        public SherifCheckResult Value;
    }

    [Player]
    [ECSSerialize]
    public class TransferredResourcesComponent : IComponent
    {
        public TransferredObjects Value;
    }

    [Player]
    [ECSSerialize]
    public class NicknameComponent : IComponent
    {
        public string Value;
    }

    [Player]
    public class WorldPlayerPlaceControllerComponent : IComponent
    {
        public WorldPlayerPlaceControllers Value;
    }


    [Player]
    [ECSSerialize]
    public class PlayerStatisticsComponent : IComponent
    {
        public PlayerStatistics Value;
    }

    [Serializable]
    public class TransferredObjects
    {
        [JsonProperty("allowed")] public Dictionary<GameResourceType, int> AllowedResources = new();
        [JsonProperty("not_allowed")] public Dictionary<GameResourceType, int> NotAllowedResources = new();


        public bool TryGetValue(GameResourceType gameResourceType, out int count)
        {
            if (AllowedResources != null && AllowedResources.TryGetValue(gameResourceType, out var c1))
            {
                count = c1;
                return true;
            }
            
            if (NotAllowedResources != null && NotAllowedResources.TryGetValue(gameResourceType, out var c2))
            {
                count = c2;
                return true;
            }

            count = 0;
            return false;
        }

        public void Inc(GameResourceCategory category, GameResourceType gameResourceType, int count = 1)
        {
            Dictionary<GameResourceType, int> source = null;
            if (category == GameResourceCategory.Allowed)
            {
                AllowedResources ??= new();
                source = AllowedResources;
            }
            else if (category == GameResourceCategory.Smuggling)
            {
                NotAllowedResources ??= new();
                source = NotAllowedResources;
            }

            if (source != null)
            {
                if (!source.TryGetValue(gameResourceType, out var cc))
                {
                    source[gameResourceType] = 0;
                }

                source[gameResourceType] += count;
            }
        }

        public void Dec(GameResourceCategory category, GameResourceType gameResourceType, int count = 1)
        {
            Dictionary<GameResourceType, int> source = null;
            if (category == GameResourceCategory.Allowed)
            {
                AllowedResources ??= new();
                source = AllowedResources;
            }
            else if (category == GameResourceCategory.Smuggling)
            {
                NotAllowedResources ??= new();
                source = NotAllowedResources;
            }

            if (source != null)
            {
                if (!source.TryGetValue(gameResourceType, out var cc))
                {
                    source[gameResourceType] = 0;
                }

                source[gameResourceType] -= count;
            }
        }
    }
}