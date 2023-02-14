/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using BeautifulTransitions.Scripts.Transitions;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using DG.Tweening;
using ExaGames.Common;
using Mkey;

public class GameOverWindow : MonoBehaviour
{
    public static GameOverWindow instance;

    public Text lblLife;

    public Text lblCurrentLevel;
    public Text lblTimeCountNextLife;

    public GameObject UINode;
    public Button_UI btnRetry;
    public Button_UI btnHome;

    public Text TimeCountNextLife;

    private void Awake()
    {
        instance = this;
        btnRetry.ClickFunc = () =>
        {
            if (LivesManager.instance.Lives <= 0)
            {
                //SHOW ADS TO PLAY AGAIN
            }
            else
            {
                LivesManager.instance.ConsumeLife();
                SoundMaster.Instance.SoundPlayClick(0, null);
                Loader.Load(Loader.Scene.GameScene);
            }
        };
        //btnRetry.AddButtonSounds();

        btnHome.ClickFunc = () =>
        {
            SoundMaster.Instance.SoundPlayClick(0, null);
            Loader.Load(Loader.Scene.HomeScreen);
        };
        //btnHome.AddButtonSounds();
        if (LivesManager.instance.Lives < 5)
        {
            lblLife.text = LivesManager.instance.Lives + 1 + "";
        }
        else
        {
            lblLife.text = "5";
        }

        Hide();
    }

    private void Update()
    {
        if (LivesManager.instance.Lives == 5)
        {
            TimeCountNextLife.text = "";
        }
        else
        {
            TimeCountNextLife.text = "Next life in " + LivesManager.instance.RemainingTimeString;
        }
    }

    private void Show()
    {
        lblCurrentLevel.text = "Level " + Common.GetLevelNumberNeedLoad();
        gameObject.SetActive(true);

        if (LivesManager.instance.Lives != 5)
        {
            lblTimeCountNextLife.gameObject.SetActive(true);
            lblTimeCountNextLife.text = "Next life in " + LivesManager.instance.RemainingTimeString;
        }
        else
        {
            lblTimeCountNextLife.gameObject.SetActive(false);
        }

        StartCoroutine(instance.SpawnLifeText());
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

    IEnumerator RemoveLifeEffect(GameObject gameObject)
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    IEnumerator SpawnLifeText()
    {
        yield return new WaitForSeconds(1.2f);
        lblLife.text = LivesManager.instance.LivesText;
        var position = lblLife.transform.position;
        GameObject boxMap =
            Instantiate(SpriteMgr.Instance.lifeEffect, position, Quaternion.identity);
        boxMap.transform.SetParent(UINode.transform);

        Transform boxMapTransform = boxMap.transform;

        Vector3 newPos = new Vector3(boxMapTransform.position.x, boxMapTransform.position.y + 50);

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(boxMapTransform.DOMove(newPos, 1.5f).SetEase(Ease.InSine))
            .Append(boxMapTransform.DOScale(new Vector3(1.2f, 1.2f), 0.2f))
            .Append(boxMapTransform.DOScale(new Vector3(1f, 1f), 0.2f));
        StartCoroutine(RemoveLifeEffect(boxMap));
    }
}