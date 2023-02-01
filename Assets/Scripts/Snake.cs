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
using System.Linq;
using UnityEngine;
using Custom;
using ExaGames.Common;
using Interface;
using Mkey;
using Utilities;

public class Snake : MonoBehaviour, ISnakeAction
{
    public enum StateSnake
    {
        Alive,
        Spicing,
        Dropping,
        Dead,
        MeetTrap,
        Win
    }

    public StateSnake state;
    public Direction gridMoveDirection;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    public static Snake Instance;
    public int snakeBodySize;


    private MovementManager _movementManager;
    private SnakeStateManager _snakeStateManager;

    public List<SnakeBodyPart> snakeBodyParts;
    public List<Vector2Int> snakePositionParts;

    private float nextActionTime = 0f;
    public float period = 1f;

    public void Setup(List<Vector2Int> snakePosList, LevelGrid levelGrid)
    {
        Instance = this;
        SwipeManager.OnSwipeDetected += OnSwipeDetected;
        this.levelGrid = levelGrid;
        snakePositionParts = new List<Vector2Int>();
        _movementManager = new MovementManager();
        _movementManager.Setup(this.levelGrid, this, snakePosList[0]);
        _snakeStateManager = new SnakeStateManager();
        _snakeStateManager.Setup(this.levelGrid, this);
        snakeBodySize = snakePosList.Count;


        for (int i = 0; i < snakePosList.Count; i++)
        {
            Vector2Int current_snake_tile = snakePosList[i];
            snakePositionParts.Add(current_snake_tile);
            CreateSnakeBodyPart();
        }

        transform.position = new Vector3(snakePosList[0].x, snakePosList[0].y, 0);
        UpdateSnakeBodyParts();
        Vector2Int current = snakePosList[1];
        Vector2Int nextMove = snakePosList[0];
        if (current.y == nextMove.y)
        {
            gridMoveDirection = nextMove.x > current.x ? Direction.Right : Direction.Left;
        }
        else
        {
            gridMoveDirection = nextMove.y > current.y ? Direction.Up : Direction.Down;
        }
    }

    private void Awake()
    {
        gridMoveTimerMax = .2f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = Direction.Right;

        snakeBodySize = 0;

        snakeBodyParts = new List<SnakeBodyPart>();

        state = StateSnake.Alive;
    }

    private void Start()
    {
        CustomEventManager.Instance.OnFoodDrop += FoodDropHandle;
        CustomEventManager.Instance.OnSpitFire += SpitFireHandle;
        CustomEventManager.Instance.OnFire += FireHandle;
        CustomEventManager.Instance.OnEatBanana += EatBananaHandle;
    }

    private void OnDestroy()
    {
        CustomEventManager.Instance.OnFoodDrop -= FoodDropHandle;
        CustomEventManager.Instance.OnSpitFire -= SpitFireHandle;
        CustomEventManager.Instance.OnFire -= FireHandle;
        CustomEventManager.Instance.OnEatBanana -= EatBananaHandle;
        SwipeManager.OnSwipeDetected -= OnSwipeDetected;
    }

    private void SpitFireHandle(Direction direction)
    {
        SnakeStateManager.Instance.stateHeadSnake = SnakeStateManager.HEADSTATE.spitfire;
        SnakeStateManager.Instance.UpdateSnakeEmotion(gridMoveDirection);
        StartCoroutine(FireSpitFire(direction));
    }

    public void FireHandle()
    {
        SnakeStateManager.Instance.stateHeadSnake = SnakeStateManager.HEADSTATE.eatChilli;
        SnakeStateManager.Instance.UpdateSnakeEmotion(gridMoveDirection);
    }

    private void EatBananaHandle()
    {
        SnakeStateManager.Instance.stateHeadSnake = SnakeStateManager.HEADSTATE.eatBanana;
        SnakeStateManager.Instance.UpdateSnakeEmotion(gridMoveDirection);
    }

    private void FoodDropHandle()
    {
        SnakeStateManager.Instance.stateHeadSnake = SnakeStateManager.HEADSTATE.dropCAB;
        SnakeStateManager.Instance.UpdateSnakeEmotion(gridMoveDirection);
        StartCoroutine(EmotionDropFood());
    }

