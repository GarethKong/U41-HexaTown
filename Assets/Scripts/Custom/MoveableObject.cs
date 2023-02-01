using System;
using DG.Tweening;
using Interface;
using Mkey;
using UnityEngine;

namespace Custom
{
    public enum FoodState
    {
        Idle,
        MovingBySnakePart,
        MovingByFood,
    }

    public abstract class MovableObject : MonoBehaviour
    {
        public SpriteRenderer sp;
        protected bool isSpicying;
        protected SnakeBodyPart _snakeBodyPart;
        protected Banana _anotherBanana;
        protected Ice _anotherIce;
        protected Spicy _anotherSpicy;
        protected FoodState foodState;
        protected Vector2Int gridPosition;
        protected LevelGrid _levelGrid;
        protected Direction gridMoveDirection;

        public virtual void Move(Direction direction)
        {
            Vector2Int moveValue = MovementMap.GetChange(direction);
            Vector2Int newPos = new Vector2Int(gridPosition.x + moveValue.x, gridPosition.y + moveValue.y);

            UpdateFoodToNewPos(gridPosition, newPos);
        }

        public  void UpdateFoodToNewPos(Vector2Int oldPos, Vector2Int newPos)
        {
            if (oldPos == newPos) return;
            GameObject temp = _levelGrid.foodInfos[oldPos.x, oldPos.y];
            _levelGrid.foodInfos[newPos.x, newPos.y] = temp;
            _levelGrid.foodInfos[gridPosition.x, gridPosition.y] = null;
            gridPosition = newPos;
            transform.position = new Vector3(newPos.x, newPos.y);
            sp.sortingOrder = 50 - newPos.y;
            SoundMaster.Instance.SoundPlayByEnum(ESoundType.FoodMove, 0, null);
            //Check food drop or not
            if (!_levelGrid.IsPosAtOnBoard(newPos))
                DoDropping(newPos);
        }

        
        public  GameObject GetFoodByPosition(Direction direction)
        {
            Vector2Int directionValue = MovementMap.GetChange(direction);
            Vector2Int checkPos = new Vector2Int(GetGridPosition().x + directionValue.x,
                GetGridPosition().y + directionValue.x);
            return LevelGrid.Instance.foodInfos[checkPos.x, checkPos.y];
        }

        //Chuối di chuyển chuối
        public void SetSpicyStateByFood(MovableObject moveAble)
        {
            if (_anotherBanana == null && _snakeBodyPart == null && _anotherSpicy == null&& _anotherIce == null)
            {
                isSpicying = true;
                if (moveAble.TryGetComponent(out Banana banana))
                {
                    _anotherBanana = banana;
                }
                else if (moveAble.TryGetComponent(out Spicy spicy))
                {
                    _anotherSpicy = spicy;
                }  else if (moveAble.TryGetComponent(out Ice ice))
                {
                    _anotherIce = ice;
                }

                foodState = FoodState.MovingByFood;
            }
        }

        public void SetSpicyState(SnakeBodyPart snakeBodyPart)
        {
            if (_snakeBodyPart == null && _anotherBanana == null && _anotherSpicy == null&& _anotherIce == null)
            {
                isSpicying = true;
                _snakeBodyPart = snakeBodyPart;
                foodState = FoodState.MovingBySnakePart;
            }
        }

        //Move index to last
        public virtual void DoDropping(Vector2Int newPos)
        {
            sp.sortingOrder = -500;
            CreateDropEffect();
            transform.DOMove(new Vector3(transform.position.x, transform.position.y - 20),
                0.15f * (20 / new Vector2(0, 5).magnitude)).SetEase(Ease.InSine);
            _levelGrid.foodInfos[newPos.x, newPos.y] = null;
        }
        
        
        public void CompleteSpicing()
            {
                isSpicying = false;
                _snakeBodyPart = null;
                _anotherBanana = null;
                _anotherSpicy = null;
                _anotherIce = null;
                var position = transform.position;
                Vector2Int newPos = new Vector2Int((int)Math.Round(position.x), (int)Math.Round(position.y));
                gridPosition = newPos;
                //Update food array, remove food at old pos, update to new pos
                //UpdateFoodToNewPos(gridPosition,newPos);
            }

        void UpdatePosition()
        {
            if (!isSpicying) return;
            if (Snake.Instance.state == Snake.StateSnake.Spicing)
            {
                Vector2Int moveValue = MovementMap.GetChange(Snake.Instance.spicingDirection);
                switch (foodState)
                {
                    case FoodState.MovingBySnakePart:
                        var snakePosition = _snakeBodyPart.transform.position;
                        transform.position = new Vector3(snakePosition.x + moveValue.x, snakePosition.y + moveValue.y, 0);
                        break;
                    case FoodState.MovingByFood:
                        Vector3 anotherFoodPosition;
                        if (_anotherBanana != null)
                        {
                            anotherFoodPosition  = _anotherBanana.transform.position;
                        }else if (_anotherIce != null)
                        {
                            anotherFoodPosition  = _anotherIce.transform.position;
                        }
                        else
                        {
                            anotherFoodPosition= _anotherSpicy.transform.position;
                        }
                        Vector3 newPos =  new Vector3(anotherFoodPosition.x + moveValue.x, anotherFoodPosition.y + moveValue.y, 0);
                        //transform.position = Vector3.MoveTowards(transform.position, newPos, 0.03f);
                        transform.position = Vector3.Lerp(transform.position, newPos, 1f);
                        //transform.position = new Vector3(anotherFoodPosition.x + moveValue.x, anotherFoodPosition.y + moveValue.y, 0);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                //Check moving another food
                CheckMovingAnotherFood();
            }
            else
            {
                CompleteSpicing();
            }
         }
            
            //
            // void Update()
            // {
            //     UpdatePosition();
            // }

            private void LateUpdate()
            {
                UpdatePosition();
            }
            
            private void CheckMovingAnotherFood()
            {
                var position = transform.position;
                var posX = (int)Mathf.Round(position.x);
                var posY =(int)Mathf.Round(position.y);
                Vector2Int moveValue = MovementMap.GetChange(Snake.Instance.spicingDirection);
                Vector2Int checkPos = new Vector2Int(posX + moveValue.x, posY + moveValue.y);
                if (_levelGrid.IsHasFoodAtPos(checkPos))
                {
                    if (LevelGrid.Instance.foodInfos[checkPos.x,checkPos.y].TryGetComponent(out Banana banana))
                    {
                        banana.SetSpicyStateByFood(this);
                    }
                    if (LevelGrid.Instance.foodInfos[checkPos.x,checkPos.y].TryGetComponent(out Spicy spicy))
                    {
                        spicy.SetSpicyStateByFood(this);
                    }
                    if (LevelGrid.Instance.foodInfos[checkPos.x,checkPos.y].TryGetComponent(out Ice ice))
                    {
                        ice.SetSpicyStateByFood(this);
                    }
                }
            }
            
              
            public void CreateDropEffect()
            {
                var position = transform.position;
                Vector3 gridPos = new Vector3(position.x, position.y, -1);
                GameObject boxMap = Instantiate(GameAssets.Instance.dropEffect, gridPos, Quaternion.identity);
                boxMap.transform.SetParent(GameManager.instance.effectNode.transform);
            }
            
            public Vector2Int GetGridPosition()
            {
                return gridPosition;
            }
    }
}