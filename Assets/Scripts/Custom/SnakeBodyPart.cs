using System;
using Interface;
using Unity.VisualScripting;
using UnityEngine;

namespace Custom
{
    /*
     * Handles a Single Snake Body Part
     * */

    /// Add this to any GameObject then assign
    /// parameter-less functions to OnUpdate.
    public class CustomUpdate : MonoBehaviour
    {
        public event System.Action OnUpdate;

        void Update()
        {
            if (OnUpdate != null)
            {
                OnUpdate();
            }
        }
    }

    public class SnakeBodyPart : CustomUpdate
    {
        private Vector2Int snakeMovePosition;
        private int bodyCount = 2;

        public SpriteRenderer spSnakeBody;
        public static SnakeBodyPart Instance;
        private int count = 0;


        public Direction dir;

        public void Setup(int bodyIndex)
        {
            Instance = this;
            //GetComponent<SpriteRenderer>().sortingOrder = -1 - bodyIndex;
            OnUpdate += OnUpdateCustom;
            if (bodyIndex == 0)
            {
                InitSprite(GameAssets.Instance.snakeHeadSprite, bodyIndex);
            }
            else if (bodyIndex > bodyCount)
            {
                bodyCount = bodyIndex;
                InitSprite(GameAssets.Instance.snakeTailSprite, bodyIndex);
            }
            else
            {
                if (Snake.Instance.snakeBodySize == 2)
                {
                    InitSprite(GameAssets.Instance.snakeTailSprite, bodyIndex);
                }
                else
                {
                    if (bodyIndex == 2)
                    {
                        InitSprite(GameAssets.Instance.snakeTailSprite, bodyIndex);
                    }
                    else
                    {
                        InitSprite(GameAssets.Instance.snakeBodySprite, bodyIndex);
                    }
                }
            }
        }

        private void InitSprite(Sprite sp, int bodyIndex)
        {
            spSnakeBody.sprite = sp;
            spSnakeBody.sortingOrder = 50 - bodyIndex;
        }

        public void SetSprite(Sprite sp, int bodyIndex)
        {
            Snake.Instance.snakeBodyParts[bodyIndex].spSnakeBody.sprite = sp;
        }

        public void SetSpriteBodyType(BodyPartType bodyType, int index)
        {
            switch (bodyType)
            {
                default:

                case BodyPartType.Body:
                    SetSprite(GameAssets.Instance.snakeBodySprite, index);
                    break;
                case BodyPartType.BodyS:
                    SetSprite(GameAssets.Instance.snakeBodyStraightSprite, index);
                    break;
                case BodyPartType.BodyLL:
                    SetSprite(GameAssets.Instance.snakeBodyLLeftSprite, index);
                    break;
                case BodyPartType.BodyLR:
                    SetSprite(GameAssets.Instance.snakeBodyLRightSprite, index);
                    break;
                case BodyPartType.BodyLLD:
                    SetSprite(GameAssets.Instance.snakeBodyLLeftDownSprite, index);
                    break;
                case BodyPartType.BodyLRD:
                    SetSprite(GameAssets.Instance.snakeBodyLRightDownSprite, index);
                    break;
            }
        }

        public void SetSnakeMovePosition(Vector2Int snakeMovePosition, Direction direction, BodyPartType bodyType,
            int index)
        {
            this.snakeMovePosition = snakeMovePosition;

            transform.position =
                new Vector3(snakeMovePosition.x, snakeMovePosition.y);
            var pos = transform.position;

            spSnakeBody.sortingOrder = 50 - snakeMovePosition.y;

            switch (bodyType)
            {
                case BodyPartType.Head:
                    SetHeadSprite(bodyType, direction);
                    break;
                case BodyPartType.Tail:
                    SetSpriteTail(bodyType, direction, index);
                    break;
                default:
                    SetSpriteBodyType(bodyType, index);
                    break;
            }
        }

