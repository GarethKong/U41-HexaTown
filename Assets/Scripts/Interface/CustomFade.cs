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
using BeautifulTransitions.Scripts.Transitions;
using UnityEngine;
using CodeMonkey.Utils;
using Mkey;

public class CustomFade : MonoBehaviour
{

    private float time;
    private float opacity;

    private void Start()
    {
        opacity = 1f;
    }

     void Update()
    {
        time += Time.deltaTime;
        if (opacity <= 0.5f)
        {
            opacity = 1f;
        }
        
        if (time > 0.1f)
        {
            opacity -= 0.05f;
            var tmp = transform.GetComponent<SpriteRenderer>().color;
            tmp.a = opacity;
            transform.GetComponent<SpriteRenderer>().color = tmp;
            time = 0;
        }
    }
}