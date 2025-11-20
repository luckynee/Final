using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEngine;

public class TestForModularity : MonoBehaviour
{
        private EventBindings<OnGameRoundStart> onGameRoundStart;
        
        private void Awake()
        {
            onGameRoundStart = new EventBindings<OnGameRoundStart>(HandleGameRoundStart);
            
            Bus<OnGameRoundStart>.Register(onGameRoundStart);
        }
        
        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void HandleGameRoundStart(OnGameRoundStart obj)
        {
            Debug.Log("Test for modularity works!");
            gameObject.SetActive(true);
            StartCoroutine(HideAfterSeconds(2f));
        }
        
        private IEnumerator HideAfterSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            gameObject.SetActive(false);
        }
}
