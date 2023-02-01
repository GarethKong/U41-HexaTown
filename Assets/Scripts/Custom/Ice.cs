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

public class Ice : MovableObject
{

     public void Setup(int i, int j, LevelGrid levelGrid)
       {
           sp.sortingOrder = 50-j;
           gridPosition = new Vector2Int(i, j);
           _levelGrid = levelGrid;
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
       }
}