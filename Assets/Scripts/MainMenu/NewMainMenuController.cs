using _PAProjcet;
using EventBus;
using PokerSeed.General;
using PokerSeed.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class NewMainMenuController : MonoBehaviour
    {
        [Header("GENERAL")]
        [Space(5)]
        [SerializeField] private Button startGameBtn;
        [SerializeField] private Button gardenBtn;
        //[SerializeField] private Button tutorialBtn;
        [SerializeField] private Button creditBtn;
        [SerializeField] private Button settingBtn;
        [SerializeField] private TextMeshProUGUI versionTxt;
        
        private void Start()
        {
             startGameBtn.onClick.AddListener(delegate
            {
                Bus<OnSfxTrigger>.Raise(new OnSfxTrigger(Enums.SFXType.BUTTON_CLICK));
                
                Bus<OnAmbienceStop>.Raise(new OnAmbienceStop(Enums.AmbienceType.MAIN_MENU));
                
                SceneTransitionManager.Instance.PlaySceneTopBottomVerticalAnimation(false, null, delegate
                {
                    SceneManager.LoadScene(Names.GameplaySceneName);
                });
            });
            gardenBtn.onClick.AddListener(delegate
            {
                Bus<OnSfxTrigger>.Raise(new OnSfxTrigger(Enums.SFXType.BUTTON_CLICK));
                
                Bus<OnAmbienceStop>.Raise(new OnAmbienceStop(Enums.AmbienceType.MAIN_MENU));
                
                SceneTransitionManager.Instance.PlaySceneTopBottomVerticalAnimation(false, null, delegate
                {
                    SceneManager.LoadScene(Names.GardenSceneName);
                });
            });
            
            creditBtn.onClick.AddListener(delegate
            {
                Bus<OnSfxTrigger>.Raise(new OnSfxTrigger(Enums.SFXType.BUTTON_CLICK));
                
                Bus<CreditBtnAddListenerEvent>.Raise(new CreditBtnAddListenerEvent(null));
                //creditPopup.OnOpen(null);
            });
            settingBtn.onClick.AddListener(delegate
            {
                Bus<OnSfxTrigger>.Raise(new OnSfxTrigger(Enums.SFXType.BUTTON_CLICK));
                
                Bus<SettingBtnAddListenerEvent>.Raise(new SettingBtnAddListenerEvent(null));

                //settingPopup.OnOpen(null);
            });
            
         
            
            Bus<MainMenuInit>.Raise(new MainMenuInit());
            
            Bus<OnAmbienceTrigger>.Raise(new OnAmbienceTrigger(Enums.AmbienceType.MAIN_MENU));
            
            SceneTransitionManager.Instance.PlaySceneTopBottomVerticalAnimation(true);

            versionTxt.text = "Version: " + Application.version;
            
        }
    }
}
