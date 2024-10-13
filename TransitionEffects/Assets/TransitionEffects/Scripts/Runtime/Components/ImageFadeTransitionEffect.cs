using System;
using UnityEngine;
using UnityEngine.UI;

namespace TransitionEffects.Components
{
    public class ImageFadeTransitionEffect : MonoBehaviour, ITransitionEffect
    {
        [SerializeField]
        private Image _image = null;

        [SerializeField]
        private bool _ignoreTimeScale = true;

        [Serializable]
        private class AnimationSettings
        {
            public string key;
            public float duration;
            public Color startColor;
            public Color endColor;
        }

        [SerializeField]
        private AnimationSettings[] _animations = null;

        private bool _isPlaying;

        private float _playElapsedTime;

        private float _duration;

        private Color _startColor;

        private Color _endColor;

        public bool IsAvailable => this != null && isActiveAndEnabled;

        public bool IsPlaying => _isPlaying;

        private void Reset()
        {
            _image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            for (int i = 0; i < _animations.Length; i++)
            {
                TransitionEffectManager.RegisterEffect(_animations[i].key, this);
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < _animations.Length; i++)
            {
                TransitionEffectManager.UnregisterEffect(_animations[i].key);
            }
        }

        public void Clear()
        {
            _image.enabled = false;
        }

        public void Play(string animationKey)
        {
            if (TryGetAnimation(animationKey, out var animation))
            {
                _isPlaying = true;
                _playElapsedTime = 0;
                _duration = animation.duration;
                _startColor = animation.startColor;
                _endColor = animation.endColor;
                _image.enabled = true;
                _image.color = _startColor;
            }
        }

        public void PlayImmediate(string animationKey)
        {
            if (TryGetAnimation(animationKey, out var animation))
            {
                _isPlaying = false;
                _image.color = animation.endColor;
                _image.enabled = animation.endColor.a > 0;
            }
        }

        private bool TryGetAnimation(string animationKey, out AnimationSettings animation)
        {
            for (int i = 0; i < _animations.Length; i++)
            {
                if (_animations[i].key == animationKey)
                {
                    animation = _animations[i];
                    return true;
                }
            }

            animation = default;
            return false;
        }

        private void Update()
        {
            if (!_isPlaying)
            {
                return;
            }

            _playElapsedTime += _ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
            var t = Mathf.Clamp01(_playElapsedTime / _duration);
            _image.color = Color.Lerp(_startColor, _endColor, t);
            if (_playElapsedTime > _duration)
            {
                _isPlaying = false;
                _image.enabled = _endColor.a > 0;
            }
        }
    }
}
