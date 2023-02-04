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
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayWindow : MonoBehaviour {

    private static GamePlayWindow instance;

    public TextMeshProUGUI levelText;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        UpdateCurrentLevel();
    }

    private void UpdateCurrentLevel() {
        levelText.text = Score.GetScore().ToString();
    }
    
    public static void ShowStatic() {
        instance.gameObject.SetActive(true);
        GameManager.Instance.background.SetActive(false);
    }

    public static void HideStatic() {
        instance.gameObject.SetActive(false);
    }
}
