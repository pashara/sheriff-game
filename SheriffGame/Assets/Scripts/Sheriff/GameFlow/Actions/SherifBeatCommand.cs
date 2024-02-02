﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sheriff.ECS;
using Sheriff.ECS.Components;

namespace Sheriff.GameFlow
{
    public class SherifBeatCommand: GameCommand<SherifBeatCommand.Params, SherifBeatCommand>
    {
        [Serializable]
        public class Params : ActionParam
        {
            [JsonProperty("sheriff_id")]
            public PlayerEntityId sheriffId;
            [JsonProperty("target_player_id")]
            public PlayerEntityId targetPlayerId;
        }
        
        [Serializable]
        public class EmulateParam : EmulateActionParams
        {
            public PlayerEntityId sheriffId;
            public PlayerEntityId targetPlayerId;
        }
        
        
        private readonly EcsContextProvider _ecsContextProvider;
        
        [JsonProperty("result")]
        private EmulateParam _result;

        public SherifBeatCommand(EcsContextProvider ecsContextProvider)
        {
            _ecsContextProvider = ecsContextProvider;
        }
        
        public override SherifBeatCommand Calculate(Params param)
        {
            _result = new EmulateParam()
            {
                playerEntityId = param.playerEntityId,
                cardEntityId = param.cardEntityId
            };

            return this;
        }

        public override void Apply()
        {
            var playerEntity = _ecsContextProvider.Context.player.GetEntityWithPlayerId(_result.playerEntityId);
            if (playerEntity == null) return;
            
            if (!playerEntity.hasSelectedCards)
                playerEntity.AddSelectedCards(new List<CardEntityId>());

            if (playerEntity.selectedCards.Value.Contains(_result.cardEntityId)) return;
                
            playerEntity.selectedCards.Value.Add(_result.cardEntityId);
            playerEntity.ReplaceSelectedCards(playerEntity.selectedCards.Value);
        }
    }
}