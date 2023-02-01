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
using CodeMonkey.Utils;
using Custom;
using DG.Tweening;
using Mkey;

public class TutorialManager : MonoBehaviour
{
    private static TutorialManager instance;

    public GameObject controller;
    private GameObject _snakeHead;
    private Vector3 originalPos;

    private bool isStartMove = false;

    private int stepGuide = 0;
    
    public GameObject tut1;
    public GameObject tut2;
    public GameObject tut3;
    private  Snake _snake;


    private void Awake()
    {
        instance = this;
    }

   public void StartRunTut()
    {
        StartCoroutine(StartTut());
        stepGuide += 1;
    }

   IEnumerator StartTut()
   {
       var position = tut1.transform.localPosition;
       yield return new WaitForSeconds(1f);
       tut1.SetActive(true);
       GameManager.instance.DoShakeCamera(0.25f);
       yield return new WaitForSeconds(1f);
       tut2.SetActive(true);
       GameManager.instance.DoShakeCamera(0.25f);
       yield return new WaitForSeconds(1f);
       tut3.SetActive(true);
       GameManager.instance.DoShakeCamera(0.25f);
       yield return new WaitForSeconds(0.25f);


       Direction direction = Direction.Right;
       Direction flyDirection = Direction.Left;
       Vector2Int flyVector = MovementMap.GetChangeReserve(direction) * 20;
       _snake.DoFlying(flyDirection, flyVector);
       

       tut1.transform.DOLocalMove(new Vector3(position.x, position.y + 1000,1000),
           2).SetEase(Ease.InSine);
       yield return new WaitForSeconds(0.5f);
       tut2.transform.DOLocalMove(new Vector3(position.x, position.y + 1000,1000),
           2).SetEase(Ease.InSine);
       yield return new WaitForSeconds(0.5f);
       tut3.transform.DOLocalMove(new Vector3(position.x, position.y + 1000,1000),
           2).SetEase(Ease.InSine);

       yield return new WaitForSeconds(2f);
       GameManager.instance.DoStopShakeCamera();
       GameManager.instance.OnWinLevel();
   }
   

    void Update()
    {
        // if (originalPos != position)
        // {
        //     isStartMove = true;
        // }
        if (_snakeHead != null && !isStartMove)
        {
            var position = _snakeHead.transform.position;
            controller.transform.position = position;
            originalPos = position;
            //controller.transform.localPosition = _snakeHead.transform.localPosition;
        }
    }

    public void SetupSnake(Snake snake)
    {
        _snake = snake;
        _snakeHead = snake.snakeBodyParts[0].gameObject;
        controller.SetActive(true);
    }

    public void HiddenGuideController()
    {
        controller.SetActive(false);
    }


    private void Show()
    {
        gameObject.SetActive(true);
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