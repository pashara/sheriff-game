using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sheriff.GameFlow.States.ClassicGame
{
    public class CommandsApplyService
    {
        [Serializable]
        public class Data
        {
            public long Id;
            public IGameCommand Command;
        }

        private long lastId;
        private LinkedList<Data> _commands = new();
        public CommandsApplyService()
        {
            
        }


        public void Apply(IGameCommand gameCommand)
        {
            var id = lastId++;
            _commands.AddLast(new Data()
            {
                Id = id,
                Command = gameCommand
            });
            
            gameCommand.Apply();
        }


        public string Serialize()
        {
            return JsonConvert.SerializeObject(_commands,
                new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
        }
    }
}