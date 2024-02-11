﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Sheriff.GameFlow.States.ClassicGame;
using Sheriff.GameResources;
using Zenject;

namespace Sheriff.GameFlow
{
    
    public class CheckDealersCommand : GameCommand<CheckDealersCommand.Params, CheckDealersCommand>
    {
        
        [Serializable]
        public class Params : ActionParam
        {
            public SherifCheckResult CheckResult;
            
            public Params()
            {
            }
            
            public Params(Params @params)
            {
                CheckResult = @params.CheckResult;
            }
        }
        
        [Inject] private readonly EcsContextProvider _ecsContextProvider;

        [Serializable]
        public class PopCardFromBagEmulateParam : EmulateActionParams
        {
            [JsonProperty("check_result")]
            public SherifCheckResult CheckResult;
        }
        
        [JsonIgnore] protected override Params AppliedParams => _params;
        
        [JsonProperty("params")]
        private Params _params = null;
        [JsonProperty("result")]
        private PopCardFromBagEmulateParam _result;

        public override CheckDealersCommand Calculate(Params param)
        {
            _params = new Params(param);
            _result = new PopCardFromBagEmulateParam()
            {
                CheckResult = param.CheckResult
            };
            return this;
        }

        public override void Apply()
        {
            if (_result?.CheckResult == null)
                return;
            
            
            Dictionary<GameResourceType, int> allowedProducts = new();
            Dictionary<GameResourceType, int> smugglingProducts = new();
            Dictionary<GameResourceType, int> rejectedCards = new();

            PlayerEntity sheriffEntity = null;
            PlayerEntity dealerEntity = null;

            void AddIf(Dictionary<GameResourceType, int> a, GameResourceType b, int c)
            {
                if (!a.TryGetValue(b, out var cc))
                {
                    cc = 0;
                }

                a[b] = cc + c;
            }
            

            if (_result.CheckResult is SkipCheckSherifResult skipCheckSherifResult)
            {
                sheriffEntity = _ecsContextProvider.Context.player
                    .GetEntityWithPlayerId(skipCheckSherifResult.SheriffId);
                
                dealerEntity = _ecsContextProvider.Context.player
                    .GetEntityWithPlayerId(skipCheckSherifResult.DealerId);
                dealerEntity.isReadyForCheck = false;
                dealerEntity.ReplaceSheriffCheckResult(skipCheckSherifResult);

                if (dealerEntity.hasSelectedCards)
                {
                    foreach (var cardId in dealerEntity.selectedCards.Value)
                    {
                        var card = _ecsContextProvider.Context.card.GetEntityWithCardId(cardId);
                        if (card.resourceCategory.Value == GameResourceCategory.Allowed)
                        {
                            AddIf(allowedProducts, card.resourceType.Value, 1);
                        }
                        else if (card.resourceCategory.Value == GameResourceCategory.Smuggling)
                        {
                            AddIf(smugglingProducts, card.resourceType.Value, 1);
                        }
                    }
                }
            }
            else if (_result.CheckResult is SherifLooseCheckResult sherifLoseCheckResult)
            {
                sheriffEntity = _ecsContextProvider.Context.player
                    .GetEntityWithPlayerId(sherifLoseCheckResult.FromPlayerId);
                
                dealerEntity = _ecsContextProvider.Context.player
                    .GetEntityWithPlayerId(sherifLoseCheckResult.ToPlayerId);

                sheriffEntity.ReplaceGoldCashCurrency(sheriffEntity.goldCashCurrency.Value - sherifLoseCheckResult.Coins);
                sheriffEntity.playerStatistics.Value.OnAutoMoneyTransaction(sheriffEntity.playerId.Value,
                    dealerEntity.playerId.Value, sherifLoseCheckResult.Coins);
                
                dealerEntity.ReplaceGoldCashCurrency(dealerEntity.goldCashCurrency.Value + sherifLoseCheckResult.Coins);
                dealerEntity.playerStatistics.Value.OnAutoMoneyTransaction(sheriffEntity.playerId.Value,
                    dealerEntity.playerId.Value, sherifLoseCheckResult.Coins);
                
                
                if (dealerEntity.hasSelectedCards)
                {
                    foreach (var cardId in dealerEntity.selectedCards.Value)
                    {
                        var card = _ecsContextProvider.Context.card.GetEntityWithCardId(cardId);
                        if (card.resourceCategory.Value == GameResourceCategory.Allowed)
                        {
                            AddIf(allowedProducts, card.resourceType.Value, 1);
                        }
                        else if (card.resourceCategory.Value == GameResourceCategory.Smuggling)
                        {
                            AddIf(smugglingProducts, card.resourceType.Value, 1);
                        }
                    }
                }
            }
            else if (_result.CheckResult is DealerLooseCheckResult sherifWinCheckResult)
            {
                sheriffEntity = _ecsContextProvider.Context.player
                    .GetEntityWithPlayerId(sherifWinCheckResult.ToPlayerId);
                
                dealerEntity = _ecsContextProvider.Context.player
                    .GetEntityWithPlayerId(sherifWinCheckResult.FromPlayerId);

                sheriffEntity.ReplaceGoldCashCurrency(sheriffEntity.goldCashCurrency.Value + sherifWinCheckResult.Coins);
                dealerEntity.playerStatistics.Value.OnAutoMoneyTransaction(dealerEntity.playerId.Value,
                    sheriffEntity.playerId.Value, sherifWinCheckResult.Coins);
                
                dealerEntity.ReplaceGoldCashCurrency(dealerEntity.goldCashCurrency.Value - sherifWinCheckResult.Coins);
                dealerEntity.playerStatistics.Value.OnAutoMoneyTransaction(dealerEntity.playerId.Value,
                    sheriffEntity.playerId.Value, sherifWinCheckResult.Coins);
                
                
                if (dealerEntity.hasSelectedCards)
                {
                    foreach (var cardId in dealerEntity.selectedCards.Value)
                    {
                        var card = _ecsContextProvider.Context.card.GetEntityWithCardId(cardId);
                        if (sherifWinCheckResult.BadCards.Contains(cardId))
                        {
                            AddIf(rejectedCards, card.resourceType.Value, 1);
                            continue;
                        }

                        if (card.resourceCategory.Value == GameResourceCategory.Allowed)
                        {
                            AddIf(allowedProducts, card.resourceType.Value, 1);
                        }
                        else if (card.resourceCategory.Value == GameResourceCategory.Smuggling)
                        {
                            AddIf(smugglingProducts, card.resourceType.Value, 1);
                        }
                    }
                }
            }

            dealerEntity.isReadyForCheck = false;
            dealerEntity.ReplaceSheriffCheckResult(_result.CheckResult);

            if (!dealerEntity.hasTransferredResources)
                dealerEntity.ReplaceTransferredResources(new TransferredObjects());

            var actualState = dealerEntity.transferredResources.Value;

            foreach (var allowedProduct in allowedProducts)
            {
                actualState.Inc(GameResourceCategory.Allowed, allowedProduct.Key, allowedProduct.Value);
            }

            foreach (var smugglingProduct in smugglingProducts)
            {
                actualState.Inc(GameResourceCategory.Smuggling, smugglingProduct.Key, smugglingProduct.Value);
            }

            dealerEntity.ReplaceTransferredResources(actualState);
            

            try
            {
                Dictionary<GameResourceType, int> passedElements = new();
                foreach (var product in allowedProducts)
                    passedElements[product.Key] = passedElements.TryGetValue(product.Key, out var v)
                        ? v + product.Value
                        : product.Value;
                
                foreach (var product in smugglingProducts)
                    passedElements[product.Key] = passedElements.TryGetValue(product.Key, out var v)
                        ? v + product.Value
                        : product.Value;
                
                
                dealerEntity.playerStatistics.Value.AddTransferredPerRound(passedElements, rejectedCards);
                
                dealerEntity.playerStatistics.Value.OnCheck(_result.CheckResult);
                sheriffEntity.playerStatistics.Value.OnSheriffCheck(_result.CheckResult);
                
                
                dealerEntity.ReplacePlayerStatistics(dealerEntity.playerStatistics.Value);
                sheriffEntity.ReplacePlayerStatistics(sheriffEntity.playerStatistics.Value);
            }
            catch (Exception e)
            {
                
            }
        }
    }
}