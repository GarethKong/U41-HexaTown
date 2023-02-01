using System;
using System.Collections;
using System.Collections.Generic;
using Custom;
using Interface;
using UnityEngine;

public class FireEffect : MonoBehaviour
{
    private GameObject headGameObject;
    private Direction _direction;
    private Vector2 valueChange;
    private Snake _snake;

    private void Start()
    {
        CustomEventManager.Instance.OnStopFire += StopFire;
    }

    public void SetupHead(GameObject head, Direction direction, Snake snake)
    {
        headGameObject = head;
        _direction = direction;
        valueChange = MovementMap.GetChangeReserve(_direction) * new Vector2(1.5f, 1.5f);
        var rotation = MovementMap.GetRotationByDirection(_direction);
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        _snake = snake;
    }


    void Update()
    {
        if (headGameObject != null)
        {
            Vector3 snakePos = headGameObject.transform.position;
            Vector3 currentPos = new Vector3(snakePos.x + valueChange.x, snakePos.y + valueChange.y + 0.22f, -1);
            transform.position = currentPos;
        }
    }

    private void OnDestroy()
    {
        CustomEventManager.Instance.OnStopFire -= StopFire;
    }

    void StopFire()
    {
        _snake.FireHandle();
        Destroy(gameObject);
    }
}