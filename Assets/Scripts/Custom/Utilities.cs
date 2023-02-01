using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Custom
{
    public class Utils
    {
        
        public static  bool IsOutRange(Vector2Int gridPosition)
        {
            return (gridPosition.x < 0 || gridPosition.y < 0 || gridPosition.x >= GameConfig.MAPSIZE ||
                    gridPosition.y >= GameConfig.MAPSIZE);
        }
        
        public static Border4 GetBorderFromArray(List<Vector2Int> snakePositionParts)
        {
            Border4 border = new Border4();
            foreach (var positionPart in snakePositionParts)
            {
                if (positionPart.x > border.right)
                {
                    border.right = positionPart.x;
                }

                if (positionPart.x < border.left)
                {
                    border.left = positionPart.x;
                }

                if (positionPart.y > border.top)
                {
                    border.top = positionPart.y;
                }

                if (positionPart.y < border.bottom)
                {
                    border.bottom = positionPart.y;
                }
            }
            return border;
        }
    }

    public class Border4
    {
        public int top = 0;
        public int bottom = 0;
        public int left = 0;
        public int right = 0;
    }
}