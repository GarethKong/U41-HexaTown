using Custom;
using UnityEngine;

namespace Interface
{
    public  interface ISnakeAction
    {
         void DoFlying(Direction direction, Vector2Int changeValue);
         void UpdateSnakePositionParts(Vector2Int changeValue);
    }
}