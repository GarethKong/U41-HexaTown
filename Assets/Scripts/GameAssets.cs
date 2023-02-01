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
using UnityEngine;

public class GameAssets : MonoBehaviour
{

    public static GameAssets Instance;

    private void Awake() {
        Instance = this;
    }

    public GameObject dust;
    
    [Header("SNAKE HEAD")]
    public Sprite snakeHeadSprite;
    public Sprite snakeHeadLeftSprite;
    public Sprite snakeHeadRightSprite;
    public Sprite snakeHeadDownSprite;

    [Header("SNAKE HEAD BANANA")] 
    public Sprite SnakeHeadSpriteBanana;
    public Sprite SnakeHeadDownSpriteBanana;
    public Sprite SnakeHeadLeftSpriteBanana;
    public Sprite SnakeHeadRightSpriteBanana;

    [Header("SNAKE HEAD CHILLI")] 
    public Sprite snakeHeadSpriteChilli;
    public Sprite snakeHeadDownSpriteChilli;
    public Sprite snakeHeadLeftSpriteChilli;
    public Sprite snakeHeadRightSpriteChilli;
    
    [Header("SNAKE HEAD SPIT FIRE")] 
    public Sprite snakeHeadSpriteSpitFire;
    public Sprite snakeHeadDownSpriteSpitFire;
    public Sprite snakeHeadLeftSpriteSpitFire;
    public Sprite snakeHeadRightSpriteSpitFire;
    
    [Header("SNAKE HEAD DROP")] 
    public Sprite snakeHeadSpriteDrop;
    public Sprite snakeHeadDownSpriteDrop;
    public Sprite snakeHeadLeftSpriteDrop;
    public Sprite snakeHeadRightSpriteDrop;

    [Header("SNAKE HEAD DROP BANANA OR CHILLI")] 
    public Sprite snakeHeadSpriteDBAC;
    public Sprite snakeHeadDownSpriteDBAC;
    public Sprite snakeHeadLeftSpriteDBAC;
    public Sprite snakeHeadRightSpriteDBAC;

    [Space(10)]
    [Header("SNAKE BODY")]
    public Sprite snakeBodySprite;
    public Sprite snakeBodyStraightSprite;
    public Sprite snakeBodyLSprite;
    public Sprite snakeBodyLLeftSprite;
    public Sprite snakeBodyLLeftDownSprite;
    public Sprite snakeBodyLRightSprite;
    public Sprite snakeBodyLRightDownSprite;


    [Space(10)]
    [Header("SNAKE TAIL")]
    public Sprite snakeTailSprite;
    public Sprite snakeTailRightSprite;
    public Sprite snakeTailLeftSprite;
    public Sprite snakeTailDownSprite;
    

    [Space(30)]
    public Sprite[] boxSprite;
    public Sprite stoneSprite;
    public Sprite spicySprite;
    public Sprite bananaSprite;
    public Sprite portOpenSprite;
    public Sprite portCloseSprite;
    
    
    [Space(30)]
    public TextAsset[] levelList;

    
    //Prefab state
    public GameObject pbBanana;
    public GameObject pbSpicy;
    public GameObject pbStone;
    public GameObject pbBodyParts;
    public GameObject pbIce;
    public GameObject pbWood;
    public GameObject pbTrap;
    public GameObject pbPort;
    public GameObject pbSnake;

    public GameObject lifeEffect;
    public GameObject eatEffectStar;
    public GameObject dropEffect;
    public GameObject fireEffect;
    public GameObject iceBurnEffect;
}
