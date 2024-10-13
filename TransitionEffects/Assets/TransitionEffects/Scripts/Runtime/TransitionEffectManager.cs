using System.Collections.Generic;
using UnityEngine;

namespace TransitionEffects
{
    public static class TransitionEffectManager
    {
        private static Dictionary<string, ITransitionEffect> _transitionEffects = new Dictionary<string, ITransitionEffect>();
        private static string _lastPlayedEffectKey;

        public static void RegisterEffect(string key, ITransitionEffect effect)
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("Key is null or empty.");
                return;
            }
            _transitionEffects[key] = effect;
            if (key == _lastPlayedEffectKey)
            {
                PlayImmediate(key);
            }
        }

        public static void UnregisterEffect(string key)
        {
            _transitionEffects.Remove(key);
        }

        public static void ClearPlayingEffect()
        {
            if (string.IsNullOrEmpty(_lastPlayedEffectKey))
            {
                return;
            }
            if (_transitionEffects.TryGetValue(_lastPlayedEffectKey, out var effect) && effect != null && effect.IsAvailable)
            {
                effect.Clear();
            }
            _lastPlayedEffectKey = string.Empty;
        }

        public static void Play(string key)
        {
            ClearPlayingEffect();
            if (_transitionEffects.TryGetValue(key, out var effect) && effect != null && effect.IsAvailable)
            {
                effect.Play(key);
                _lastPlayedEffectKey = key;
            }
            else
            {
                Debug.LogError($"Effect with key {key} not found.");
            }
        }

        public static void PlayImmediate(string key)
        {
            ClearPlayingEffect();
            if (_transitionEffects.TryGetValue(key, out var effect) && effect != null && effect.IsAvailable)
            {
                effect.PlayImmediate(key);
                _lastPlayedEffectKey = key;
            }
            else
            {
                Debug.LogError($"Effect with key {key} not found.");
            }
        }

        public static bool IsPlaying()
        {
            if (string.IsNullOrEmpty(_lastPlayedEffectKey))
            {
                return false;
            }
            if (_transitionEffects.TryGetValue(_lastPlayedEffectKey, out var effect) && effect != null && effect.IsAvailable)
            {
                return effect.IsPlaying;
            }
            return false;
        }

        public static bool IsPlaying(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            if (key != _lastPlayedEffectKey)
            {
                return false;
            }
            return IsPlaying();
        }
    }
}
