using System;
using System.Collections.Generic;
using Custom;
using Mkey;
using Unity.VisualScripting;
using UnityEngine;
using Utilities;
using System.Collections;
using Object = UnityEngine.Object;

namespace Interface
{
    enum SnakeMoveType
    {
        CANTMOVE,
        EAT,
        MOVE
    }

    public class MovementManager : IMovementAction
    {
        private LevelGrid _levelGrid;
        private Snake _snake;
        private Vector2Int _gridPosition;

        static MovementManager instance;


        public void Setup(LevelGrid levelGrid, Snake snake, Vector2Int gridPosition)
        {
            instance = this;
            _levelGrid = levelGrid;
            _snake = snake;
            _gridPosition = gridPosition;
        }


        //Vi tri ran move toi de an
        public void TryToMove(Direction direction)
        {
            _gridPosition += MovementMap.GetChange(direction);
            //Move or Cant move
            if (IsAroundCanMove(_gridPosition))
            {
                _snake.snakePositionParts.Insert(0, _gridPosition);
                if (_levelGrid.IsHasFoodAtPos(_gridPosition))
                {
                    TryToEat(_gridPosition, direction);
                }
                else
                {
                    SnakeStateManager.Instance.stateHeadSnake = SnakeStateManager.HEADSTATE.none;
                    SnakeStateManager.Instance.UpdateSnakeEmotion(direction);
                }

                if (_snake.state != Snake.StateSnake.Spicing)
                {
                    if (_snake.snakePositionParts.Count >= _snake.snakeBodySize + 1)
                    {
                        _snake.snakePositionParts.RemoveAt(_snake.snakePositionParts.Count - 1);
                    }

                    _snake.UpdateSnakeBodyParts();
                    _snake.CheckStillAlive(true);
                    SoundMaster.Instance.SoundPlayByEnum(ESoundType.Move, 0, null);
                }
                //Check win
            }
            else
            {
                //GAN LAI VALUE GRIDPOSITION NEU STEP TIEP THEO K MOVE DC
                //DO BAN DAU DA +VALUE VAO GRID POSITION NEN P TRU DI
                _gridPosition -= MovementMap.GetChange(direction);
                _snake.gridMoveDirection = Direction.None;
            }

            if (!Common.isTutorialLevel)
            {
                DoCheckWinGame(_gridPosition);
            }
            else
            {
                GameManager.instance.tutorialNode.HiddenGuideController();
            }
        }

        public void DoCheckWinGame(Vector2Int gridPosition)
        {
            if (_levelGrid!=null)
            {
                if (_levelGrid.IsWinGame(gridPosition))
                {
                    _snake.DoSnakeWinGame(gridPosition);
                }

                if (_levelGrid.IsMeetTrap(gridPosition))
                {
                    _snake.DoSnakeMeetTrap(gridPosition);
                }
            }
        }

        public void TryToEat(Vector2Int gridPosition, Direction direction)
        {
            List<GameObject> listMovable = new List<GameObject>();

            bool isHas = true;
            Vector2Int cursorGridPos = gridPosition;
            while (isHas)
            {
                if (_levelGrid.IsHasFoodAtPos(cursorGridPos))
                {
                    listMovable.Add(_levelGrid.foodInfos[cursorGridPos.x, cursorGridPos.y]);
                    cursorGridPos += MovementMap.GetChange(direction);
                }
                else
                {
                    isHas = false;
                }
            }

            //002301
            //cursorGridPos = vi tri cua so 0 thu 3
            bool canEatNow = !_levelGrid.IsValid(cursorGridPos);
            if (canEatNow)
            {
                int foodType = _levelGrid.GetTypeFoodAtPos(gridPosition);
                if (foodType == (int)MapObject.Banana)
                {
                    DoEatBanana(direction);
                }
                else if (foodType == (int)MapObject.Spicy)
                {
                    if (_snake.snakePositionParts.Count >= _snake.snakeBodySize + 1)
                    {
                        _snake.snakePositionParts.RemoveAt(_snake.snakePositionParts.Count - 1);
                    }

                    _snake.UpdateSnakeBodyParts();
                    DoEatSpicy(direction);
                }
                else if (foodType == (int)MapObject.Ice)
                {
                    _snake.snakePositionParts.RemoveAt(0);
                    _gridPosition -= MovementMap.GetChange(direction);
                }
            }
            else
            {
                DoMoveFoodList(listMovable, direction);
            }
        }

        public void DoMoveFoodList(List<GameObject> listMovable, Direction direction)
        {
            //Di chuyển food khi vẫn còn trống
            for (int i = listMovable.Count - 1; i >= 0; i--)
            {
                if (listMovable[i].TryGetComponent(out Banana banana))
                {
                    banana.Move(direction);
                }
                else if (listMovable[i].TryGetComponent(out Spicy spicy))
                {
                    spicy.Move(direction);
                }
                else if (listMovable[i].TryGetComponent(out Ice ice))
                {
                    ice.Move(direction);
                }
            }
        }
        
