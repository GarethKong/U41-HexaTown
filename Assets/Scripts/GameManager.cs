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
using BeautifulTransitions.Scripts.Transitions;
using UnityEngine;
using Custom;
using DG.Tweening;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Interface;
using Mkey;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum EDialog
{
    PLAY,
    SETTING,
    ENDGAME
}


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private SnapshotCamera snapshotCamera;

    public GameObject foreground;

    private HexGrid grid;
    private Trihex nextTrihex;
    List<Trihex> trihexDeck;

    public GameObject[] listDlg;

    public GameObject background;

    public GameObject effectNode;
    public GameObject scoreNode;

    public TextMeshProUGUI scoreTextMain;
    public TextMeshProUGUI deckCounterText;

    public GameObject HexTilePrefab;
    public GameObject dynamicPreview;
    public GameObject staticPreview;
    public GameObject deckCounterImage;
    public GameObject staticPreviewSP;


    public GameObject GridBoardPrefab;
    public GameObject boardNode;
    public bool isMoving = false;
    public bool isCanMove = false;

    public Button btnLeft;
    public Button btnRight;

    public GameObject waves1;
    public GameObject waves2;

    public Image endGameImage;

    List<Hex> bigPreviewTrihex;

    int score;
    
    public float fadeTime = 1f;
    public bool Tutorial = false;

    public int[,,] RCPosYellowHex = new int[6, 3, 2]
    {{{4,6},{4,7},{3,7}},
        {{5,6},{5,7},{5,8}},
        {{4,10},{5,9},{6,9}},
        {{2,6},{2,7},{3,6}},    
        {{1,4},{1,5},{2,5}},
        {{3,5},{4,5},{5,4}}};

    private int indexStep = 0;
    public GameObject[] TextTutorial;

    public Camera uiCamera;





    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartTutorial(true);
        Debug.Log("GameHandler.Start");
        CustomEventManager.Instance.OnEndGame += OnEndGame;
        btnLeft.onClick.AddListener(onRotateLeftButtonClick);
        btnRight.onClick.AddListener(onRotateRightButtonClick);
        snapshotCamera = SnapshotCamera.MakeSnapshotCamera(1, "");
        StartNewGame();
    }

    private GameObject gridBoardPri;

    public void StartNewGame()
    {
        GameConfig.sessionNumber += 1;
        score = 0;
        dynamicPreview.active = true;
        var gridBoardNode = Instantiate(GridBoardPrefab);
        gridBoardNode.transform.parent = boardNode.transform;


        if (gridBoardNode.TryGetComponent(out HexGrid hexgrid))
        {
            grid = hexgrid;
            Action<int> actionUpdateScore = onScoreUpdate;
            grid.init(5, 8, 0, 0, actionUpdateScore);
        }


        
        if (Tutorial)
        {
            SetHexForTutorial();
        }
        else
        {
            trihexDeck = createTrihexDeck(GameConfig.TrihexDeckNum, true);
        }
        
        scoreTextMain.text = " 0 ";
        deckCounterText.text = trihexDeck.Count + "";
        var position = deckCounterText.transform.position;
        position = new Vector3(position.x, position.y + 0.45f, 1);
        deckCounterText.transform.position = position;
        deckCounterImage.SetActive(true);
        var sp = deckCounterImage.GetComponent<Image>();
        sp.sprite = null; //'a-shape' spFrame
        Utils.setColorAlphaImage(sp, 1);
        if (onAds)
        {
            onAds = false;
            foreach (Transform trans in adsPreview.transform)
            {
                Destroy(trans.gameObject);
            }
        }

        bigPreviewTrihex = new List<Hex>();
        staticPreviewSP.transform.position = deckCounterImage.transform.position;


        for (var i = 0; i < 3; i++)
        {
            GameObject hexNode = Instantiate(this.HexTilePrefab);
            hexNode.transform.parent = staticPreview.transform;

            if (hexNode.TryGetComponent(out Hex hex))
            {
                hex.initGrid(0, 0, -1, -1);
                bigPreviewTrihex.Add(hex);
            }
        }

        pickNextTrihex();
        grid.updateTriPreview(GameConfig.DynamicPos.X, GameConfig.DynamicPos.Y, this.nextTrihex, true);

        // if (GameConfig.sessionNumber % 3 == 0)
        // {
        //     FBGlobal.instance.showAdsInterestial();
        // }

        gridBoardPri = gridBoardNode;
    }

    void pickNextTrihex()
    {
        if (this.trihexDeck.Count > 0)
        {
            this.nextTrihex = trihexDeck.Last();
            trihexDeck.RemoveAt(trihexDeck.Count - 1);
            this.deckCounterText.text = trihexDeck.Count + "";
            if (this.trihexDeck.Count > 0)
            {
                switch (this.trihexDeck[this.trihexDeck.Count - 1].shape)
                {
                    case 'a':
                    case 'v':
                        if (deckCounterImage.TryGetComponent(out Image sp))
                        {
                            sp.sprite = onAds ? null : SpriteMgr.Instance.deckCounterImage[0];
                        }

                        break;

                    case '/':
                    case '-':
                    case '\\':
                        if (deckCounterImage.TryGetComponent(out Image sp1))
                        {
                            sp1.sprite = onAds ? null : SpriteMgr.Instance.deckCounterImage[1];
                        }

                        break;

                    case 'c':
                    case 'r':
                    case 'n':
                    case 'd':
                    case 'j':
                    case 'l':
                        if (deckCounterImage.TryGetComponent(out Image sp2))
                        {
                            sp2.sprite = onAds ? null : SpriteMgr.Instance.deckCounterImage[2];
                        }

                        break;

                    default:
                        break;
                }
            }
            else
            {
                this.deckCounterImage.SetActive(false);
                this.deckCounterText.text = "";
            }

            this.updateStaticTrihex(this.onAds);

            var position = deckCounterImage.transform.position;
            this.staticPreview.transform.position = new Vector3(position.x, position.y);
            this.staticPreview.transform.localScale = new Vector3(0.2f, 0.2f);
            //TODO cc.tween(this.staticPreview)
            //     .to(0.4,  {
            //     position:
            //     cc.v3(0, GameConfig.DynamicPos.y), scale:
            //     1
            // })
            // .start()
        }
        else
        {
            this.staticPreview.active = false;
            this.nextTrihex = new Trihex(0, 0, 0, 'a');
        }
    }


    public void updateStaticTrihex(bool isAds = false)
    {
        if (this.trihexDeck.Count == 0)
        {
            return;
        }

        var triHex = isAds ? this.trihexDeck[this.trihexDeck.Count - 1] : this.nextTrihex;
        var shapeIndex = HexGrid.shapes[triHex.shape];
        var preview = isAds ? this.adsPreviewTrihex : this.bigPreviewTrihex;

        for (var i = 0; i < 3; i++)
        {
            var row = shapeIndex[i].ro;
            var col = shapeIndex[i].co;
            var posX = (col + 0.5f * row) * Utils.d_col;
            var posY = row * Utils.d_row;
            preview[i].transform.position = new Vector3(posX + 3.5f, posY - 7);
            preview[i].GetComponent<Hex>().setType((EHexType)triHex.hexes[i]);
            if (triHex.hexes[i] == 0)
            {
                preview[i].gameObject.SetActive(false);
                grid.triPreviews[i].SetActive(false);
            }
        }
    }


    List<Trihex> createTrihexDeck(int size, bool allShapes)
    {
        List<Trihex> deck = new List<Trihex>();
        for (var i = 0; i < size; i++)
        {
            if (allShapes)
            {
                if (i < size / 3)
                {
                    deck.Add(new Trihex(0, 0, 0, Utils.pick(new[] { 'a', 'v' })));
                }
                else if (i < size / 1.5)
                {
                    deck.Add(new Trihex(0, 0, 0, Utils.pick(new[] { '/', '-', '\\' })));
                }
                else
                {
                    deck.Add(new Trihex(0, 0, 0, Utils.pick(new[] { 'c', 'r', 'n', 'd', 'j', 'l' })));
                }
            }
            else
            {
                deck.Add(new Trihex(0, 0, 0, 'a'));
            }
        }

        Utils.Shuffle(deck);
        for (var i = 0; i < size; i++)
        {
            if (i < size / 2)
            {
                deck[i].hexes[0] = 3;
            }
            else
            {
                deck[i].hexes[0] = 1;
            }
        }

        Utils.Shuffle(deck);
        for (var i = 0; i < size; i++)
        {
            if (i < size / 2)
            {
                deck[i].hexes[1] = 3;
            }
            else
            {
                deck[i].hexes[1] = 2;
            }
        }

        Utils.Shuffle(deck);
        for (var i = 0; i < size; i++)
        {
            if (i < size / 2)
            {
                deck[i].hexes[2] = 3;
            }
            else
            {
                deck[i].hexes[2] = 2;
            }

            Utils.Shuffle(deck[i].hexes);
        }

        Utils.Shuffle(deck);
        return deck;
    }

    void onScoreUpdate(int score)
    {
        this.score = score;
        this.scoreTextMain.text = score + "";
        Common.curScore = this.score;
    }

    public static void ResumeGame()
    {
        SettingWindow.HideStatic();
        Time.timeScale = 1f;
    }

    public static void PauseGame()
    {
        SettingWindow.ShowStatic();
        Time.timeScale = 0f;
    }

    public static bool IsGamePaused()
    {
        return Time.timeScale == 0f;
    }


    public void OnRestartGame()
    {
    }

    public void OnEndGame()
    {
        StartCoroutine(EEndGame(0f));
    }

    IEnumerator EEndGame(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        CaptureImage();
        ShowDialogByEDialog(EDialog.ENDGAME);
        Common.saveScore(Common.curScore);
        //GamePlayWindow.HideStatic();
    }

    public void SettingHandle()
    {
        SoundMaster.Instance.SoundPlayClick(0, null);
        ShowDialogByEDialog(EDialog.SETTING);
    }

    void ShowDialogByEDialog(EDialog dialogType)
    {
        foreach (var t in listDlg)
        {
            t.SetActive(false);
        }

        background.SetActive(true);
        switch (dialogType)
        {
            case EDialog.SETTING:
                SettingWindow.ShowStatic();
                break;
            case EDialog.ENDGAME:
                EndGameWindow.ShowStatic();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dialogType), dialogType, null);
        }
    }

    public void HideDialog(EDialog dialogType)
    {
        background.SetActive(false);
        foreach (var t in listDlg)
        {
            t.SetActive(false);
        }

        switch (dialogType)
        {
            case EDialog.SETTING:
                SettingWindow.HideStatic();
                break;
            case EDialog.ENDGAME:
                WinWindow.HideStatic();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dialogType), dialogType, null);
        }
    }

    float time = 0;
    float pressTime = 0;

    void Update()
    {
        // Handle screen touches.

        if (Input.touchCount > 0 && Time.timeScale != 0 && !EventSystem.current.IsPointerOverGameObject())
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                var touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                Debug.Log("ON TOUCH START");
                OnTouchStart(touchPos);
            }

            // if (touch.phase == TouchPhase.Stationary)
            // {
            //     pressTime += Time.deltaTime;
            // }

            if (touch.phase == TouchPhase.Moved)
            {
                var touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                touchPos.z = transform.position.z;
                OnTouchMove(touchPos);
            }

            if (touch.phase == TouchPhase.Ended)
            {
                Debug.Log("ON TOUCH END");
                var touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                touchPos.z = transform.position.z;
                OnTouchEnd(touchPos);
            }

            if (Input.touchCount == 2)
            {
            }
        }

        if (Input.mouseScrollDelta.y > 0)
        {
            rotateLeft();
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            rotateRight();
        }

        time += Time.deltaTime;
        var position = waves1.transform.position;
        position = new Vector3(position.x + MathF.Sin(time) * 0.003f, position.y);
        waves1.transform.position = position;
        var position1 = this.waves2.transform.position;
        position1 = new Vector3(position1.x - MathF.Sin(time) * 0.003f, position1.y);
        waves2.transform.position = position1;
    }

    void OnTouchStart(Vector3 touchPos)
    {
        isMoving = false;
        isCanMove = false;
        Transform[] allChildren = dynamicPreview.GetComponentsInChildren<Transform>();
        foreach (Transform child in dynamicPreview.transform)
        {
            var distance = touchPos - child.position;
            if (distance.magnitude - 10 < 0.01f)
            { 
                isCanMove = true;
                break;
            }
        }
        Debug.Log("onTouchStart");
    }

    void OnTouchMove(Vector2 touchPos)
    {
        if (!this.isCanMove) return;
        this.isMoving = true;

        var l_touchPos = touchPos;
        this.grid.updateTriPreview(l_touchPos.x, l_touchPos.y, this.nextTrihex, true);
        // Debug.Log("ON TOUCH MOVE");
    }

    void OnTouchEnd(Vector2 touchPos)
    {
        if (isCanMove == false) return;
        var self = this;
        if (isMoving == false)
        {
            // rotate
            return;
        }

        if (grid.placeTrihex(this.dynamicPreview.transform.position.x, this.dynamicPreview.transform.position.y,
                this.nextTrihex))
        {
            this.pickNextTrihex();
            
            if (Tutorial)
            {
                if (indexStep < 6) indexStep++;
                SetStepTut(indexStep);
            }

            if (nextTrihex.hexes[0] == 0)
            {
                staticPreview.SetActive(false);
                dynamicPreview.SetActive(false);
            }

            if (nextTrihex.hexes[0] == (int)EHexType.empty || !grid.canPlaceShape(nextTrihex.shape))
            {
                StartCoroutine(EndgameAction(2.5f));
                StartCoroutine(DeactivateameAction(1.2f));
            }
        }

        isMoving = false;
        grid.updateTriPreview(GameConfig.DynamicPos.X, GameConfig.DynamicPos.Y, this.nextTrihex, true);
        Debug.Log("ON TOUCH END");
    }

    private IEnumerator EndgameAction(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        CustomEventManager.Instance.EndLevel();
    }

    private IEnumerator DeactivateameAction(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        grid.deactivate();
    }

    private void CaptureImage()
    {
        int height = (int)Math.Round(Screen.width * 1.15f);
        Texture2D texture2D = snapshotCamera.TakeObjectSnapshot(gridBoardPri, Screen.width, height);
        endGameImage.sprite = Sprite.Create(texture2D, new Rect(0, 0, Screen.width, height), new Vector2());
        var rectTransform = endGameImage.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(Screen.width, height);
    }


    void onRotateRightButtonClick()
    {
        SoundMaster.Instance.SoundPlayByEnum(EAudioEffectID.click, 0, 0.9f, null);
        this.rotateRight();
        //CustomEventManager.Instance.EndLevel();
    }

    void rotateRight()
    {
        this.nextTrihex.rotateRight();
        this.grid.updateTriPreview(0, 0, this.nextTrihex);
        //this.updateStaticTrihex();
    }

    void onRotateLeftButtonClick()
    {
        SoundMaster.Instance.SoundPlayByEnum(EAudioEffectID.click, 0, 0.9f, null);
        this.rotateLeft();
    }

    void rotateLeft()
    {
        nextTrihex.rotateLeft();
        grid.updateTriPreview(0, 0, this.nextTrihex);
        //updateStaticTrihex();
    }

    public GameObject adsBtn;
    public GameObject adsPreview;
    private List<Hex> adsPreviewTrihex = new List<Hex>();
    private bool onAds = false;

    public void onAdsPreviewClick()
    {
        var self = this;
        //TODO CHECK ADS let AVSuccessCb = function (arg) {
        //     self.onShowPreview();
        // };
        // let AVFailedCb = function (arg) {
        // };
        // FBGlobal.instance.showAdsVideo(AVSuccessCb.bind(self), AVFailedCb.bind(self), null);
        self.onShowPreview();
    }

    public void onShowPreview()
    {
        this.onAds = true;
        this.adsBtn.active = false;
        this.adsPreviewTrihex = new List<Hex>();
        for (var i = 0; i < 3; i++)
        {
            var hexNode = Instantiate(this.HexTilePrefab, staticPreviewSP.transform, true);
            var hex = hexNode.GetComponent<Hex>();
            hex.initGrid(0, 0, -1, -1);
            this.adsPreviewTrihex.Add(hex);
        }

        updateStaticTrihex(onAds);
        deckCounterImage.GetComponent<Image>().enabled = false;
    }
    
    #region Trong Write
     public void StartTutorial(bool tut)
     {
         Tutorial = tut;
         // GetListTextTutorial();
     }
     
     private void TransTextTutorial(int step)
     {
         if (step == 0)
         {
             TransTextIn(step);
         }
         else
         {
             if (step < 6)
             {
                 TransTextOut(step-1);
                 TransTextIn(step);   
             }
             else
             {
                 TransTextOut(step-1);
             }

         }
     }

     private void TransTextIn(int step)
     {
         var rectTrans = TextTutorial[step].GetComponent<RectTransform>();
         rectTrans.transform.localPosition = new Vector3(-6, -5, 0);
         rectTrans.DOAnchorPos(new Vector2(1466, -5), fadeTime, false).SetEase(Ease.InBack);
     }
     
     private void TransTextOut(int step)
     {
         var rectTrans = TextTutorial[step].GetComponent<RectTransform>();
         rectTrans.DOAnchorPos(new Vector2(3000, -5), fadeTime, false).SetEase(Ease.InBack);
     }
     
     public void SetStepTut(int Step)
     {
         if (Step == 4)
         {
             grid.SetYellowHexTut(2,3);
         }
         else if(Step == 5)
         {
             grid.RemoveYellowHexTut(2,3);
         }
         
         for (int j = 0; j < 3; j++)
         {
             if(Step < 6){
                 if (Step >= 1)
                 {
                     grid.RemoveYellowHexTut(RCPosYellowHex[Step-1, j, 0], RCPosYellowHex[Step-1, j, 1]);
                 }
                 grid.SetYellowHexTut(RCPosYellowHex[Step, j, 0], RCPosYellowHex[Step, j, 1]);
             }
             else
             {
                 return;
             }
         }
     }

     private void SetHexForTutorial()
     {
         List<Trihex> deckTut = new List<Trihex>();
         deckTut.Add(new Trihex(3,1,2,'c'));
         deckTut.Add(new Trihex(2,2,1,'c'));
         deckTut.Add(new Trihex(3,2,2,'a'));
         deckTut.Add(new Trihex(3,3,3,'c'));
         deckTut.Add(new Trihex(3,3,3,'\\'));
         deckTut.Add(new Trihex(3,2,1,'a'));

         trihexDeck = deckTut;
         grid.initHill();
         SetStepTut(indexStep);
         TransTextTutorial(indexStep);
     }
     
     #endregion
}