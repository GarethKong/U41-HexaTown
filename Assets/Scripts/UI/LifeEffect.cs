/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using UnityEngine;
using UnityEngine.UI;

public class LifeEffect : MonoBehaviour
{
    public Text textLife;

    void SetTextNumber(string numberLife)
    {
        textLife.text = numberLife;
    }
}