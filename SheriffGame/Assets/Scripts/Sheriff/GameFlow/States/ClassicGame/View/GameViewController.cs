using System.Collections.Generic;
using Sheriff.GameFlow.States.ClassicGame.World;
using UnityEngine;

namespace Sheriff.GameFlow.States.ClassicGame.View
{
    public class GameViewController : MonoBehaviour
    {
        [SerializeField] private List<WorldPlayerPlaceControllers> worldPlayerPlaceControllers;

        public IReadOnlyList<WorldPlayerPlaceControllers> WorldPlayerPlaceControllers => worldPlayerPlaceControllers;
    }
}