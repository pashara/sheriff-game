using System;
using System.Collections.Generic;
using Sheriff.ECS.Components;
using Sheriff.GameFlow.States.ClassicGame;
using Sheriff.GameResources;

namespace Sheriff.GameFlow.GameStatistics
{
    [Serializable]
    public class RoundTransferInfo
    {
        public Dictionary<GameResourceType, int> Passed = new();
        public Dictionary<GameResourceType, int> Rejected = new();

        public RoundTransferInfo()
        {
            
        }

        public RoundTransferInfo(Dictionary<GameResourceType, int> passed, Dictionary<GameResourceType, int> rejected)
        {
            Passed = passed == null
                ? new Dictionary<GameResourceType, int>()
                : new Dictionary<GameResourceType, int>(passed);
            
            Rejected = rejected == null
                ? new Dictionary<GameResourceType, int>()
                : new Dictionary<GameResourceType, int>(rejected);
        }
    }
    
    [Serializable]
    public class GoldTransfer
    {
        public int amount;
        public PlayerEntityId from;
        public PlayerEntityId to;

        public GoldTransfer()
        {
            
        }

        public GoldTransfer(PlayerEntityId from, PlayerEntityId to, int amount)
        {
            this.from = from;
            this.to = to;
            this.amount = amount;
        }
    }
    
    
    [Serializable]
    public class PlayerStatistics
    {
        public int checkSkips;
        public int checkWins;
        public int checkLoses;

        public int sheriffSkips;
        public int sheriffWins;
        public int sheriffLoses;

        public int sheriffWinCoins;
        public int sheriffLoseCoins;

        public int goldInTransfer;
        public int goldOutTransfer;

        public List<RoundTransferInfo> resourceTransfersInfo = new();

        public List<GoldTransfer> autoGoldTransfers = new();
        public List<GoldTransfer> manualGoldTransfers = new();
        
        
        
        
        public void AddTransferredPerRound(
            Dictionary<GameResourceType, int> passed,
            Dictionary<GameResourceType, int> rejected
            )
        {
            resourceTransfersInfo.Add(new RoundTransferInfo(passed, rejected));
        }

        public void OnSheriffCheck(SherifCheckResult result)
        {
            if (result is SkipCheckSherifResult)
            {
                sheriffSkips++;
            } 
            else if (result is SherifLooseCheckResult sl)
            {
                sheriffLoses++;
                sheriffLoseCoins += sl.Coins;
            }
            else if (result is DealerLooseCheckResult dl)
            {
                sheriffWins++;
                sheriffWinCoins += dl.Coins;
            }
        }

        public void OnCheck(SherifCheckResult result)
        {
            if (result is SkipCheckSherifResult)
            {
                checkSkips++;
            } 
            else if (result is SherifLooseCheckResult sl)
            {
                checkWins++;
            }
            else if (result is DealerLooseCheckResult dl)
            {
                checkLoses++;
            }
        }

        public void OnAutoMoneyTransaction(PlayerEntityId from, PlayerEntityId to, int amount)
        {
            autoGoldTransfers.Add(new GoldTransfer(from, to, amount));
        }

        public void OnManualMoneyTransaction(PlayerEntityId from, PlayerEntityId to, int amount)
        {
            manualGoldTransfers.Add(new GoldTransfer(from, to, amount));
        }
    }
}