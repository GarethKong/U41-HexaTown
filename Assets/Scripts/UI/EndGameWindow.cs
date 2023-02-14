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
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class EndGameWindow : MonoBehaviour
{
    private static EndGameWindow instance;

    //public Button btnNext;
    public Button btnHome;
    public TextMeshProUGUI currentScore;


    private void Awake()
    {
        instance = this;
        transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        transform.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

        // btnNext.onClick.AddListener(() =>
        // {
        //     SoundMaster.Instance.SoundPlayClick(0, null);
        // });

        btnHome.onClick.AddListener(() =>
        {
            SoundMaster.Instance.SoundPlayClick(0, null);
            Loader.Load(Loader.Scene.HomeScreen);
        });
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        TransitionHelper.TransitionIn(instance.gameObject);
        currentScore.text = "You just got " + Common.curScore  + " points";
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
}