using PokerSeed.General;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

namespace PokerSeed.UI
{
    public class FailurePopup : MonoBehaviour
    {
        [SerializeField] private AnimationHandler animHandler;
        [SerializeField] private Button restartBtn;

        #region DEFAULT
        public void Init()
        {
            restartBtn.onClick.AddListener(delegate
            {
                AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
                OnRestart(delegate
                {
                    SceneTransitionManager.Instance.PlaySceneTopBottomVerticalAnimation(false, null,
                    delegate
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    });
                });
            });
        }

        public void OnOpen(Action _openPopupCallback)
        {
            AudioManager.Instance.PlaySFX(Enums.SFXType.FAIL);
            gameObject.SetActive(true);
            animHandler.PlayAnimation(false, delegate
            {
                animHandler.StopAnimation(true);
                _openPopupCallback?.Invoke();
            });
        }
        private void OnRestart(Action _changeSceneCallback)
        {
            animHandler.PlayAnimation(true, delegate
            {
                animHandler.StopAnimation();    
                _changeSceneCallback?.Invoke();
                gameObject.SetActive(false);
            });
        }
        #endregion
    }
}