    public void ResetGame()
    {
        snakeBodyParts.Clear();
        snakePositionParts.Clear();
        StopAllCoroutines();
        state = StateSnake.Alive;
    }

    private bool alreadyWin = false;
    private bool alreadyMeetTrap = false;

    private void Update()
    {
        switch (state)
        {
            case StateSnake.Spicing:
                GameManager.instance.stateWorm.text = "Spicing";
                break;
            case StateSnake.Alive:
                GameManager.instance.stateWorm.text = "Alive";
                HandleInput();
                break;
            case StateSnake.Dead:
                GameManager.instance.stateWorm.text = "Dead";
                //Show game over dialog
                break;
            case StateSnake.Dropping:
                GameManager.instance.stateWorm.text = "Dropping";
                break;
            case StateSnake.Win:
                GameManager.instance.stateWorm.text = "Winning";
                if (!alreadyWin && !Common.isTutorialLevel)
                {
                    alreadyWin = true;
                    InvokeRepeating("RutDuoi", 0f, 0.1f);
                }

                break;
            case StateSnake.MeetTrap:
                GameManager.instance.stateWorm.text = "Meet trap";
                if (!alreadyMeetTrap)
                {
                    alreadyMeetTrap = true;
                    InvokeRepeating("MeetTrap", 0f, 0.085f);
                }

                break;
        }
    }

