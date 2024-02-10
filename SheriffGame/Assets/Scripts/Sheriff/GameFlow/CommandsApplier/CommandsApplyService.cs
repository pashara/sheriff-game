using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

namespace Sheriff.GameFlow.CommandsApplier
{
    [Serializable]
    public class CommandData
    {
        public string Hash;
        public IGameCommand Command;
    }
    
    public class CommandsApplyService : ICommandsApplyService, ICommandsSerializable
    {

        private long lastId;
        private LinkedList<CommandData> _commands = new();
        public CommandsApplyService()
        {
            
        }


        public async UniTask<bool> Apply(IGameCommand gameCommand)
        {
            var id = lastId++;
            _commands.AddLast(new CommandData()
            {
                Command = gameCommand
            });
            
            gameCommand.Apply();
            return true;
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