        public void SetHeadSprite(BodyPartType bodyType, Direction direction)
        {
            dir = direction;
            Debug.Log("dirr dirr: " + dir);

            SetEmotionHead(dir);
        }

        public void SetEmotionHead(Direction direction)
        {
            SnakeStateManager.Instance.UpdateSnakeEmotion(direction);
        }

        public void SetSpriteTail(BodyPartType bodyType, Direction direction, int index)
        {
            switch (direction)
            {
                case Direction.Up: // Currently going Up
                    SetSprite(GameAssets.Instance.snakeTailDownSprite, index);
                    break;

                case Direction.Down: // Currently going Down
                    SetSprite(GameAssets.Instance.snakeTailSprite, index);
                    break;

                case Direction.Left: // Currently going to the Left
                    SetSprite(GameAssets.Instance.snakeTailRightSprite, index);
                    break;

                case Direction.Right: // Currently going to the Right
                    SetSprite(GameAssets.Instance.snakeTailLeftSprite, index);
                    break;
            }

            InitDust(direction);
        }

        public void InitDust(Direction direction)
        {
            Vector2Int pos = GetGridPosition() + MovementMap.GetChange(direction);
            Vector3 posDust = new Vector3(pos.x, pos.y, 0);

            Instantiate(GameAssets.Instance.dust, transform.position, transform.rotation);
            if (count != 0)
            {
                Instantiate(GameAssets.Instance.dust, posDust, transform.rotation);
            }

            count++;
        }

        public void UpdateSnakeMovePosition(Vector2Int newPosition)
        {
            snakeMovePosition = newPosition;
        }

        public Vector2Int GetGridPosition()
        {
            return snakeMovePosition;
        }

        private void OnUpdateCustom()
        {
            if (Snake.Instance == null) return;
            if (Snake.Instance.state == Snake.StateSnake.Spicing)
            {
                var position = transform.position;
                var posX = 0;
                var posY = 0;
                switch (Snake.Instance.spicingDirection)
                {
                    case Direction.Up:
                    case Direction.Right:
                        posX = (int)Mathf.Floor(position.x);
                        posY = (int)Mathf.Floor(position.y);
                        break;
                    case Direction.Left:
                    case Direction.Down:
                        posX = (int)Mathf.Ceil(position.x);
                        posY = (int)Mathf.Ceil(position.y);
                        break;
                    case Direction.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                Vector2Int moveValue = MovementMap.GetChange(Snake.Instance.spicingDirection);
                Vector2Int newPos = new Vector2Int(posX + moveValue.x, posY + moveValue.y);
                if (LevelGrid.Instance.IsHasFoodAtPos(newPos))
                {
                    if (LevelGrid.Instance.foodInfos[newPos.x, newPos.y].TryGetComponent(out Ice ice))
                    {
                        ice.SetSpicyState(this);
                    }

                    if (LevelGrid.Instance.foodInfos[newPos.x, newPos.y].TryGetComponent(out Banana banana))
                    {
                        banana.SetSpicyState(this);
                    }

                    if (LevelGrid.Instance.foodInfos[newPos.x, newPos.y].TryGetComponent(out Spicy spicy))
                    {
                        spicy.SetSpicyState(this);
                    }
                }
            }
        }

        //Move index to last
        public void DoDropping()
        {   
            CreateDropEffect();
            spSnakeBody.sortingOrder = 50 - 500;
        }

        public void CreateDropEffect()
        {
            var position = transform.position;
            Vector3 gridPos = new Vector3(position.x, position.y, -1);
            GameObject boxMap = Instantiate(GameAssets.Instance.dropEffect, gridPos, Quaternion.identity);
            boxMap.transform.SetParent(GameManager.instance.effectNode.transform);
        }

        public void RemoveFromParent()
        {
            Destroy(gameObject);
        }

        public void DoHidden()
        {
            spSnakeBody.sortingOrder = -500;
            spSnakeBody.gameObject.SetActive(false);
        }
    }
}