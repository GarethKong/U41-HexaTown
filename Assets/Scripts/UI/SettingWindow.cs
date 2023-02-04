/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using BeautifulTransitions.Scripts.Transitions;
using UnityEngine;
using CodeMonkey.Utils;
using Mkey;
using UnityEngine.UI;

public class SettingWindow : MonoBehaviour
{
    private static SettingWindow instance;

    public Button_UI btnSound;
    public Button_UI btnBackground;
    public Button_UI btnVibration;

    public Button_UI btnHome;
    public Button_UI btnTutorial;
    public Button_UI btnClose;

    public Sprite[] soundSprite;
    public Sprite[] musicSprite;
    public Sprite[] vibrationSprite;

    private void Awake()
    {
        instance = this;

        btnSound.ClickFunc = () =>
        {
            SoundMaster.Instance.SoundPlayClick(0, null);
            bool currentState = SoundMaster.Instance.SoundOn;
            SoundMaster.Instance.SetSound(!currentState);
            UpdateState();
        };
        //btnNext.AddButtonSounds();

        btnBackground.ClickFunc = () =>
        {
            SoundMaster.Instance.SoundPlayClick(0, null);
            bool currentState = SoundMaster.Instance.MusicOn;
            SoundMaster.Instance.SetMusic(!currentState);
            UpdateState();
        };

        btnVibration.ClickFunc = () =>
        {
            SoundMaster.Instance.SoundPlayClick(0, null);
            bool currentState = SoundMaster.Instance.VibrationOn;
            SoundMaster.Instance.SetVibration(!currentState);
            UpdateState();
        };

        btnHome.ClickFunc = () =>
        {
            SoundMaster.Instance.SoundPlayClick(0, null);
            Loader.Load(Loader.Scene.LevelScreen);
        };

        btnTutorial.ClickFunc = () => { SoundMaster.Instance.SoundPlayClick(0, null); };

        btnClose.ClickFunc = () =>
        {
            SoundMaster.Instance.SoundPlayClick(0, null);
            GameManager.Instance.HideDialog(EDialog.SETTING);
        };
        Hide();
    }

    private void UpdateState()
    {
        btnSound.gameObject.GetComponent<Image>().sprite =
            SoundMaster.Instance.SoundOn ? soundSprite[0] : soundSprite[1];
        btnBackground.gameObject.GetComponent<Image>().sprite =
            SoundMaster.Instance.MusicOn ? musicSprite[0] : musicSprite[1];
        btnVibration.gameObject.GetComponent<Image>().sprite =
            SoundMaster.Instance.VibrationOn ? vibrationSprite[0] : vibrationSprite[1];
    }
    
    private void Show()
    {
        gameObject.SetActive(true);
        UpdateState();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public static void ShowStatic()
    {
        instance.Show();
        TransitionHelper.TransitionIn(instance.gameObject);
    }

    public static void HideStatic()
    {
        instance.Hide();
    }
}