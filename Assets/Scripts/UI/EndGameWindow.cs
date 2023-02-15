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
using ExaGames.Common;
using Mkey;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class EndGameWindow : MonoBehaviour
{
    private static EndGameWindow instance;

    //public Button btnNext;
    public Button btnHome;
    public Button btnPlayAgain;
    public TextMeshProUGUI currentMessage;
    public TextMeshProUGUI currentScore;
    public TextMeshProUGUI bestScore;
    public Sprite[] imgButtonPlayAgain;
    public Image spPlayAgain;


    private void Awake()
    {
        instance = this;
        transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        transform.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

        btnPlayAgain.onClick.AddListener(() =>
        {
            SoundMaster.Instance.SoundPlayClick(0, null);
            if (LivesManager.instance.Lives > 0)
            {
                transform.gameObject.SetActive(false);
                LivesManager.instance.ConsumeLife();
                GameManager.Instance.clearGame();
                GameManager.Instance.StartNewGame();
                GameManager.Instance.ShowDialogByEDialog(EDialog.PLAY);
            }
            else
            {
                //TODO Show ads then play again
                transform.gameObject.SetActive(false);
                LivesManager.instance.ConsumeLife();
                GameManager.Instance.clearGame();
                GameManager.Instance.StartNewGame();
                GameManager.Instance.ShowDialogByEDialog(EDialog.PLAY);
            }
        });

        btnHome.onClick.AddListener(() =>
        {
            SoundMaster.Instance.SoundPlayClick(0, null);
            Loader.Load(Loader.Scene.HomeScreen);
        });
        Hide();

        if (LivesManager.instance.Lives > 0)
        {
            spPlayAgain.sprite = imgButtonPlayAgain[0];
        }
        else
        {
            spPlayAgain.sprite = imgButtonPlayAgain[1];
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
        TransitionHelper.TransitionIn(instance.gameObject);
        currentScore.text = Common.curScore + " points";
        bestScore.text = "Best score: " + Common.maxScore;
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

    public static void loadMessage(int score)
    {
        var rank = "";
        var message1 = "";
        var message2 = "";
        if (score < 70)
        {
            // E rank
            rank = "Rank: E";
            message1 = " Finished! ";
            message2 = "(Next rank at 70 points)";
        }
        else if (score < 80)
        {
            // D rank
            rank = "Rank: D";
            message1 = " Not bad! ";
            message2 = "(Next rank at 80 points)";
        }
        else if (score < 90)
        {
            // C rank
            rank = "Rank: C";
            message1 = " Good job! ";
            message2 = "(Next rank at 90 points)";
        }
        else if (score < 100)
        {
            // B rank
            rank = "Rank: B";
            message1 = " Well done! ";
            message2 = "(Next rank at 100 points)";
        }
        else if (score < 110)
        {
            // A rank
            rank = "Rank: A";
            message1 = " Excellent! ";
            message2 = "(Next rank at 110 points)";
        }
        else if (score < 120)
        {
            // A+ rank
            rank = "Rank: A+";
            message1 = " Nearly flawless! ";
            message2 = "(Next rank at 120 points)";
        }
        else
        {
            // S rank
            rank = "Rank: S";
            message1 = " Incredible!! ";
            message2 = "(This is the highest rank!)";
        }
    }
}