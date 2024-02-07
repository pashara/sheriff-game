using System;
using Newtonsoft.Json;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Sheriff.GameFlow.States.ClassicGame;
using Zenject;

namespace Sheriff.GameFlow
{
    
    public class CheckDealersCommand : GameCommand<CheckDealersCommand.Params, CheckDealersCommand>
    {
        
        [Serializable]
        public class Params : ActionParam
        {
            public SherifCheckResult CheckResult;
        }
        
        [Inject] private readonly EcsContextProvider _ecsContextProvider;

        [Serializable]
        public class PopCardFromBagEmulateParam : EmulateActionParams
        {
            [JsonProperty("check_result")]
            public SherifCheckResult CheckResult;
        }
        
        [JsonProperty("result")]
        private PopCardFromBagEmulateParam _result;

        public override CheckDealersCommand Calculate(Params param)
        {
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

            if (_result.CheckResult is SkipCheckSherifResult skipCheckSherifResult)
            {
                
                var dealerEntity = _ecsContextProvider.Context.player
                    .GetEntityWithPlayerId(skipCheckSherifResult.DealerId);
                dealerEntity.isReadyForCheck = false;
                dealerEntity.ReplaceSheriffCheckResult(skipCheckSherifResult);
                return;
            }

            if (_result.CheckResult is SherifLooseCheckResult sherifLoseCheckResult)
            {
                var sheriffEntity = _ecsContextProvider.Context.player
                    .GetEntityWithPlayerId(sherifLoseCheckResult.FromPlayerId);
                
                var dealerEntity = _ecsContextProvider.Context.player
                    .GetEntityWithPlayerId(sherifLoseCheckResult.ToPlayerId);

                // sheriffEntity.ReplaceGoldCashCurrency(sheriffEntity.goldCashCurrency.Value - sherifLoseCheckResult.Coins);
                // dealerEntity.ReplaceGoldCashCurrency(dealerEntity.goldCashCurrency.Value + sherifLoseCheckResult.Coins);
                dealerEntity.isReadyForCheck = false;
                dealerEntity.ReplaceSheriffCheckResult(_result.CheckResult);
                return;
            }
            
            if (_result.CheckResult is DealerLooseCheckResult sherifWinCheckResult)
            {
                var sheriffEntity = _ecsContextProvider.Context.player
                    .GetEntityWithPlayerId(sherifWinCheckResult.ToPlayerId);
                
                var dealerEntity = _ecsContextProvider.Context.player
                    .GetEntityWithPlayerId(sherifWinCheckResult.FromPlayerId);

                // sheriffEntity.ReplaceGoldCashCurrency(sheriffEntity.goldCashCurrency.Value + sherifWinCheckResult.Coins);
                // dealerEntity.ReplaceGoldCashCurrency(dealerEntity.goldCashCurrency.Value - sherifWinCheckResult.Coins);
                dealerEntity.isReadyForCheck = false;
                dealerEntity.ReplaceSheriffCheckResult(_result.CheckResult);
                return;
            }
        }

    }
}