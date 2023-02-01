/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using BeautifulTransitions.Scripts.Transitions;
using UnityEngine;
using Custom;
using DG.Tweening;
using Interface;
using Mkey;
using UnityEngine.UI;

public enum EDialog
{
    PLAY,
    SETTING,
    LOSE,
    WIN
}


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public  TutorialManager tutorialNode;

    public enum StateGame
    {
        Idle,
        Restarting,
        Undoing,
        Dead
    }

    public LevelGrid levelGrid;

    public TextAsset mapText;

    public GameObject mapNode;

    public GameObject obstacleNode;
    public GameObject foodNode;
    public GameObject snakeNode;
    private StateGame stateGame;

    public Text txtLevel;
    public Text stateWorm;

    public Camera cameraGame;

    private Tweener tweener;
    public static bool isPauseGame;


    public GameObject[] listDlg;

    public GameObject background;
    
    public GameObject effectNode;


    private void Awake()
    {
        instance = this;
        background.SetActive(false);
        Score.InitializeStatic();
        Time.timeScale = 1f;
        MovementMap.Init();
    }

    private void Start()
    {
        Debug.Log("GameHandler.Start");
        CustomEventManager.Instance.OnFailLevel += OnFailLevel;
        CustomEventManager.Instance.OnWinLevel += OnWinLevel;
        CustomEventManager.Instance.OnNextLevel += OnNextLevel;
        StartNewGame();
    }

    public void StartNewGame()
    {
        stateGame = StateGame.Idle;
        int currentLevel = Common.GetLevelNumberNeedLoad();
        txtLevel.text = "Level " + currentLevel;
        levelGrid.StartNewGame(currentLevel);
    }

    // public void StartNewGameByLevel(int level)
    // {
    //     stateGame = StateGame.Idle;
    //     levelGrid.StartNewGame(level);
    // }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsGamePaused())
            {
                GameManager.ResumeGame();
            }
            else
            {
                GameManager.PauseGame();
            }
        }
    }

    public static void ResumeGame()
    {
        SettingWindow.HideStatic();
        Time.timeScale = 1f;
    }

    public static void PauseGame()
    {
        SettingWindow.ShowStatic();
        Time.timeScale = 0f;
    }

    public static bool IsGamePaused()
    {
        return Time.timeScale == 0f;
    }

    public void DoShakeCamera(float time)
    {
        tweener = cameraGame.DOShakePosition(time, 0.1f, 5, 90f);
    }
    
    public void DoStopShakeCamera()
    {
        tweener.Kill();
    }

    public void HandleController(int direction)
    {
        Debug.Log("Click button r nhe" + direction);

        if (stateGame == StateGame.Idle)
        {
            Snake.Instance.HandleButtonInput((Direction)direction);
        }
    }

    public void OnRestartGame()
    {
        DoStopShakeCamera();
        levelGrid.ClearBoard();
        levelGrid.StartNewGame(levelGrid._currentLevel);
    }

    public void OnFailLevel()
    {
        levelGrid.snake.state = Snake.StateSnake.Dead;
        Debug.Log("CHECK DEAD GAN LAI");
        StartCoroutine(ESnakeDied(2f));
    }

    public void OnWinLevel()
    {
        levelGrid.snake.state = Snake.StateSnake.Win;
        Debug.Log("CHECK DEAD GAN LAI");
        StartCoroutine(EWinLevel(1f));
    }

    public void OnNextLevel()
    {
        levelGrid.snake.state = Snake.StateSnake.Win;
        Debug.Log("CHECK DEAD GAN LAI");
        StartCoroutine(ENextLevel(2f));
    }

    IEnumerator ESnakeDied(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        ShowDialogByEDialog(EDialog.LOSE);
        //GamePlayWindow.HideStatic();
    }

    IEnumerator EWinLevel(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (Common.maxLevelUnlocked == Common.GetLevelNumberNeedLoad())
        {
            Common.SaveNextStage();
        }
        ShowDialogByEDialog(EDialog.WIN);
        //GamePlayWindow.HideStatic();
    }

    IEnumerator ENextLevel(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
    }

   public  void SettingHandle()
    {
        SoundMaster.Instance.SoundPlayClick(0, null);
        ShowDialogByEDialog(EDialog.SETTING);
    }


    void ShowDialogByEDialog(EDialog dialogType)
    {
        foreach (var t in listDlg)
        {
            t.SetActive(false);
        }

        background.SetActive(true);
        switch (dialogType)
        {
            case EDialog.SETTING:
                SettingWindow.ShowStatic();
                break;
            case EDialog.LOSE:
                GameOverWindow.ShowStatic();
                break;
            case EDialog.WIN:
                WinWindow.ShowStatic();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dialogType), dialogType, null);
        }
    }

    public void HideDialog(EDialog dialogType)
    {
        background.SetActive(false);
        foreach (var t in listDlg)
        {
            t.SetActive(false);
        }

        switch (dialogType)
        {
            case EDialog.SETTING:
                SettingWindow.HideStatic();
                break;
            case EDialog.LOSE:
                GameOverWindow.HideStatic();
                break;
            case EDialog.WIN:
                WinWindow.HideStatic();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dialogType), dialogType, null);
        }
    }
}