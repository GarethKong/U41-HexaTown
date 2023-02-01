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
using Custom;
using DG.Tweening;
using UnityEngine;

public class SpicingNode : MonoBehaviour
{
    void Update()
    {
        /*if (Snake.Instance.state == Snake.StateSnake.Spicing)
        {
            var speed = 5.0f;
            var intensity = 0.1f;

            transform.localPosition = intensity * new Vector3(
                Mathf.PerlinNoise(speed * Time.time, 1),
                Mathf.PerlinNoise(speed * Time.time, 2),
                Mathf.PerlinNoise(speed * Time.time, 3));
        }*/
    }

    public GameObject FindGameObjectAtPos(Vector2Int checkPos)
    {
        GameObject currentObject = null;
        int count = transform.childCount;
        for (int i = 0; i < count; ++i)
        {
            Transform child = transform.GetChild(i);
            var position = child.position;
            int x = (int)position.x;
            int y = (int)position.y;
            if (checkPos.x == x && checkPos.y == y)
            {
                currentObject = transform.GetChild(i).gameObject;
                break;
            }
        }

        return currentObject;
    }
}