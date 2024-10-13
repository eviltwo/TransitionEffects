using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TransitionEffects.Samples
{
    [AddComponentMenu("")] // Hide in the Add Component menu
    public class SceneController : MonoBehaviour
    {
        [SerializeField]
        private string _sceneName = string.Empty;

        [SerializeField]
        private string _startEffectKey = string.Empty;

        [SerializeField]
        private string _endEffectKey = string.Empty;

        private void Start()
        {
            TransitionEffectManager.Play(_endEffectKey);
        }

        public void LoadScene()
        {
            StartCoroutine(LoadSceneWithTransition());
        }

        private IEnumerator LoadSceneWithTransition()
        {
            TransitionEffectManager.Play(_startEffectKey);
            yield return new WaitWhile(() => TransitionEffectManager.IsPlaying());
            SceneManager.LoadScene(_sceneName);
        }
    }
}
