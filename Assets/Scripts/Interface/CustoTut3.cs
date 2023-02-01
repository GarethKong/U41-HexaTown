/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using UnityEngine;

public class CustoTut3 : MonoBehaviour
{
     void Start()
    {
        StartCoroutine(StartDisplay());
    }

    private IEnumerator StartDisplay()
    {
        GameObject go1 = transform.GetChild(0).gameObject;
        GameObject go2 = transform.GetChild(1).gameObject;
        GameObject go3 = transform.GetChild(2).gameObject;
        GameObject go4 = transform.GetChild(3).gameObject;
        GameObject go5 = transform.GetChild(4).gameObject;
        GameObject go6 = transform.GetChild(5).gameObject;
        go1.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        go2.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        go3.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        go4.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        go5.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        go6.SetActive(true);
    }
}