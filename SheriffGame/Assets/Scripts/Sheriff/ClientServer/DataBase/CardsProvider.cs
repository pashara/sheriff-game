using System.Collections.Generic;
using System.Linq;
using Sheriff.GameResources;
using Sheriff.GameStructures;
using Sirenix.OdinInspector;
using ThirdParty.ProjectEditorFinder;
using UnityEngine;

namespace Sheriff.DataBase
{
    public interface ICardConfigProvider
    {
        public CardInfo Get(GameResourceType resourceType);
    }
    [CreateAssetMenu]
    public class CardsProvider : ScriptableObject, ICardConfigProvider
    {
        [SerializeField] private List<CardInfoSO> cards;
        
        
        
        [Button]
        private void Bake()
        {
            cards = EditorFinder.GetInProject<CardInfoSO>();
            EditorFinder.MakeDirty(this);
        }

        public CardInfo Get(GameResourceType resourceType) =>
            cards.FirstOrDefault(x => x.CardInfo.ResourceType == resourceType)?.CardInfo;
    }
}