using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Photon.Pun;
using Sheriff.ClientServer.Game;
using UniRx;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow.CommandsApplier
{
    public class NetworkCommandsApplyService : ICommandsApplyService, ICommandsSerializable
    {
        private readonly IPunSender _punManager;
        private readonly DiContainer _container;

        private long lastId;
        private LinkedList<CommandData> _commands = new();
        private readonly CancellationTokenSource _cts;

        public NetworkCommandsApplyService(
            IPunSender punManager,
            DiContainer container
            )
        {
            _punManager = punManager;
            _container = container;
            _cts = new CancellationTokenSource();
            CommandsApplyHandler(_cts.Token);
            CommandsListen(_cts.Token);
        }


        public async UniTask<bool> Apply(IGameCommand gameCommand)
        {
            var id = lastId++;
            var commandData = new CommandData()
            {
                Hash = Guid.NewGuid().ToString(),
                Command = gameCommand,
            };

            UniTask<CommandData> answerWaitHandler = default;
            if (PhotonNetwork.IsMasterClient)
            {
                answerWaitHandler = WaitAnswer(commandData.Hash, 100f);
                var json = Serialize(commandData);
                _punManager.SendCommandToMaster(json);
            }
            else
            {
                answerWaitHandler = WaitAnswer(commandData.Hash, 100f);
                var json = Serialize(commandData);
                _punManager.SendCommandToMaster(json);
            }
            
            var command = await answerWaitHandler;

            if (command != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        async UniTask<CommandData> WaitAnswer(string commandGuid, float timeout)
        {
            var commandTask = _punManager.ReceivedFromMasterCommands.ObserveAdd()
                .Select(x => Deserialize(x.Value))
                .Where(x => x is { Command: not null } && x.Hash.Equals(commandGuid))
                .First().ToUniTask();


            var result = await commandTask;

            return result;
            
            // var result = await UniTask.WhenAny(UniTask.Delay(TimeSpan.FromSeconds(timeout)), commandTask);
        }


        async void CommandsApplyHandler(CancellationToken ct)
        {
            do
            {
                var commandTask = await _punManager.ReceivedFromMasterCommands.ObserveAdd()
                    .Select(x => Deserialize(x.Value))
                    .Where(x => x is { Command: not null })
                    .First().ToUniTask(cancellationToken: ct).SuppressCancellationThrow();
                
                if (commandTask.IsCanceled)
                    return;

                _commands.AddLast(commandTask.Result);
                commandTask.Result.Command.Apply();
            } while (true);
        }

        async void CommandsListen(CancellationToken ct)
        {
            do
            {
                var commandTask = await _punManager.ReceivedFromSlaveCommands.ObserveAdd()
                    .Select(x => Deserialize(x.Value))
                    .Where(x => x is { Command: not null })
                    .First().ToUniTask(cancellationToken: ct).SuppressCancellationThrow();
                
                if (commandTask.IsCanceled)
                    return;

                var command = commandTask.Result.Command;
                command.Recalculate();
                _punManager.NotifyCommand(Serialize(commandTask.Result));
            } while (true);
        }


        private CommandData Deserialize(string json)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<CommandData>(json,
                    new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    });

                _container.Inject(obj.Command);

                return obj;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(_commands,
                new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
        }

        public string Serialize(CommandData command)
        {
            return JsonConvert.SerializeObject(command,
                new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
        }
    }
}