using System;
using System.Diagnostics.Contracts;
using _PAProjcet;
using PokerSeed.Config;
using PokerSeed.Gameplay;
using PokerSeed.Player;
using UnityEngine;

namespace Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager Instance;

        public event Action OnTutorialChanged;
        
        [SerializeField] private NewCardController cardController;
        [SerializeField] private ToolController toolController;
        [SerializeField] private GameConfigData tutorialConfigData;
        [SerializeField] private UI_Tutorial uI_Tutorial;
        
        [SerializeField] private bool _isOnTutorial = false;

        private int _currentTutorialIndex = 0;
        public int CurrentTutorialIndex => _currentTutorialIndex;
        public bool IsOnTutorial => _isOnTutorial;
        public GameConfigData GetTutorialConfigData => tutorialConfigData;

        public Enums.TutorialStep tutorialStep = Enums.TutorialStep.NORMAL_TUTORIAL;

        private void Awake()
        {
            Instance = this;
            if (PlayerDataManager.Instance.HasShownTutorial)
            {
                _isOnTutorial = false;
            }
        }

        private void Start()
        {
            cardController.onTutorialCountPoint += CardController_OnTutorialCountPoint;
        }

        private void OnDisable()
        {
            cardController.onTutorialCountPoint -= CardController_OnTutorialCountPoint;
        }

        private void CardController_OnTutorialCountPoint()
        {
            tutorialStep = Enums.TutorialStep.STOP_TIME;
        }

        private void Update()
        {
            if (_isOnTutorial)
            {
                HandleSpecialTutorial();
                //Debug.Log(_currentTutorialIndex);
            }
        }
        
        public GameObject GetActiveTutorial()
        {
            return uI_Tutorial.GetCurrentActiveTutorial();
        }
        public void NextTutorial()
        {
            if (!_isOnTutorial) return;
            _currentTutorialIndex++;
            OnTutorialChanged?.Invoke();
        }

        public void ButtonNextTutorial()
        {
            if (!_isOnTutorial ) return;
            if(tutorialStep == Enums.TutorialStep.NORMAL_TUTORIAL) return;
            _currentTutorialIndex++;
            OnTutorialChanged?.Invoke();
        }

        public void SpecialNext()
        {
            NextTutorial();
        }

        public void FinishTutorial()
        {
            if (!_isOnTutorial) return;
            if(tutorialStep == Enums.TutorialStep.NORMAL_TUTORIAL) return;  
            _isOnTutorial = false;
        }

        public void ChangeStateTutorial()
        {
            tutorialStep = Enums.TutorialStep.RESUME_TIME;
        }

        public void ChangeToNormalTutorial()
        {
            tutorialStep = Enums.TutorialStep.NORMAL_TUTORIAL;
        }

        public void PreviousTutorial()
        {   
            _currentTutorialIndex--;
        }

        private void HandleSpecialTutorial()
        {
            switch (tutorialStep)
            {
                case Enums.TutorialStep.SECOND_ROUND:
                    cardController.ChangeGameState(Enums.GameState.SECOND_ROUND);
                    NextTutorial();
                    tutorialStep = Enums.TutorialStep.NORMAL_TUTORIAL;
                    break;
                case Enums.TutorialStep.THIRD_ROUND:
                    cardController.ChangeGameState(Enums.GameState.THIRD_ROUND);
                    NextTutorial();
                    StopTime();
                    tutorialStep = Enums.TutorialStep.NORMAL_TUTORIAL;
                    break;
                case Enums.TutorialStep.RESUME_TIME:
                    ResumeTime();
                    break;
                case Enums.TutorialStep.STOP_TIME:
                    StopTime();
                    NextTutorial();
                    tutorialStep = Enums.TutorialStep.NORMAL_TUTORIAL;
                    break;
            }
        }

        private void StopTime()
        {
            Time.timeScale = 0;
        }

        private void ResumeTime()
        {
            Time.timeScale = 1;
        }
        
    }
}
