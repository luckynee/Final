using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace PokerSeed.General
{
    public class SceneTransitionManager : MonoBehaviour
    {
        [SerializeField] private AnimationHandler sceneFullFadeAnim;
        [SerializeField] private List<AnimationHandler> sceneTopBottomVerticalAnim;

        public static SceneTransitionManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        public void PlaySceneFullFadeAnimation(
            Action _startCallback = null, 
            Action _middleCallback = null, 
            Action _endCallback = null)
        {
            _startCallback?.Invoke();
            sceneFullFadeAnim.PlayAnimation(false, delegate
            {
                _middleCallback?.Invoke();
                sceneFullFadeAnim.PlayAnimation(true, delegate
                {
                    _endCallback?.Invoke();
                    sceneFullFadeAnim.StopAnimation();
                });
            });
        }
        public void PlaySceneTopBottomVerticalAnimation(
            bool _isStart,
            Action _startCallback = null,
            Action _endCallback = null)
        {
            _startCallback?.Invoke();
            sceneTopBottomVerticalAnim[0].transform.parent.gameObject.SetActive(true);
            sceneTopBottomVerticalAnim[0].PlayAnimation(_isStart, delegate
            {
                sceneTopBottomVerticalAnim[0].StopAnimation(!_isStart);
                _endCallback?.Invoke();
            });
            sceneTopBottomVerticalAnim[1].PlayAnimation(_isStart, delegate 
            {
                sceneTopBottomVerticalAnim[1].StopAnimation(!_isStart);
            });
        }
    }
}
