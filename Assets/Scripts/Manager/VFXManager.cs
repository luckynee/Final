using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokerSeed.General
{
    public class VFXManager : MonoBehaviour
    {
        [SerializeField] private List<VFXData> vfxDataList;
        public static VFXManager Instance;

        private List<ParticleSystem> vfxParticleList = new List<ParticleSystem>();

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

            Init();
        }
        private void Init()
        {
            for (int i = 0; i < vfxDataList.Count; i++)
            {
                ParticleSystem spawnVFX = Instantiate(vfxDataList[i].VFXPrefab, transform);
                spawnVFX.transform.position = Vector2.zero;
                spawnVFX.name = vfxDataList[i].VFXTrigger.ToString();
                spawnVFX.gameObject.SetActive(false);
                vfxParticleList.Add(spawnVFX);
            }
        }

        public void PlayVFX(Enums.VFXTrigger _vfxTrigger, Vector2 _vfxPosition)
        {
            for(int i = 0; i < vfxParticleList.Count; i++)
            {
                if (vfxParticleList[i].name == _vfxTrigger.ToString())
                {
                    vfxParticleList[i].gameObject.SetActive(true);
                    vfxParticleList[i].transform.position = _vfxPosition;
                    vfxParticleList[i].Play();
                }
            }
        }
        public void SpawnVFX(Enums.VFXTrigger _vfxTrigger, Vector2 _vfxPosition)
        {
            for (int i = 0; i < vfxDataList.Count; i++)
            {
                if (vfxDataList[i].VFXTrigger == _vfxTrigger)
                {
                    ParticleSystem spawnVFX = Instantiate(vfxDataList[i].VFXPrefab, transform);
                    spawnVFX.transform.position = _vfxPosition;
                    spawnVFX.name = vfxDataList[i].VFXTrigger.ToString();
                    spawnVFX.Play();
                    StartCoroutine(VFXRoutine(spawnVFX));
                }
            }
        }
        private IEnumerator VFXRoutine(ParticleSystem _vfx)
        {
            yield return new WaitForSeconds(_vfx.main.duration);
            Destroy(_vfx.gameObject);
        }
        public void StopVFX()
        {
            for (int i = 0; i < vfxParticleList.Count; i++)
            {
                vfxParticleList[i].gameObject.SetActive(false);
                vfxParticleList[i].Stop();
            }
        }

        [System.Serializable]
        public struct VFXData
        {
            public Enums.VFXTrigger VFXTrigger;
            public ParticleSystem VFXPrefab;
        }
    }
}
