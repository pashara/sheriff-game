using System.Collections.Generic;
using System.Linq;
using Sheriff.DataBase;
using Sheriff.ECS;
using Sheriff.GameFlow.States.ClassicGame.View;
using Sheriff.GameResources;
using Sheriff.InputLock;
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
        [SerializeField] private GameObject sheriffReminderTEMP;
        [SerializeField] private Button declareButton;
        [SerializeField] private GameObject uiRoot;
        [SerializeField] private WorldToEcsController worldToEcsController;
        [SerializeField] private SelectToDeclareWorldCardsInputController selectToDeclareWorldCardsInputController;
        
        [Inject] private EcsContextProvider _ecsContextProvider;
        [Inject] private ICardConfigProvider _cardConfigProvider;
        private readonly List<CardView> _cardsToBag = new();
        private List<(TMP_Dropdown.OptionData, GameResourceType x)> _allowedElements;

        public int DeclaredCount => _cardsToBag.Count;

        private ReactiveProperty<bool> _isAllowedToDeclare = new();
        private ReactiveProperty<bool> _isAllowedToGetNewCards = new();
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
        private ReactiveProperty<bool> IsSheriff = new();
        private ReactiveProperty<bool> IsDeclared = new();

        private void EnableController()
        {
            uiRoot.gameObject.SetActive(true);
            FillAllowedToDeclare();
            _disposable.Clear();

            _isAllowedToDeclare.Subscribe(x => declareButton.interactable = x).AddTo(_disposable);
            _isAllowedToGetNewCards.Subscribe(x => declareButton.interactable = x).AddTo(_disposable);

            declareDropdown.onValueChanged.AsObservable().Subscribe(x =>
            {
                _selectedIndex.Value = x;
            });
            
            declareButton.OnClickAsObservable().Subscribe(async x =>
            {
                bool result = false;
                using (LoadingOverlay.Lock())
                {
                    result = await worldToEcsController.Declare(_allowedElements[_selectedIndex.Value].x, DeclaredCount);
                    if (!result)
                        return;
                }

                using (LoadingOverlay.Lock())
                {
                    result = await worldToEcsController.MarkSelected(_cardsToBag);
                    if (!result)
                        return;
                }

            }).AddTo(_disposable);
        }

        void DisableController()
        {
            uiRoot.gameObject.SetActive(false);
        }
        

        public void Initialize()
        {
            
            _isAllowedToDeclare.Value = false;
            
            _playerEntity.OnAllowedActions().Subscribe(x =>
            {
                IsDeclareState.Value = x != null && 
                                       x.Value.AllowedActions.Contains(typeof(DeclareCommand));
            }).AddTo(_initDisposable);

            IsSheriff.Value = _playerEntity.isSheriff;

            _playerEntity.OnDeclareResourcesByPlayer().Subscribe(x =>
            {
                IsDeclared.Value = (x != null);
            }).AddTo(_initDisposable);


            IsDeclareState
                .CombineLatest(IsDeclared, (isDeclareState, isDeclared) => isDeclareState && !isDeclared)
                .Subscribe(x =>
                {
                    selectToDeclareWorldCardsInputController.gameObject.SetActive(x);
                    _isAllowedToDeclare.Value = x;
                    _isAllowedToGetNewCards.Value = x;
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
            
            IsSheriff.Subscribe(x => sheriffReminderTEMP.SetActive(x)).AddTo(_disposable);
        }


        public void Dispose()
        {
            DisableController();
            sheriffReminderTEMP.SetActive(false);
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
                .Select(x =>
                {
                    var resourceData = _cardConfigProvider.Get(x);
                    var resourceName = resourceData?.Title ?? x.ToString();
                    var resourceImage = resourceData?.Icon ?? null;
                    return (new TMP_Dropdown.OptionData(resourceName, resourceImage), x);
                }).ToList();


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