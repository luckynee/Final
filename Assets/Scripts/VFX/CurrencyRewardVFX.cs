using DG.Tweening;
using Kopsis.General;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PokerSeed.VFX
{
    public class CurrencyRewardVFX : MonoBehaviour
    {
        [SerializeField] private float scaleDuration = 0.3f;
        [SerializeField] private float movingDuration = 0.8f;
        [SerializeField] private GameObject pileOfCurrency;
        [SerializeField] private Vector2[] initialPos;
        [SerializeField] private Quaternion[] initialRotation;
        [SerializeField] private RectTransform targetTransform;
        [SerializeField] private int currencyAmount;

        private int shownAmount;
        void Start()
        {
            initialPos = new Vector2[currencyAmount];
            initialRotation = new Quaternion[currencyAmount];

            for (int i = 0; i < pileOfCurrency.transform.childCount; i++)
            {
                initialPos[i] = pileOfCurrency.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition;
                initialRotation[i] = pileOfCurrency.transform.GetChild(i).GetComponent<RectTransform>().rotation;
            }
        }
        public void SetTarget(RectTransform _target, int _count)
        {
            targetTransform = _target;
            shownAmount = _count;
            if (shownAmount >= currencyAmount)
            {
                shownAmount = currencyAmount;
            }
            for (int i = 0; i < pileOfCurrency.transform.childCount; i++)
            {
                pileOfCurrency.transform.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < shownAmount; i++)
            {
                pileOfCurrency.transform.GetChild(i).gameObject.SetActive(true);
            }
            CountCoins();
        }
        public void CountCoins()
        {
            pileOfCurrency.SetActive(true);
            var delay = 0f;
            for (int i = 0; i < shownAmount; i++)
            {
                pileOfCurrency.transform.GetChild(i).DOScale(1f, scaleDuration).SetDelay(delay).SetEase(Ease.OutBack);
                pileOfCurrency.transform.GetChild(i).DOMove(targetTransform.position, movingDuration)
                    .SetDelay(delay + 0.1f).SetEase(Ease.InBack);
                pileOfCurrency.transform.GetChild(i).DOScale(0f, scaleDuration).SetDelay(delay + movingDuration).SetEase(Ease.OutBack);
                delay += 0.1f;
            }
            StartCoroutine(DestroyObject());
        }
        IEnumerator DestroyObject()
        {
            yield return new WaitForSeconds(2);
            Destroy(gameObject);
        }
    }
}
