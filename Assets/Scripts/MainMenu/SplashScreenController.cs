using Kopsis.General;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using PokerSeed.General;
using UnityEngine.SceneManagement;

namespace Kopsis.MainMenuMode
{
    public class SplashScreenController : MonoBehaviour
    {
        [SerializeField] private AnimationHandler logoAnim;

        private void Start()
        {
            logoAnim.PlayAnimation(false, delegate
            {
                logoAnim.PlayAnimation(true, delegate
                {
                    logoAnim.StopAnimation();
                    SceneTransitionManager.Instance.PlaySceneTopBottomVerticalAnimation(false, null, delegate
                    {
                        SceneManager.LoadScene(Names.MainMenuSceneName);
                    });
                }, 1);
            });
        }
    }
}
