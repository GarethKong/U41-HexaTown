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
using System.Collections.Generic;
using UnityEngine;

public class SpriteMgr : MonoBehaviour
{

    public static SpriteMgr Instance;

    private void Awake() {
        Instance = this;
    }
 
    public Sprite[] hexList;
    public Sprite[] sketchyList;
    public Sprite[] deckCounterImage;
    public Sprite[] triPreviewsRed;
    public Sprite[] triPreviews;
    public Sprite redSpriteFrame;
    public GameObject lifeEffect;
}
