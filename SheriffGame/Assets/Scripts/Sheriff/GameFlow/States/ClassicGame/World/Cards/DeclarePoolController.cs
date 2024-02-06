using System.Collections.Generic;
using System.Linq;
using Sheriff.ECS;
using Sheriff.GameFlow.States.ClassicGame.View;
using Sheriff.GameResources;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.World.Cards
{
    public class DeclarePoolController : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown declareDropdown;
        [SerializeField] private Button declareButton;
        [SerializeField] private GameObject uiRoot;
        [SerializeField] private WorldToEcsController worldToEcsController;
        [SerializeField] private SelectToDeclareWorldCardsInputController selectToDeclareWorldCardsInputController;
        
        [Inject] private EcsContextProvider _ecsContextProvider;
        private readonly List<CardView> _cardsToBag = new();
        private List<(TMP_Dropdown.OptionData, GameResourceType x)> _allowedElements;

        public int DeclaredCount => _cardsToBag.Count;

        private ReactiveProperty<bool> _isAllowedToDeclare = new();
        private ReactiveProperty<int> _selectedIndex = new();
        public IReadOnlyCollection<CardView> CardViews => _cardsToBag;
        private CompositeDisposable _initDisposable = new();
        private CompositeDisposable _disposable = new();
        private PlayerEntity _playerEntity;


        public void Link(PlayerEntity playerEntity)
        {
            _playerEntity = playerEntity;
        }


        private ReactiveProperty<bool> IsDeclareState = new();
        private ReactiveProperty<bool> IsDeclared = new();

        private void EnableController()
        {
            uiRoot.gameObject.SetActive(true);
            FillAllowedToDeclare();
            _disposable.Clear();

            _isAllowedToDeclare.Subscribe(x => declareButton.interactable = x).AddTo(_disposable);

            declareDropdown.onValueChanged.AsObservable().Subscribe(x =>
            {
                _selectedIndex.Value = x;
                _isAllowedToDeclare.Value = true;
            });
            
            declareButton.OnClickAsObservable().Subscribe(x =>
            {
                worldToEcsController.Declare(_allowedElements[_selectedIndex.Value].x, DeclaredCount);
                worldToEcsController.MarkSelected(_cardsToBag);
            }).AddTo(_disposable);
        }

        void DisableController()
        {
            uiRoot.gameObject.SetActive(false);
        }
        

        public void Initialize()
        {
            _playerEntity.OnAllowedActions().Subscribe(x =>
            {
                IsDeclareState.Value = x != null && 
                                       x.Value.AllowedActions.Contains(typeof(DeclareCommand));
            }).AddTo(_initDisposable);

            _playerEntity.OnDeclareResourcesByPlayer().Subscribe(x =>
            {
                IsDeclared.Value = (x != null);
            }).AddTo(_initDisposable);


            IsDeclareState.CombineLatest(IsDeclared, (isDeclare, isDeclared) => isDeclare && !isDeclared).Subscribe(x =>
            {
                selectToDeclareWorldCardsInputController.gameObject.SetActive(x);
            }).AddTo(_initDisposable);

            IsDeclareState.Subscribe(x =>
            {
                if (x)
                {
                    EnableController();
                }
                else
                {
                    DisableController();
                }
            }).AddTo(_initDisposable);

        }


        public void Dispose()
        {
            DisableController();
            _initDisposable.Clear();
            _disposable.Clear();
        }
        

        public void FillAllowedToDeclare()
        {
            _allowedElements = _ecsContextProvider
                .Context
                .game
                .gameIdEntity
                .allowedToDeclareGameResources
                .Value
                .Select(x => (new TMP_Dropdown.OptionData(x.ToString()), x)).ToList();


            declareDropdown.options = _allowedElements.Select(x => x.Item1).ToList();
        }
        
        public void PutCardInBag(CardView cardView)
        {
            if (!_cardsToBag.Contains(cardView))
            {
                _cardsToBag.Add(cardView);
            }
        }

        public void RemoveFromBag(CardView cardView)
        {
            if (_cardsToBag.Contains(cardView))
            {
                _cardsToBag.Remove(cardView);
            }
        }
        
        public void ClearState()
        {
            _cardsToBag.Clear();
        }
    }
}