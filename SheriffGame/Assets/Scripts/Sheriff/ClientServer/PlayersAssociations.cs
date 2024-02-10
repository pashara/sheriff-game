using System.Collections.Generic;
using NaughtyCharacter;
using Photon.Pun;
using Photon.Realtime;

namespace Sheriff.ClientServer
{
    public class NetworkPlayerInfo
    {
        public Player punPlayer;
        public PhotonView photonView;
        public PlayerController playerController;
        public PlayerEntity playerEntity;
    }
    
    public class PlayersAssociations
    {
        private Dictionary<Player, NetworkPlayerInfo> _info = new();
        
        
        // Define the indexer to allow client code to use [] notation.
        public NetworkPlayerInfo this[Player i]
        {
            get => _info.GetValueOrDefault(i);
            set => _info[i] = value;
        }
    }
}