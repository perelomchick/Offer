using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class LoadingCurtainView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Slider _progressBar;
        [SerializeField] private float _fadeDuration = 0.4f;
        [SerializeField] private float _minShowTime = 0.5f;

        private readonly ReactiveProperty<float> _progress = new();
        private float _showAt;

        private void Awake()
        {
            Debug.Log("[Test] Curtain Awake");
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;

            _progress
                .Subscribe(x => _progressBar.value = x)
                .AddTo(this);

            _progress.AddTo(this);
        }

        public void SetProgress(float value) => _progress.Value =  Mathf.Clamp01(value);
        
        public async UniTask Show(CancellationToken ct)
        {
            _showAt = Time.unscaledTime;
            _progress.Value = 0;
            await Fade(1f, ct);
        }

        public async UniTask Hide(CancellationToken ct = default)
        {
            float reaming = _minShowTime - (Time.unscaledTime - _showAt);
            if(reaming > 0f)
                await UniTask.Delay(TimeSpan.FromSeconds(reaming),DelayType.UnscaledDeltaTime,cancellationToken: ct);
            
            await Fade(0f, ct);
        }

        private async UniTask Fade(float target, CancellationToken token)
        {
            _canvasGroup.blocksRaycasts = true;
            float startTime = _canvasGroup.alpha;

            for (float t = 0f; t < _fadeDuration; t += Time.unscaledDeltaTime)
            {
                _canvasGroup.alpha = Mathf.Lerp(startTime, target, t/_fadeDuration);
                await UniTask.Yield(token);
            }
            
            _canvasGroup.alpha = target;
            _canvasGroup.blocksRaycasts = target > 0.5f;
        }
    }
}