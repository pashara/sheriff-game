using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sheriff.InputLock;
using UniRx;
using UnityEngine;

namespace Sheriff.GameFlow.OverlayLoader
{
    public class OverlayLoadingHandler : MonoBehaviour
    {
        [SerializeField] private GameObject inputLocker;
        [SerializeField] private CanvasGroup loader;

        private readonly CompositeDisposable _disposable = new();

        private void Awake()
        {
            loader.gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            var property = LoadingOverlay.IsLocked;

            property.Subscribe(x => inputLocker.SetActive(x)).AddTo(_disposable);
            
            property
                .Where(value => value)
                .Throttle(TimeSpan.FromSeconds(3))
                .Where(x => property.Value)
                .Subscribe(_ =>
                {
                    Affect(1f, 0.5f);
                })
                .AddTo(_disposable);
            
            property
                .Where(value => !value)
                .Throttle(TimeSpan.FromSeconds(0.5)) 
                .Where(x => !property.Value)
                .Subscribe(_ =>
                {
                    Affect(0f, 0.05f);
                })
                .AddTo(_disposable);
        }

        private void OnDisable()
        {
            _disposable.Clear();
        }
        
        TweenerCore<float, float, FloatOptions> Affect(float alpha, float duration)
        {
            DOTween.Kill(loader);
            return DOTween.To(() => 0f, (x) =>
            {
                loader.alpha = x;
            }, alpha, duration).SetTarget(loader);
        }
    }
}