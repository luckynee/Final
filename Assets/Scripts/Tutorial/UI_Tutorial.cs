using System;
using TMPro;
using UnityEngine;

namespace Tutorial
{
    public class UI_Tutorial : MonoBehaviour   
    {
        [SerializeField] private GameObject[] uI_Tutorial;
        private void Start()
        {
            if (TutorialManager.Instance.IsOnTutorial)
            {
                TutorialManager.Instance.OnTutorialChanged += OnTutorialChanged;
                
                ShowCurrentTutorial();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        
        private void OnTutorialChanged()
        {
            ShowCurrentTutorial();
        }
        
        private void ShowCurrentTutorial()
        {
            uI_Tutorial[TutorialManager.Instance.CurrentTutorialIndex].SetActive(true);
            if(TutorialManager.Instance.CurrentTutorialIndex == 0) return;
            
            uI_Tutorial[TutorialManager.Instance.CurrentTutorialIndex - 1].SetActive(false);
        }
        
        public GameObject GetCurrentActiveTutorial()
        {
            return uI_Tutorial[TutorialManager.Instance.CurrentTutorialIndex];
        }
        public int GetTotalTutorial()
        {
            return uI_Tutorial.Length;
        }
    }
}
