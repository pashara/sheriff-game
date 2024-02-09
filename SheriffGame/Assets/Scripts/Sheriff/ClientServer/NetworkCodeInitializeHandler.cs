// using UnityEngine;
// using Zenject;
//
// namespace Sheriff.ClientServer
// {
//     public class NetworkCodeInitializeHandler : IInitializable
//     {
//         public void Initialize()
//         {
//             
//         }
//         
//         public GameLogic(string appId, string gameVersion)
//         {
//             // this.MasterServerAddress = "your server"; // no need to set any address when using the Photon Cloud.
//             this.AppId = appId;
//             this.AppVersion = gameVersion;
//
//             this.LocalPlayer.NickName = "usr" + SupportClass.ThreadSafeRandom.Next() % 99;
//
//             this.StateChanged += this.OnStateChanged;
//             this.UseInterestGroups = true;
//             this.JoinRandomGame = true;
//
//             this.DispatchInterval = new TimeKeeper(10);
//             this.SendInterval = new TimeKeeper(100);
//             this.MoveInterval = new TimeKeeper(500);
//             this.UpdateOthersInterval = new TimeKeeper(this.MoveInterval.Interval);
//         }
//     }
// }
