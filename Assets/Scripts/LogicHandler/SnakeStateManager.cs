using System;
using System.Collections;
using System.Linq;
using System.Timers;
using Custom;
using DG.Tweening;
using UnityEngine;

namespace Interface
{
    public class SnakeStateManager
    {
        private LevelGrid _levelGrid;
        private Snake _snake;
        public static SnakeStateManager Instance;
        public enum HEADSTATE {none, eatBanana, eatChilli, spitfire, drop, dropCAB};
        public HEADSTATE stateHeadSnake = HEADSTATE.none;

        public void Setup(LevelGrid levelGrid, Snake snake)
        {
            _levelGrid = levelGrid;
            _snake = snake; 
            Instance = this;
        }


        public void UpdateStateSnake()
        {
            //TODO Check snakePositionParts gen snake sprite
            if (_snake.snakePositionParts.Count > 2)
            {
                for (int i = 1; i < _snake.snakePositionParts.Count - 1; i++)
                {
                    Direction directionBodyP =
                        MovementMap.GetDirection(_snake.snakePositionParts[i] - _snake.snakePositionParts[i - 1]);

                    BodyPartType type = MovementMap.GetBodyTypeByPos(i, _snake.snakePositionParts);

                    _snake.snakeBodyParts[i].SetSnakeMovePosition(_snake.snakePositionParts[i], directionBodyP, type, i);
                }

                //HEAD
                Direction directionHead = MovementMap.GetDirection(_snake.snakePositionParts[0] - _snake.snakePositionParts[1]);
                _snake.snakeBodyParts[0].SetSnakeMovePosition(_snake.snakePositionParts[0], directionHead, BodyPartType.Head, 0);

                //TAIL
                Direction directionTail = MovementMap.GetDirection(
                    _snake.snakePositionParts[_snake.snakePositionParts.Count - 1] -
                    _snake.snakePositionParts[_snake.snakePositionParts.Count - 2]);
                _snake.snakeBodyParts[_snake.snakePositionParts.Count - 1].SetSnakeMovePosition(
                    _snake.snakePositionParts[_snake.snakePositionParts.Count - 1], directionTail, BodyPartType.Tail,
                    _snake.snakePositionParts.Count - 1);
            }
            else if (_snake.snakePositionParts.Count == 2)
            {
                //Case chi dau + duoi
                Direction directionHead = MovementMap.GetDirection(_snake.snakePositionParts[0] - _snake.snakePositionParts[1]);
                _snake.snakeBodyParts[0].SetSnakeMovePosition(_snake.snakePositionParts[0], directionHead, BodyPartType.Head, 0);
                Direction directionTail = MovementMap.GetDirection(_snake.snakePositionParts[1] - _snake.snakePositionParts[0]);
                _snake.snakeBodyParts[1].SetSnakeMovePosition(_snake.snakePositionParts[1], directionTail, BodyPartType.Tail, 1);
            }
            if (_snake.state == Snake.StateSnake.Win || _snake.state == Snake.StateSnake.MeetTrap)
            {
                _snake.snakeBodyParts[0].DoHidden();
            }
        }

        public void DoFly(Vector2Int changeValue, float timeFlying)
        {
            for (int i = 0; i < _snake.snakeBodyParts.Count; i++)
            {
                var position = _snake.snakeBodyParts[i].transform.position;
                //Debug.Log("BINH DO FLYING POSITION" + position);
                _snake.snakeBodyParts[i].transform
                    .DOMove(new Vector3(position.x + changeValue.x, position.y + changeValue.y), timeFlying).SetEase(Ease.InSine);
            }
        }

        public void DoDropDie()
        {
            for (int i = 0; i < _snake.snakeBodyParts.Count; i++)
            {
                var position = _snake.snakeBodyParts[i].transform.position;
                //Debug.Log("BINH DO FLYING POSITION" + position);
                _snake.snakeBodyParts[i].DoDropping();
                _snake.snakeBodyParts[i].transform.DOMove(new Vector3(position.x, position.y - 20),
                    0.15f * (20 / new Vector2(0, 5).magnitude)).SetEase(Ease.InSine);
            }
            _snake.UpdateSnakeState(Snake.StateSnake.Dead, 2);
        }

