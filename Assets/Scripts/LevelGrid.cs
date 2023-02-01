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
using Custom;
using Interface;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using Utilities;
using Object = UnityEngine.Object;

enum MapObject
{
    None,
    Background,
    Banana,
    Spicy,
    Stone,
    Ice,
    Wood,
    Trap,
    Ghost,
    Port
};

public class LevelGrid : MonoBehaviour
{
    private int width;
    private int height;
    public Snake snake;

    public int[,] mapInfos = new int[GameConfig.MAPSIZE, GameConfig.MAPSIZE];
    public GameObject[,] foodInfos = new GameObject[GameConfig.MAPSIZE, GameConfig.MAPSIZE];

    private int headValue = 6;
    private int tailValue;
    public int numberFoodOnBoard;
    public static LevelGrid Instance;
    public Port _port;
    public int _currentLevel;

    //public static LevelGrid Instance;

    public void StartNewGame(int currentLevel)
    {
        Instance = this;
        numberFoodOnBoard = 0;
        ClearBoard();
        //loadMapText(GameAssets.Instance.levelList[currentLevel].text);
        //TEST
        loadMapText(GameAssets.Instance.levelList[currentLevel].text);
        SpawnMap(this);
        width = GameConfig.MAPSIZE;
        height = GameConfig.MAPSIZE;
        _currentLevel = currentLevel;
    }

    public void ClearBoard()
    {
        foreach (Transform food in GameManager.instance.mapNode.transform)
        {
            Destroy(food.gameObject);
        }

        foreach (Transform food in GameManager.instance.foodNode.transform)
        {
            Destroy(food.gameObject);
        }

        foreach (Transform obstacle in GameManager.instance.obstacleNode.transform)
        {
            Destroy(obstacle.gameObject);
        }

        foreach (Transform snake in GameManager.instance.snakeNode.transform)
        {
            Destroy(snake.gameObject);
        }
        
        foreach (Transform transform in  GameManager.instance.effectNode.transform) {
            Destroy(transform.gameObject);
        }
    }

    void loadMapText(String mapText)
    {
        int indexRow = 0;
        int maxValue = headValue;

        var lines = mapText.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        for (int row = GameConfig.MAPSIZE - 1; row >= 0; row--)
        {
            var word = lines[row];

            for (int column = 0; column < GameConfig.MAPSIZE; column++)
            {
                char character = word.ToCharArray()[column];

                if (character > 64)
                {
                    mapInfos[column, GameConfig.MAPSIZE - 1 - row] = character;
                }
                else
                {
                    mapInfos[column, GameConfig.MAPSIZE - 1 - row] = Int32.Parse(word[column] + "");
                }

                foodInfos[column, row] = null;

                if (mapInfos[column, GameConfig.MAPSIZE - 1 - row] > maxValue)
                {
                    tailValue = mapInfos[column, GameConfig.MAPSIZE - 1 - row];
                    maxValue = mapInfos[column, GameConfig.MAPSIZE - 1 - row];
                }
            }

            indexRow++;
        }
    }

    private void SpawnStone(int i, int j)
    {
        GameObject boxMap = Instantiate(GameAssets.Instance.pbStone, new Vector3(i, j), Quaternion.identity);
        boxMap.transform.parent = GameManager.instance.obstacleNode.transform;
        if (boxMap.TryGetComponent(out Stone stone))
        {
            stone.Setup(i, j);
        }
    }

    private void SpawnIce(int i, int j)
    {
        GameObject boxMap = Instantiate(GameAssets.Instance.pbIce, new Vector3(i, j), Quaternion.identity);
        boxMap.transform.parent = GameManager.instance.foodNode.transform;
        if (boxMap.TryGetComponent(out Ice ice))
        {
            ice.Setup(i, j, this);
        }

        foodInfos[i, j] = boxMap;
    }

    private void SpawnWood(int i, int j)
    {
        GameObject boxMap = Instantiate(GameAssets.Instance.pbWood, new Vector3(i, j), Quaternion.identity);
        boxMap.transform.parent = GameManager.instance.obstacleNode.transform;
        if (boxMap.TryGetComponent(out Wood wood))
        {
            wood.Setup(i, j);
        }
    }

    private void SpawnTrap(int i, int j)
    {
        GameObject boxMap = Instantiate(GameAssets.Instance.pbTrap, new Vector3(i, j), Quaternion.identity);
        boxMap.transform.parent = GameManager.instance.obstacleNode.transform;
        if (boxMap.TryGetComponent(out Trap trap))
        {
            trap.Setup(i, j);
        }
    }


    private void SpawnPort(int i, int j)
    {
        GameObject boxMap = Instantiate(GameAssets.Instance.pbPort, new Vector3(i, j), Quaternion.identity);
        boxMap.transform.parent = GameManager.instance.obstacleNode.transform;
        if (boxMap.TryGetComponent(out Port port))
        {
            port.Setup(i, j, this);
            _port = port;
        }
    }

