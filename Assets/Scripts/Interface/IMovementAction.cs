using System.Collections.Generic;
using Custom;
using UnityEngine;

namespace Interface
{
    public  interface IMovementAction
    {
         void TryToMove(Direction direction);
         void TryToEat(Vector2Int _gridPosition, Direction direction);
         void DoMoveFoodList(List<GameObject> listMovable, Direction direction);
         void DoEatBanana(Direction direction);
         void DoEatSpicy(Direction direction);
         void CheckBreakIce();
    }
}