        //Gap port khi complete game move snake y to y-1; sau do remove tail tu tu
        public void DoSnakeWinGame(Vector2Int portPosition)
        {
            Vector2Int gridPosition = portPosition + MovementMap.GetChange(Direction.Down);
            _snake.state = Snake.StateSnake.Win;
            _snake.snakePositionParts.Insert(0, gridPosition);
            if (_snake.snakePositionParts.Count >= _snake.snakeBodySize + 1)
            {
                _snake.snakePositionParts.RemoveAt(_snake.snakePositionParts.Count - 1);
            }
            UpdateStateSnake();
            Debug.Log("CHECK WIN GAN LAI");
        }
        
        public void DoSnakeMeetTrap(Vector2Int portPosition)
        {
            Vector2Int gridPosition = portPosition + MovementMap.GetChange(Direction.Down);
            _snake.state = Snake.StateSnake.MeetTrap;
            _snake.snakePositionParts.Insert(0, gridPosition);
            if (_snake.snakePositionParts.Count >= _snake.snakeBodySize + 1)
            {
                _snake.snakePositionParts.RemoveAt(_snake.snakePositionParts.Count - 1);
            }
            UpdateStateSnake();
            Debug.Log("CHECK MEET TRAP GAN LAI");
        }

        
        #region Update Sprite Emotion 
        public void SetSpriteEmotionHead(Sprite spnone, Sprite spbanana, Sprite spchillli, Sprite spSpitfire, Sprite spdrop, Sprite spdropCAB)
        {
            Debug.Log("stateHeadSnake: " + stateHeadSnake);
            switch (stateHeadSnake)
            {
                case HEADSTATE.none:
                    Snake.Instance.snakeBodyParts[0].spSnakeBody.sprite = spnone;
                    break;
                case HEADSTATE.eatBanana:
                    Snake.Instance.snakeBodyParts[0].spSnakeBody.sprite = spbanana;
                    break;
                case HEADSTATE.eatChilli:
                    Snake.Instance.snakeBodyParts[0].spSnakeBody.sprite = spchillli;
                    break;
                case HEADSTATE.spitfire:
                    Snake.Instance.snakeBodyParts[0].spSnakeBody.sprite = spSpitfire;
                    break;
                case  HEADSTATE.drop:
                    Snake.Instance.snakeBodyParts[0].spSnakeBody.sprite = spdrop;
                    break;
                case HEADSTATE.dropCAB:
                    Snake.Instance.snakeBodyParts[0].spSnakeBody.sprite = spdropCAB;
                    Debug.Log("droppp foodddddd");
                    Debug.Log(spdropCAB);
                    break;
            }
        }

        public void UpdateSnakeEmotion(Direction dir)
        {           
            Debug.Log("update emotion dir: " + dir);

            switch (dir)
            {
                default:
                case Direction.Up:
                    SetSpriteEmotionHead(GameAssets.Instance.snakeHeadSprite, GameAssets.Instance.SnakeHeadSpriteBanana,GameAssets.Instance.snakeHeadSpriteChilli, GameAssets.Instance.snakeHeadSpriteSpitFire, GameAssets.Instance.snakeHeadSpriteDrop, GameAssets.Instance.snakeHeadSpriteDBAC);
                    break;
                case Direction.Down:
                    SetSpriteEmotionHead(GameAssets.Instance.snakeHeadDownSprite, GameAssets.Instance.SnakeHeadDownSpriteBanana, GameAssets.Instance.snakeHeadDownSpriteChilli, GameAssets.Instance.snakeHeadDownSpriteSpitFire, GameAssets.Instance.snakeHeadDownSpriteDrop, GameAssets.Instance.snakeHeadDownSpriteDBAC);
                    break;
                case Direction.Left:
                    SetSpriteEmotionHead(GameAssets.Instance.snakeHeadLeftSprite, GameAssets.Instance.SnakeHeadLeftSpriteBanana, GameAssets.Instance.snakeHeadLeftSpriteChilli, GameAssets.Instance.snakeHeadLeftSpriteSpitFire, GameAssets.Instance.snakeHeadLeftSpriteDrop, GameAssets.Instance.snakeHeadLeftSpriteDBAC);
                    break;
                case Direction.Right:
                    SetSpriteEmotionHead(GameAssets.Instance.snakeHeadRightSprite, GameAssets.Instance.SnakeHeadRightSpriteBanana, GameAssets.Instance.snakeHeadRightSpriteChilli, GameAssets.Instance.snakeHeadRightSpriteSpitFire, GameAssets.Instance.snakeHeadRightSpriteDrop, GameAssets.Instance.snakeHeadRightSpriteDBAC);
                    break;
            }
        }
        #endregion
    }
}