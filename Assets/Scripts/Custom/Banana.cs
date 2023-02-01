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
using Mkey;
using UnityEngine;

public class Banana : MovableObject
{

    public void Setup(int i, int j, LevelGrid levelGrid)
    {
        gridPosition = new Vector2Int(i, j);
        _levelGrid = levelGrid;
        sp.sortingOrder = 50 - j;
    }

    private void Awake()
    {
        gridMoveDirection = Direction.Right;
        foodState = FoodState.Idle;
    }

    public override void DoDropping(Vector2Int newPos)
    {
        SoundMaster.Instance.SoundPlayByEnum(ESoundType.FoodDrop, 0, null);
        base.DoDropping(newPos);
        CustomEventManager.Instance.FailLevel();
        CustomEventManager.Instance.FoodDrop();
    }
  
}