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
using DG.Tweening;
using UnityEngine;

public class CustomTut12 : MonoBehaviour
{
    void Start()
    {
        var position = transform.localPosition;
        transform.DOLocalMove(new Vector3(position.x, position.y + 50),
            0.5f).SetEase(Ease.InSine);
    }
}