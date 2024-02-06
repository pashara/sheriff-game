using System;
using System.Linq;
using Sheriff.GameFlow.States.ClassicGame.View;
using UnityEngine;

namespace Sheriff.GameFlow.States.ClassicGame.World.Cards
{
    public class SelectToDeclareWorldCardsInputController : MonoBehaviour
    {
        private Camera _mainCamera;
        [SerializeField] private WorldPlayerCardsController worldPlayerCardsController;
        
        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            bool isLeftClick = Input.GetMouseButtonDown(0);
            bool isRightClick = Input.GetMouseButtonDown(1);
            if (isLeftClick || isRightClick)
            {
                var hitInfo = RaycastFromMouse<ICardInteractable>();
            
                if (hitInfo.Item1 != null)
                {
                    if (hitInfo.Item1 is InteractableCardProvider cardInteractable)
                    {
                        if (isLeftClick)
                        {
                            if (cardInteractable.CardView.CardEntity.isCardOnHand)
                            {
                                if (!worldPlayerCardsController.CardsInBag.Contains(cardInteractable.CardView))
                                {
                                    worldPlayerCardsController.PutInBag(cardInteractable.CardView);
                                }
                                else
                                {
                                    worldPlayerCardsController.PopFromBag(cardInteractable.CardView);
                                }
                            }

                            return;
                        }
                        
                        if (isRightClick)
                        {
                            if (!cardInteractable.CardView.CardEntity.isCardRelease)
                            {
                                worldPlayerCardsController.ReleaseCard(cardInteractable.CardView);
                            }

                            return;
                        }
                    }

                    return;
                }
            }
        }


        private (T, RaycastHit) RaycastFromMouse<T>(Func<T, bool> check = null)
        {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            int uiLayerMask = LayerMask.GetMask("UI"); // Укажите слой для UI
            var hits = Physics.RaycastAll(ray, 5f, uiLayerMask);

            return hits.Select(x => (x.collider.GetComponent<T>(), x)).Where(x => x.Item1 != null).Where(x =>
            {
                if (check != null)
                    return check.Invoke(x.Item1);

                return true;
            }).FirstOrDefault();
        }
    }
}