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
using UnityEngine;

public class Wood : MonoBehaviour
{

    
    public SpriteRenderer sp;
     
    public void Setup(int i, int j)
    {
        sp.sortingOrder = 50-j;
    }
    
}