    private void SpawnSpicy(int i, int j)
    {
        GameObject boxMap = Instantiate(GameAssets.Instance.pbSpicy, new Vector3(i, j), Quaternion.identity);
        boxMap.transform.parent = GameManager.instance.foodNode.transform;
        if (boxMap.TryGetComponent(out Spicy spicy))
        {
            spicy.Setup(i, j, this);
        }

        foodInfos[i, j] = boxMap;
        UpdateNumberFood(true);
    }

    private void SpawnBanana(int i, int j)
    {
        GameObject boxMap = Instantiate(GameAssets.Instance.pbBanana, new Vector3(i, j), Quaternion.identity);
        boxMap.transform.parent = GameManager.instance.foodNode.transform;
        if (boxMap.TryGetComponent(out Banana banana))
        {
            banana.Setup(i, j, this);
        }

        foodInfos[i, j] = boxMap;
        UpdateNumberFood(true);
    }

    private void SpawnSnake(List<Vector2Int> listSnake, LevelGrid levelGrid)
    {
        GameObject boxMap = Instantiate(GameAssets.Instance.pbSnake, new Vector3(0, 0), Quaternion.identity);
        boxMap.transform.parent = GameManager.instance.snakeNode.transform;
        if (boxMap.TryGetComponent(out Snake _snake))
        {
            snake = _snake;
            snake.Setup(listSnake, levelGrid);
        }

        if (Common.isTutorialLevel)
        {
            GameManager.instance.tutorialNode.gameObject.SetActive(true);
            if (GameManager.instance.tutorialNode.TryGetComponent(out TutorialManager tutorialManager))
            {
                tutorialManager.SetupSnake(snake);
            }
        }
    }

    private void SpawnMap(LevelGrid levelGrid)
    {
        List<SnakePosObject> listPositionSnake = new List<SnakePosObject>();

        for (int i = 0; i < GameConfig.MAPSIZE; i++)
        {
            for (int j = 0; j < GameConfig.MAPSIZE; j++)
            {
                if (mapInfos[i, j] != (int)MapObject.None)
                {
                    GameObject boxMap = new GameObject("BoxMap", typeof(SpriteRenderer));
                    int index = ((i + j) % 2 == 0) ? 1 : 0;
                    boxMap.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.boxSprite[index];
                    boxMap.GetComponent<SpriteRenderer>().sortingOrder = -j;
                    boxMap.transform.position = new Vector3(i, j);
                    boxMap.transform.parent = GameManager.instance.mapNode.transform;
                }

                if (mapInfos[i, j] == (int)MapObject.Banana)
                {
                    SpawnBanana(i, j);
                }

                if (mapInfos[i, j] == (int)MapObject.Stone)
                {
                    SpawnStone(i, j);
                }

                if (mapInfos[i, j] == (int)MapObject.Ice)
                {
                    SpawnIce(i, j);
                }

                if (mapInfos[i, j] == (int)MapObject.Wood)
                {
                    SpawnWood(i, j);
                }

                if (mapInfos[i, j] == (int)MapObject.Trap)
                {
                    SpawnTrap(i, j);
                }

                if (mapInfos[i, j] == (int)MapObject.Spicy)
                {
                    SpawnSpicy(i, j);
                }

                if (mapInfos[i, j] == (int)MapObject.Port)
                {
                    SpawnPort(i, j);
                }

                if (mapInfos[i, j] >= 65)
                {
                    SnakePosObject snakePosObject = new SnakePosObject(new Vector2Int(i, j), mapInfos[i, j]);
                    listPositionSnake.Add(snakePosObject);
                }
            }
        }

        listPositionSnake.Sort(delegate(SnakePosObject x, SnakePosObject y)
        {
            if (x.gridValue == 0 && y.gridValue == 0) return 0;
            if (x.gridValue == 0) return -1;
            if (y.gridValue == 0) return 1;
            return x.gridValue.CompareTo(y.gridValue);
        });

        // Debug.Log("List snake" + listPositionSnake.ToString());
        List<Vector2Int> afterSort = new List<Vector2Int>();
        foreach (var positionSnake in listPositionSnake)
        {
            afterSort.Add(positionSnake.gridPos);
        }

        /*foreach (var VARIABLE in afterSort)
        {
            Debug.Log(VARIABLE);
        }*/

        SpawnSnake(afterSort, levelGrid);

        SmoothCameraSize.Instance.StartZoom(this);
    }

    public bool IsValid(Vector2Int gridPosition)
    {
        bool isHasSnakePos = false;
        for (int i = 0; i < snake.snakePositionParts.Count - 1; i++)
        {
            if (snake.snakePositionParts[i].x == gridPosition.x && snake.snakePositionParts[i].y == gridPosition.y)
            {
                isHasSnakePos = true;
                break;
            }
        }

        return !IsStone(gridPosition) && !IsWood(gridPosition) && !isHasSnakePos;
    }

