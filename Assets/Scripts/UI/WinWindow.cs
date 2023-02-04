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

public class WinWindow : MonoBehaviour
{
    private static WinWindow instance;

    public Button_UI btnNext;
    public Button_UI btnHome;
    public Text levelCurrent;


    private void Awake()
    {
        instance = this;
        transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        transform.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

        btnNext.ClickFunc = () =>
        {
            SoundMaster.Instance.SoundPlayClick(0, null);
            DoNextLevel();
        };
        //btnNext.AddButtonSounds();

        btnHome.ClickFunc = () =>
        {
            SoundMaster.Instance.SoundPlayClick(0, null);
            Loader.Load(Loader.Scene.LevelScreen);
        };
        //btnHome.AddButtonSounds();
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        TransitionHelper.TransitionIn(instance.gameObject);
        if ( Common.GetLevelNumberNeedLoad() !=0)
        {
            levelCurrent.text = "Level " + Common.GetLevelNumberNeedLoad();
        }
        else
        {
            levelCurrent.text = "Tutorial";
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public static void ShowStatic()
    {
        instance.Show();
    }

    public static void HideStatic()
    {
        instance.Hide();
    }

    public void DoNextLevel()
    {
        HideStatic();
        GamePlayWindow.ShowStatic();
        Common.SetLevelNumberNeedLoad(Common.currentStageLoad+1);
        GameManager.Instance.StartNewGame();
    }
}