        public void DoEatBanana(Direction direction)
        {       
            SoundMaster.Instance.SoundPlayByEnum(ESoundType.EatBanana, 0, null);
            CustomEventManager.Instance.EatBanana();
            _snake.snakeBodySize++;
            _snake.CreateSnakeBodyPart();
            _snake.CreateEatEffect(direction,new Vector2(_gridPosition.x, _gridPosition.y));
            _levelGrid.UpdateNumberFood(false);

            //SoundManager.PlaySound(SoundManager.Sound.SnakeEat);
            Object.Destroy(_levelGrid.foodInfos[_gridPosition.x, _gridPosition.y]);
            _levelGrid.foodInfos[_gridPosition.x, _gridPosition.y] = null;
            Score.AddScore();
            SoundMaster.Instance.SoundPlayByEnum(ESoundType.DuoiDai, 0, null);
        }

        public void DoEatSpicy(Direction direction)
        {
            // CustomEventManager.Instance.SpitFire();
            CustomEventManager.Instance.Fire();
            if (!Common.isTutorialLevel)
            {
                _snake.CreateEatEffect(direction,new Vector2(_gridPosition.x, _gridPosition.y));

                Direction flyDirection = MovementMap.GetFlyDirection(direction);

                // SoundManager.PlaySound(SoundManager.Sound.SnakeEat);
                Object.Destroy(_levelGrid.foodInfos[_gridPosition.x, _gridPosition.y]);
                SoundMaster.Instance.SoundPlayByEnum(ESoundType.EatSpicy, 0, null);
                _levelGrid.foodInfos[_gridPosition.x, _gridPosition.y] = null;
                _levelGrid.UpdateNumberFood(false);
                _snake.state = Snake.StateSnake.Spicing;


                _snake.spicingDirection = flyDirection;
                int stepToStone = 30;
                for (int i = 0; i < _snake.snakeBodyParts.Count; i++)
                {
                    Vector2Int checkPos = _snake.snakeBodyParts[i].GetGridPosition();
                    //Check alive if move outside border
                    int numberStepToStone = _levelGrid.GetNumberStepToStone(flyDirection, checkPos);
                    if (numberStepToStone < stepToStone)
                    {
                        stepToStone = numberStepToStone;
                    }
                }

                Debug.Log("STEP TO  STONE" + stepToStone);

                Vector2Int flyVector = MovementMap.GetChangeReserve(direction) * stepToStone;
                _snake.DoFlying(flyDirection, flyVector);
                SnakeStateManager.Instance.stateHeadSnake = SnakeStateManager.HEADSTATE.none;
            }
            else
            {
                Object.Destroy(_levelGrid.foodInfos[_gridPosition.x, _gridPosition.y]);
                SoundMaster.Instance.SoundPlayByEnum(ESoundType.EatSpicy, 0, null);
                _snake.CreateEatEffect(direction,new Vector2(_gridPosition.x, _gridPosition.y));
                _levelGrid.foodInfos[_gridPosition.x, _gridPosition.y] = null;
                _snake.state = Snake.StateSnake.Spicing;
                GameManager.instance.tutorialNode.StartRunTut();
            }
        }

        //Check break ice
        public void CheckBreakIce()
        {
            //TODO change sping node to ice when need for remove ice by spicing
            Vector2Int checkPos = _gridPosition + MovementMap.GetChangeReserve(Snake.Instance.spicingDirection);
            if (_levelGrid.IsIce(checkPos))
            {
                if (GameManager.instance.foodNode.TryGetComponent(out SpicingNode node))
                {
                    GameObject iceNode = node.FindGameObjectAtPos(checkPos);
                    Object.Destroy(iceNode);
                    Vector3 gridPos = new Vector3(checkPos.x , checkPos.y, -1);
                    GameObject boxMap = Object.Instantiate(GameAssets.Instance.iceBurnEffect, gridPos, Quaternion.identity);
                    boxMap.transform.SetParent(GameManager.instance.effectNode.transform);
                }
            }
        }

        public void CheckBreakWood()
        {
            Vector2Int checkPos = _gridPosition + MovementMap.GetChangeReserve(Snake.Instance.spicingDirection);
            if (_levelGrid.IsWood(checkPos))
            {
                if (GameManager.instance.obstacleNode.TryGetComponent(out SpicingNode node))
                {
                    GameObject woodNode = node.FindGameObjectAtPos(checkPos);
                    Object.Destroy(woodNode);
                    _levelGrid.mapInfos[checkPos.x, checkPos.y] = 1;
                }
            }
        }

        private bool IsAroundCanMove(Vector2Int newPos)
        {
            return _levelGrid.IsValid(newPos);
        }

        public void UpdateHeadPosition(Vector2Int newPos)
        {
            _gridPosition = newPos;
        }
    }
}