using PokerSeed.General;
using PokerSeed.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PokerSeed.MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("GENERAL")]
        [Space(5)]
        [SerializeField] private Button startGameBtn;
        [SerializeField] private Button gardenBtn;
        //[SerializeField] private Button tutorialBtn;
        [SerializeField] private Button creditBtn;
        [SerializeField] private Button settingBtn;
        [SerializeField] private TextMeshProUGUI versionTxt;

        [Header("POPUP")]
        [Space(5)]
        [SerializeField] private TutorialPopup tutorialPopup;
        [SerializeField] private SettingPopup settingPopup;
        [SerializeField] private CreditPopup creditPopup;


        private void Awake()
        {
            startGameBtn.onClick.AddListener(delegate
            {
                AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
                SceneTransitionManager.Instance.PlaySceneTopBottomVerticalAnimation(false, null, delegate
                {
                    SceneManager.LoadScene(Names.GameplaySceneName);
                });
            });
            gardenBtn.onClick.AddListener(delegate
            {
                AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
                SceneTransitionManager.Instance.PlaySceneTopBottomVerticalAnimation(false, null, delegate
                {
                    SceneManager.LoadScene(Names.GardenSceneName);
                });
            });
            /*
            tutorialBtn.onClick.AddListener(delegate
            {
                AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
                tutorialPopup.OnOpen();
            });
            */
            creditBtn.onClick.AddListener(delegate
            {
                AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
                creditPopup.OnOpen(null);
            });
            settingBtn.onClick.AddListener(delegate
            {
                AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
                settingPopup.OnOpen(null);
            });
            

            tutorialPopup.Init();
            settingPopup.Init();
            creditPopup.Init();
        }
        private void Start()
        {
            
            AudioManager.Instance.PlayAmbience(Enums.AmbienceType.MAIN_MENU);
            SceneTransitionManager.Instance.PlaySceneTopBottomVerticalAnimation(true);

            versionTxt.text = "Version: " + Application.version;
        }
    }
}

