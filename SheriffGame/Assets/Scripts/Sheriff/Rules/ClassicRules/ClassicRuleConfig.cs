using System;
using System.Collections.Generic;
using Sheriff.GameResources;

namespace Sheriff.Rules.ClassicRules
{
    
    public class ClassicRuleConfig : BaseRuleConfig, ICardsPerStepConfig
    {
        public InitialCardsConfig intitalCardsConfig;
        public InitialBankConfig bankConfig;
        public ClassicCardsPerStepConfig cardsPerStepConfig;
        
        CardsPerStepConfig ICardsPerStepConfig.Config => cardsPerStepConfig;
    }


    
    [Serializable]
    public class ClassicCardsPerStepConfig : CardsPerStepConfig
    {
        public int cardsPerStep;
    }
    
    [Serializable]
    public class CardsConfig
    {
        public int cardsCount;
        public GameResourceType resourceType;
        public GameResourceCategory category;
    }

    [Serializable]
    public class InitialCardsConfig
    {
        public List<CardsConfig> cardsConfigs;
    }

    [Serializable]
    public class InitialBankConfig
    {
        public int perPlayer;
        public int totalBank;
    }
}