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
using Custom;
using DG.Tweening;
using Interface;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public SpriteRenderer sp;
     
    public void Setup(int i, int j)
    {
        sp.sortingOrder = 49 - j;
    }
            
    public void OpenDoor()
    {
        sp.sprite =  GameAssets.Instance.portOpenSprite;
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOScale(new Vector3(1.2f, 1.2f), 0.1f).SetEase(Ease.InSine))
            .Append(transform.DOScale(new Vector3(0.8f, 0.8f), 0.2f))
            .Append(transform.DOScale(new Vector3(1f, 1f), 0.2f));
    }
        
    public void CloseDoor()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOScale(new Vector3(0.8f, 0.8f), 0.3f).SetEase(Ease.InSine));
        sp.sprite =  GameAssets.Instance.portCloseSprite;
    }
}