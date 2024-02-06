// using System;
// using System.Collections.Generic;
// using Newtonsoft.Json;
// using Sheriff.ECS;
// using Sheriff.ECS.Components;
// using Zenject;
//
// namespace Sheriff.GameFlow
// {
//     
//     public class PutCardInBagCommand : GameCommand<PutCardInBagCommand.Params, PutCardInBagCommand>
//     {
//         [Serializable]
//         public class Params : ActionParam
//         {
//             [JsonProperty("player_id")]
//             public PlayerEntityId playerEntityId;
//             [JsonProperty("card_id")]
//             public CardEntityId cardEntityId;
//         }
//         
//         [Serializable]
//         public class PutCardInBagEmulateParam : EmulateActionParams
//         {
//             public PlayerEntityId playerEntityId;
//             public CardEntityId cardEntityId;
//         }
//         
//         
//         [Inject] private readonly EcsContextProvider _ecsContextProvider;
//         
//         [JsonProperty("result")]
//         private PutCardInBagEmulateParam _result;
//
//         public PutCardInBagCommand(EcsContextProvider ecsContextProvider)
//         {
//             _ecsContextProvider = ecsContextProvider;
//         }
//         
//         public override PutCardInBagCommand Calculate(Params param)
//         {
//             _result = new PutCardInBagEmulateParam()
//             {
//                 playerEntityId = param.playerEntityId,
//                 cardEntityId = param.cardEntityId
//             };
//
//             return this;
//         }
//
//         public override void Apply()
//         {
//             var playerEntity = _ecsContextProvider.Context.player.GetEntityWithPlayerId(_result.playerEntityId);
//             if (playerEntity == null) return;
//             
//             if (!playerEntity.hasSelectedCards)
//                 playerEntity.AddSelectedCards(new List<CardEntityId>());
//
//             if (playerEntity.selectedCards.Value.Contains(_result.cardEntityId)) return;
//                 
//             playerEntity.selectedCards.Value.Add(_result.cardEntityId);
//             playerEntity.ReplaceSelectedCards(playerEntity.selectedCards.Value);
//         }
//     }
// }