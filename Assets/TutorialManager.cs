
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BeautifulTransitions.Scripts.Transitions;
using UnityEngine;
using Custom;
using DG.Tweening;
using Interface;
using Mkey;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;


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

    public GameObject GridBoardPrefab;
    public GameObject boardNode;
    public bool isMoving = false;
    public bool isCanMove = false;
    
    public Button btnLeft;
    public Button btnRight;
    
    public GameObject waves1;
    public GameObject waves2;

    List<Hex> bigPreviewTrihex;

    int score;

    public bool Tutorial = false;

    public int[,,] RCPosYellowHex = new int[6, 3, 2]
    {{{4,6},{4,7},{3,7}},
        {{5,6},{5,7},{5,8}},
        {{4,10},{5,9},{6,9}},
        {{2,6},{2,7},{3,6}},    
        {{1,4},{1,5},{2,5}},
        {{3,5},{4,5},{5,4}}};

    private int indexStep;

    private void Awake()
    {
        Instance = this;
       
    }

    private void Start()
    {
        Tutorial = true;
        indexStep = 0;
        Debug.Log("GameHandler.Start");
        CustomEventManager.Instance.OnWinLevel += OnWinLevel;
        btnLeft.onClick.AddListener(onRotateLeftButtonClick);
        btnRight.onClick.AddListener(onRotateRightButtonClick);
        StartNewGame();
    }

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
            grid.initHill();
        }
        
        List<Trihex> deckTut = new List<Trihex>();
        deckTut.Add(new Trihex(3,1,2,'c'));
        deckTut.Add(new Trihex(2,2,1,'c'));
        deckTut.Add(new Trihex(3,2,2,'a'));
        deckTut.Add(new Trihex(3,3,3,'c'));
        deckTut.Add(new Trihex(3,3,3,'\\'));
        deckTut.Add(new Trihex(3,2,1,'a'));


        trihexDeck = deckTut;
        SetStepTut(indexStep);

        scoreTextMain.text = " 0 ";
        deckCounterText.text = trihexDeck.Count + "";
        Debug.Log(trihexDeck.Count);
        var position = deckCounterText.transform.position;
        position = new Vector3(position.x, position.y + 0.45f, 1);
        deckCounterText.transform.position = position;
        deckCounterImage.SetActive(true);
        var sp = deckCounterImage.GetComponent<Image>();
        sp.sprite = null; //'a-shape' spFrame
        Utils.setColorAlphaImage(sp, 1);

        bigPreviewTrihex = new List<Hex>();
        
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
    }

    void pickNextTrihex()
    {
        if (this.trihexDeck.Count > 0)
        {
            this.nextTrihex = trihexDeck.Last();
            trihexDeck.RemoveAt(trihexDeck.Count - 1);
            this.deckCounterText.text = trihexDeck.Count +"";
            if (this.trihexDeck.Count > 0)
            {
                switch (this.trihexDeck[this.trihexDeck.Count - 1].shape)
                {
                    case 'a':
                    case 'v':
                        if (deckCounterImage.TryGetComponent(out Image sp))
                        {
                            sp.sprite = SpriteMgr.Instance.deckCounterImage[0];
                        }
                        break;

                    case '/':
                    case '-':
                    case '\\':
                        if (deckCounterImage.TryGetComponent(out Image sp1))
                        {
                            sp1.sprite = SpriteMgr.Instance.deckCounterImage[1];
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
                            sp2.sprite = SpriteMgr.Instance.deckCounterImage[2];
                        }
                        break;

                    default:
                        break;
                }
            }
            else
            {
                this.deckCounterImage.active = false;
                this.deckCounterText.text = "";
            }

            this.updateStaticTrihex();

            var position = deckCounterImage.transform.position;
            this.staticPreview.transform.position = new Vector3(position.x, position.y);
            this.staticPreview.transform.localScale = new Vector3(0.2f,0.2f);
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

    
    public void updateStaticTrihex() {
        var shapeIndex = HexGrid.shapes[this.nextTrihex.shape];
        for (var i = 0; i < 3; i++) {
            var row = shapeIndex[i].ro;
            var col = shapeIndex[i].co;
            var posX = (col + 0.5f * row) * Utils.d_col;
            var posY = row * Utils.d_row;
            this.bigPreviewTrihex[i].transform.position = new Vector2(posX, posY);
            this.bigPreviewTrihex[i].setType((EHexType)nextTrihex.hexes[i]);
            if (this.nextTrihex.hexes[i] == 0) {
                this.bigPreviewTrihex[i].gameObject.SetActive(false);
                this.grid.triPreviews[i].SetActive(false);
            }
        }
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

    public void OnWinLevel()
    {
        StartCoroutine(EWinLevel(1f));
    }

    IEnumerator EWinLevel(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (Common.maxLevelUnlocked == Common.GetLevelNumberNeedLoad())
        {
            Common.SaveNextStage();
        }

        ShowDialogByEDialog(EDialog.WIN);
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
            case EDialog.WIN:
                WinWindow.ShowStatic();
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
            case EDialog.WIN:
                WinWindow.HideStatic();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dialogType), dialogType, null);
        }
    }
    
    float time = 0;

    public void SetStepTut(int Step)
    {
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

    void Update ( ) {
        // Handle screen touches.
        if (Input.touchCount > 0  && Time.timeScale != 0 && !EventSystem.current.IsPointerOverGameObject())
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 pos = touch.position;
                Debug.Log("ON TOUCH START");
                OnTouchStart();
            }

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
        position = new Vector3(position.x + MathF.Sin(time) * 0.003f, position.y ) ;
        waves1.transform.position = position;
        var position1 = this.waves2.transform.position;
        position1 = new Vector3(position1.x - MathF.Sin(time)* 0.003f, position1.y ) ;
        waves2.transform.position = position1;
    }
    
    void OnTouchStart( ) {
         isMoving = false;
         isCanMove = true;
         Debug.Log("onTouchStart");
     }
    
    void  OnTouchMove(Vector2 touchPos) {
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
         if (isMoving == false) {
             // rotate
             return;
         }
    
         if (grid.placeTrihex(this.dynamicPreview.transform.position.x, this.dynamicPreview.transform.position.y, this.nextTrihex)) {
             this.pickNextTrihex();

             if (indexStep < 6) indexStep++;
             SetStepTut(indexStep);
             if (nextTrihex.hexes[0] == 0) {
                 staticPreview.SetActive(false);
                 dynamicPreview.SetActive(false);
             }
             if (nextTrihex.hexes[0] == (int)EHexType.empty || !grid.canPlaceShape(nextTrihex.shape))
             {
                 StartCoroutine(EndgameAction(2.5f));
                 StartCoroutine(DeactivateameAction( 1.2f));
             }
         }
         isMoving = false;
        grid.updateTriPreview(GameConfig.DynamicPos.X, GameConfig.DynamicPos.Y, this.nextTrihex, true);
        Debug.Log("ON TOUCH END");
     }

     private IEnumerator EndgameAction(float delayTime)
     {
         yield return new WaitForSeconds(delayTime);
         grid.sinkBlanks();
         endGame();
     }
     
     private IEnumerator DeactivateameAction(float delayTime)
     {
         yield return new WaitForSeconds(delayTime);
         grid.deactivate();
     }

     void endGame() {
         Debug.Log("END ALREADY");
     }
     
     
     void onRotateRightButtonClick() {
         SoundMaster.Instance.SoundPlayByEnum(EAudioEffectID.click, 0, 0.9f, null);
         this.rotateRight();
     }

     void rotateRight() {
         this.nextTrihex.rotateRight();
         this.grid.updateTriPreview(0, 0, this.nextTrihex);
         this.updateStaticTrihex();
     }

     void onRotateLeftButtonClick() {
         SoundMaster.Instance.SoundPlayByEnum(EAudioEffectID.click, 0, 0.9f, null);
         this.rotateLeft();
     }

     void rotateLeft() {
         this.nextTrihex.rotateLeft();
         this.grid.updateTriPreview(0, 0, this.nextTrihex);
         this.updateStaticTrihex();
     }
}