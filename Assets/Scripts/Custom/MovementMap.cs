using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Custom
{
    public class MovementMap
    {
        
        private static Dictionary<Direction, Vector2Int> changeMap = new Dictionary<Direction, Vector2Int>();
        private static Dictionary<Vector2Int, Direction> directionMap = new Dictionary<Vector2Int, Direction>();

        public static void Init()
        {
            changeMap = new Dictionary<Direction, Vector2Int>();
            changeMap.Add(Direction.Up, new Vector2Int(0, 1));
            changeMap.Add(Direction.Down, new Vector2Int(0, -1));
            changeMap.Add(Direction.Left, new Vector2Int(-1, 0));
            changeMap.Add(Direction.Right, new Vector2Int(1, 0));
            changeMap.Add(Direction.None, new Vector2Int(0, 0));
            
            directionMap = new Dictionary<Vector2Int, Direction>();
            directionMap.Add(new Vector2Int(0, 1), Direction.Up);
            directionMap.Add(new Vector2Int(0, -1), Direction.Down );
            directionMap.Add(new Vector2Int(-1, 0),Direction.Left);
            directionMap.Add(new Vector2Int(1, 0),Direction.Right);
            directionMap.Add(new Vector2Int(0, 0), Direction.None);
        }

        public static Direction GetDirection(Vector2Int vector2d)
        {
            return directionMap[vector2d];
        }

        public static Vector2Int GetChange(Direction direction)
        {
            return changeMap[direction];
        }

        
        //Tra ve vector nguoc huong
        public static Vector2Int GetChangeReserve(Direction direction)
        {
            return direction switch
            {
                Direction.Up => changeMap[Direction.Down],
                Direction.Down => changeMap[Direction.Up],
                Direction.Left => changeMap[Direction.Right],
                Direction.Right => changeMap[Direction.Left],
                _ => changeMap[Direction.Up]
            };
        }
        
        public static int GetRotationByDirection(Direction direction)
        {
            return direction switch
            {
                Direction.Down => 90,
                Direction.Up =>270,
                Direction.Right => 180,
                Direction.Left => 0,
                _ => 0
            };
        }

        public static Direction GetFlyDirection(Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Down,
                Direction.Down => Direction.Up,
                Direction.Left => Direction.Right,
                Direction.Right => Direction.Left,
                _ => Direction.Up
            };
        }

        public static Vector2Int PosToRowColumn(Vector2Int position)
        {
            return new Vector2Int(position.y, position.x);
        }
        

        public static BodyPartType GetBodyTypeByPos(int index, List<Vector2Int> snakePositionParts)
        {
            Vector2Int b1, b2;
            if (snakePositionParts[index].x == snakePositionParts[index - 1].x)
            {
                b2 = snakePositionParts[index + 1];
                b1 = snakePositionParts[index - 1];
            }
            else
            {
                Debug.Log("CHECK INDEX"+ index+ "CURRENT LENGTH" + snakePositionParts.Count);
                b2 = snakePositionParts[index - 1];
                b1 = snakePositionParts[index + 1];
            }
            if (b1.x < b2.x && b1.y > b2.y)
            {
                return BodyPartType.BodyLR;
            }
            if (b1.x > b2.x && b1.y > b2.y)
            {
                return BodyPartType.BodyLL;
            }
            if (b1.x > b2.x && b1.y < b2.y)
            {
                return BodyPartType.BodyLLD;
            }
            if (b1.x < b2.x && b1.y < b2.y)
            {
                return BodyPartType.BodyLRD;
            }
            
            if (b1.x == b2.x)
            {
                //doc 
                return BodyPartType.Body;
            }
        
            if (b1.y == b2.y)
            {
                //ngang
                return BodyPartType.BodyS;
            }

            return BodyPartType.Body;
        }
    }

    public enum Direction
    {
        None,
        Up,
        Down,
        Left,
        Right
    }
    
    public enum BodyPartType
    {
        Head,
        Body,
        BodyS,
        BodyLL,
        BodyLR,
        BodyLLD,
        BodyLRD,
        Tail
    }
}