    public bool IsStone(Vector2Int gridPosition)
    {
        if (Utils.IsOutRange(gridPosition))
        {
            return false;
        }

        return mapInfos[gridPosition.x, gridPosition.y] == (int)MapObject.Stone;
    }

    public bool IsWood(Vector2Int gridPosition)
    {
        if (Utils.IsOutRange(gridPosition))
        {
            return false;
        }

        return mapInfos[gridPosition.x, gridPosition.y] == (int)MapObject.Wood;
    }

    public bool IsIce(Vector2Int gridPosition)
    {
        if (Utils.IsOutRange(gridPosition))
        {
            return false;
        }

        var isIce = false;
        if (foodInfos[gridPosition.x, gridPosition.y] == null) return false;
        if (foodInfos[gridPosition.x, gridPosition.y].TryGetComponent(out Ice ice))
        {
            isIce = true;
        }

        return isIce;
    }

    public bool IsHasFoodAtPos(Vector2Int gridPosition)
    {
        if (Utils.IsOutRange(gridPosition)) return false;
        return foodInfos[gridPosition.x, gridPosition.y] != null;
    }

    public bool IsPosAtOnBoard(Vector2Int gridPosition)
    {
        if (Utils.IsOutRange(gridPosition)) return false;
        return mapInfos[gridPosition.x, gridPosition.y] != (int)MapObject.None;
    }

    public int GetTypeFoodAtPos(Vector2Int gridPosition)
    {
        if (Utils.IsOutRange(gridPosition)) return -1;
        if (foodInfos[gridPosition.x, gridPosition.y] == null) return -1;
        if (foodInfos[gridPosition.x, gridPosition.y].TryGetComponent(out Banana banana))
        {
            return (int)MapObject.Banana;
        }

        if (foodInfos[gridPosition.x, gridPosition.y].TryGetComponent(out Spicy spicty))
        {
            return (int)MapObject.Spicy;
        }

        if (foodInfos[gridPosition.x, gridPosition.y].TryGetComponent(out Ice ice))
        {
            return (int)MapObject.Ice;
        }

        return -1;
    }

    public bool IsWinGame(Vector2Int gridPosition)
    {
        if (Utils.IsOutRange(gridPosition)) return false;
        if (_port == null) return false;
        return (_port.GetGridPosition() == gridPosition && _port.isCanPass);
    }

    public bool IsMeetTrap(Vector2Int gridPosition)
    {
        if (Utils.IsOutRange(gridPosition)) return false;
        return (mapInfos[gridPosition.x, gridPosition.y] == (int)MapObject.Trap);
    }


    public void UpdateNumberFood(bool isAdd)
    {
        if (isAdd)
        {
            numberFoodOnBoard++;
        }
        else
        {
            numberFoodOnBoard--;
        }

        if (numberFoodOnBoard == 0)
        {
            _port.OpenDoor();
        }
    }

    //Tìm vị trí stone theo hướng và điểm bắt đầu
    //Move hết 20x20 board, nếu có stone đúng hướng đang di chuyển trả về số bước k return -1(Không có đá)
    public int GetNumberStepToStone(Direction direction, Vector2Int gridPosition)
    {
        int step = 0;
        int cursorRange = 1;
        Vector2Int valueCheck = MovementMap.GetChange(direction);
        bool check = true;
        bool isHasStone = false;
        while (check)
        {
            Vector2Int posCheck = gridPosition + valueCheck * cursorRange;
            if (posCheck.x is < 0 or >= GameConfig.MAPSIZE || posCheck.y is < 0 or >= GameConfig.MAPSIZE)
            {
                check = false;
            }
            else
            {
                if (!IsStone(posCheck) && !IsWood(posCheck))
                {
                    if (!IsHasFoodAtPos(posCheck))
                    {
                        step += 1;
                    }
                }
                else
                {
                    check = false;
                    isHasStone = true;
                }
            }

            cursorRange += 1;
        }

        return (isHasStone) ? step : 30;
    }

    public int GetNumberFoodAtRange(Direction direction, Vector2Int gridPosition, int step)
    {
        while (step > 0)
        {
            Vector2Int valueCheck = MovementMap.GetChange(direction);
            bool check = true;
            while (check)
            {
                Vector2Int posCheck = gridPosition + valueCheck * (step + 1);
                if (gridPosition.x is <= 0 or > GameConfig.MAPSIZE || gridPosition.y is <= 0 or > GameConfig.MAPSIZE)
                {
                    check = false;
                }

                if (!IsStone(posCheck))
                {
                    step += 1;
                }
            }
        }

        return step;
    }

    private class SnakePosObject
    {
        public Vector2Int gridPos;
        public int gridValue = 0;

        public SnakePosObject(Vector2Int gridPos, int gridValue)
        {
            this.gridPos = gridPos;
            this.gridValue = gridValue;
        }
    }
}