    void OnSwipeDetected(Swipe direction, Vector2 swipeVelocity)
    {
        if (state != StateSnake.Alive) return;
        if (levelGrid == null) return;
        switch (direction)
        {
            case Swipe.Up:
                UP();
                break;
            case Swipe.Down:
                Down();
                break;
            case Swipe.Left:
                Left();
                break;
            case Swipe.Right:
                Right();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    void UP()
    {
        if (gridMoveDirection != Direction.Down)
        {
            gridMoveDirection = Direction.Up;
            HandleGridMovement();
        }
    }

    void Down()
    {
        if (gridMoveDirection != Direction.Up)
        {
            gridMoveDirection = Direction.Down;
            HandleGridMovement();
        }
    }

    void Left()
    {
        if (gridMoveDirection != Direction.Right)
        {
            gridMoveDirection = Direction.Left;
            HandleGridMovement();
        }
    }

    void Right()
    {
        if (gridMoveDirection != Direction.Left)
        {
            gridMoveDirection = Direction.Right;
            HandleGridMovement();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            HandleButtonInput(Direction.Up);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            HandleButtonInput(Direction.Down);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            HandleButtonInput(Direction.Left);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            HandleButtonInput(Direction.Right);
        }
    }

    public void HandleButtonInput(Direction direction)
    {
        if (state != StateSnake.Alive) return;
        switch (direction)
        {
            case Direction.Up:
                UP();
                break;
            case Direction.Down:
                Down();
                break;
            case Direction.Left:
                Left();
                break;
            case Direction.Right:
                Right();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    private void HandleGridMovement()
    {
        _movementManager.TryToMove(gridMoveDirection);
    }

    public void CreateSnakeBodyPart()
    {
        GameObject boxMap = Instantiate(GameAssets.Instance.pbBodyParts, new Vector3(0, 0), Quaternion.identity);
        boxMap.transform.parent = GameManager.instance.snakeNode.transform;

        if (boxMap.TryGetComponent(out SnakeBodyPart snakeBodyPart))
        {
            snakeBodyPart.Setup(snakeBodyParts.Count);
            snakeBodyParts.Add(snakeBodyPart);
        }
    }

    public void CreateEatEffect(Direction direction, Vector2 _gridPosition)
    {
        Vector2 half = new Vector2(0.5f, 0.5f);
        Vector2 valueChange = half * MovementMap.GetChange(direction);
        Vector3 gridPos = new Vector3(_gridPosition.x + valueChange.x, _gridPosition.y + valueChange.y, -1);
        GameObject boxMap = Instantiate(GameAssets.Instance.eatEffectStar, gridPos, Quaternion.identity);
        boxMap.transform.SetParent(GameManager.instance.effectNode.transform);
    }

    public void UpdateSnakeBodyParts()
    {
        _snakeStateManager.UpdateStateSnake();
    }

    public Direction spicingDirection;

    public void DoFlying(Direction direction, Vector2Int changeValue)
    {
        //CustomEventManager.Instance.SpitFire(direction);
        StartCoroutine(ExecuteFlyingSnake(direction, changeValue));
    }

    IEnumerator ExecuteFlyingSnake(Direction direction, Vector2Int changeValue)
    {
        yield return new WaitForSeconds(0.5f);
        CustomEventManager.Instance.SpitFire(direction);
        yield return new WaitForSeconds(0.5f);
        SoundMaster.Instance.SoundPlayByEnum(ESoundType.Spicing, 0, null);
        float timeFlying = 0.2f * MathF.Round(changeValue.magnitude / new Vector2(0, 2).magnitude);
        GameManager.instance.DoShakeCamera(10);

        Debug.Log("TIME FLY SPICING LA " + timeFlying);
        //Vibration.Vibrate(1000+(long)timeFlying * 1000);
        _snakeStateManager.DoFly(changeValue, timeFlying);
        _movementManager.CheckBreakIce();
        _movementManager.CheckBreakWood();
        UpdateSnakePositionParts(changeValue);

        //Delay more 1sec to complete stop
        float newTime = 1 + timeFlying;

        Debug.Log("TIME NEW FLY SPICING LA " + newTime);

        if (!Common.isTutorialLevel)
        {
            StartCoroutine(ExecuteDoAfterFly(newTime));
        }
    }

    IEnumerator ExecuteDropDie()
    {
        yield return new WaitForSeconds(0.6f);
        _snakeStateManager.DoDropDie();
    }

    IEnumerator FireSpitFire(Direction direction)
    {
        yield return new WaitForSeconds(0.1f);
        CreateFireEffect(direction);
    }

    void UpdateFoodMap()
    {
        Transform foodNode = GameManager.instance.foodNode.transform;
        int children = foodNode.childCount;
        Array.Clear(levelGrid.foodInfos, 0, levelGrid.foodInfos.Length);
        for (int i = 0; i < children; i++)
        {
            GameObject food = foodNode.GetChild(i).gameObject;
            Vector2Int pos = new Vector2Int();
            var position = food.transform.position;
            Debug.Log("POS FOOD X0" + position);
            pos.x = Convert.ToInt32(position.x);
            pos.y = Convert.ToInt32(position.y);
            if (Utils.IsOutRange(pos))
            {
                continue;
            }

            Debug.Log("POS FOOD X" + pos);
            levelGrid.foodInfos[pos.x, pos.y] = food;

            if (!levelGrid.IsPosAtOnBoard(pos))
            {
                if (food.TryGetComponent(out MovableObject move))
                {
                    move.DoDropping(pos);
                }
            }
        }
    }

    private IEnumerator ExecuteDoAfterFly(float timeFlying)
    {
        yield return new WaitForSeconds(timeFlying);
        //Update position
        CustomEventManager.Instance.StopFire();
        GameManager.instance.DoStopShakeCamera();
        StateSnake state = CheckStillAlive(true);
        SoundMaster.Instance.StopAllClip(false);
        //Update food map neu van song
        // if (state == StateSnake.Alive)
        // {

        Vector2 pos = snakeBodyParts[0].transform.position;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos, Vector2.one * 0.8f, 0);
        if (colliders.Length > 1) //Presuming the object you are testing also has a collider 0 otherwise
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                var go = colliders[i].gameObject; //T$$anonymous$$s is the game object you collided with
                go.TryGetComponent(out Trap trap);
                {
                    if (trap != null)
                    {
                        trap.OpenDoor();
                    }
                }
            }
        }

        _movementManager.DoCheckWinGame(snakePositionParts[0]);
        StartCoroutine(ExecuteUpdateFoodMap());
        //}
    }


    private IEnumerator ExecuteUpdateFoodMap()
    {
        yield return new WaitForSeconds(0.1f);
        UpdateFoodMap();
    }

    private IEnumerator EmotionDropFood()
    {
        yield return new WaitForSeconds(2f);
    }

    public StateSnake CheckStillAlive(bool isShowDrop)
    {
        //Case roi food chet ngay lap tuc
        if (state == StateSnake.Dead) return StateSnake.Dead;
        Vector2Int change = new Vector2Int();
        if (IsStillAlive(change))
        {
            state = StateSnake.Alive;
        }
        else
        {
            state = StateSnake.Dropping;
            if (isShowDrop)
            {
                //Case drop die
                SnakeStateManager.Instance.stateHeadSnake = SnakeStateManager.HEADSTATE.drop;
                SnakeStateManager.Instance.UpdateSnakeEmotion(gridMoveDirection);
                SoundMaster.Instance.SoundPlayByEnum(ESoundType.SnakeDrop, 0, null);
                StartCoroutine(ExecuteDropDie());
                CustomEventManager.Instance.FailLevel();
            }
            else
            {
                //Case spicy die
                UpdateSnakeState(StateSnake.Dead, 0);
                state = StateSnake.Dead;
                CustomEventManager.Instance.FailLevel();
            }
        }

        return state;
    }


    public bool IsStillAlive(Vector2Int change)
    {
        return snakeBodyParts.Select(t => t.GetGridPosition() + change)
            .Where(checkPos =>
                checkPos.x is >= 0 and < GameConfig.MAPSIZE && checkPos.y is >= 0 and < GameConfig.MAPSIZE)
            .Any(checkPos => levelGrid.IsPosAtOnBoard(checkPos));
    }

    public void UpdateSnakePositionParts(Vector2Int changeValue)
    {
        for (int i = 0; i < snakePositionParts.Count; i++)
        {
            snakePositionParts[i] += changeValue;
            snakeBodyParts[i].UpdateSnakeMovePosition(snakePositionParts[i]);
        }

        _movementManager.UpdateHeadPosition(snakePositionParts[0]);
    }

    public void UpdateSnakeState(StateSnake _state, float timeDelay)
    {
        StartCoroutine(ExecuteSnakeState(timeDelay, _state));
    }

    private IEnumerator ExecuteSnakeState(float delayTime, StateSnake _state)
    {
        yield return new WaitForSeconds(delayTime);
        state = _state;
    }

    public void DoSnakeWinGame(Vector2Int port)
    {
        _snakeStateManager.DoSnakeWinGame(port);
    }

    public void DoSnakeMeetTrap(Vector2Int trapPos)
    {
        _snakeStateManager.DoSnakeMeetTrap(trapPos);
    }

    public void RutDuoi()
    {
        if (snakeBodyParts.Count > 1)
        {
            SnakeBodyPart child = snakeBodyParts[snakeBodyParts.Count - 1];
            child.RemoveFromParent();
            snakePositionParts.RemoveAt(snakePositionParts.Count - 1);
            snakeBodyParts.RemoveAt(snakeBodyParts.Count - 1);
            _snakeStateManager.UpdateStateSnake();
        }
        else
        {
            CancelInvoke();
            levelGrid._port.CloseDoor();
            CustomEventManager.Instance.WinLevel();
            if (LivesManager.instance.Lives < 5)
            {
                LifeCount.Instance.OnButtonGiveOnePressed();
            }

            SoundMaster.Instance.SoundPlayByEnum(ESoundType.WinGame, 0, null);
        }
    }

    public void CreateFireEffect(Direction direction)
    {
        var position = transform.position;
        GameObject boxMap = Instantiate(GameAssets.Instance.fireEffect, position, Quaternion.identity);
        if (boxMap.TryGetComponent(out FireEffect fireEffect))
        {
            fireEffect.SetupHead(snakeBodyParts[0].gameObject, direction, this);
        }
        boxMap.transform.SetParent(GameManager.instance.effectNode.transform);
    }

    public void MeetTrap()
    {
        if (snakeBodyParts.Count > 1)
        {
            SnakeBodyPart child = snakeBodyParts[snakeBodyParts.Count - 1];
            child.RemoveFromParent();
            snakePositionParts.RemoveAt(snakePositionParts.Count - 1);
            snakeBodyParts.RemoveAt(snakeBodyParts.Count - 1);
            _snakeStateManager.UpdateStateSnake();
        }
        else
        {
            SoundMaster.Instance.SoundPlayByEnum(ESoundType.LoseGame, 0, null);
            CancelInvoke();
            CustomEventManager.Instance.OnFailLevel();
        }
    }
}