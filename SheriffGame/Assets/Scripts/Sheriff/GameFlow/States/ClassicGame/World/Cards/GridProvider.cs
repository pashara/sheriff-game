using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sheriff.GameFlow.States.ClassicGame.World.Cards
{
    public class GridProvider : MonoBehaviour
    {
        [Serializable]
        public class GridPositionInfo
        {
            public Transform target;
        }

        [SerializeField] List<GridPositionInfo> gridPositions;

        public GridPositionInfo GetAt(int index)
        {
            return gridPositions[index];
        }
    }
}