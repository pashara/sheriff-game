using System.Collections.Generic;
using Sheriff.GameStructures;
using Sirenix.OdinInspector;
using ThirdParty.ProjectEditorFinder;
using UnityEngine;

namespace Sheriff.DataBase
{
    [CreateAssetMenu]
    public class CardsProvider : ScriptableObject
    {
        [SerializeField] private List<CardInfoSO> cards;
        
        
        [Button]
        private void Bake()
        {
            cards = EditorFinder.GetInProject<CardInfoSO>();
            EditorFinder.MakeDirty(this);
        }
    }
}