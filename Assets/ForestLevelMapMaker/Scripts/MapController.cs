using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UI;

namespace Mkey
{
    public class MapController : MonoBehaviour
    {
        private List<LevelButton> mapLevelButtons;

        public List<LevelButton> MapLevelButtons
        {
            get { return mapLevelButtons; }
            set { mapLevelButtons = value; }
        }

        public static MapController Instance;
        private LevelButton activeButton;

        public LevelButton ActiveButton
        {
            get { return activeButton; }
            set { activeButton = value; }
        }

        [HideInInspector] public Canvas parentCanvas;
        private MapMaker mapMaker;
        private ScrollRect sRect;
        private RectTransform content;

        public GameObject arrow;
        // public RectTransform arrow;
        //
        // public RectTransform testRect;

        private int biomesCount = 6;

        public static int currentLevel = 1; // set from this script by clicking on button. Use this variable to load appropriate level.
        // set from game MapController.topPassedLevel = 2; 

        [Header("If true, then the map will scroll to the Active Level Button", order = 1)]
        public bool scrollToActiveButton = true;

        private SoundMaster MSound => SoundMaster.Instance;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            Common.loadPlayerData();
        }

        private Vector2 WorldToCanvasPosition(RectTransform canvas, Camera camera, Vector3 position)
        {
            //Vector position (percentage from 0 to 1) considering camera size.
            //For example (0,0) is lower left, middle is (0.5,0.5)
            Vector2 temp = camera.WorldToViewportPoint(position);

            //Calculate position considering our percentage, using our canvas size
            //So if canvas size is (1100,500), and percentage is (0.5,0.5), current value will be (550,250)
            temp.x *= canvas.sizeDelta.x;
            temp.y *= canvas.sizeDelta.y;

            //The result is ready, but, t$$anonymous$$s result is correct if canvas recttransform pivot is 0,0 - left lower corner.
            //But in reality its middle (0.5,0.5) by default, so we remove the amount considering cavnas rectransform pivot.
            //We could multiply with constant 0.5, but we will actually read the value, so if custom rect transform is passed(with custom pivot) , 
            //returned value will still be correct.

            temp.x -= canvas.sizeDelta.x * canvas.pivot.x;
            temp.y -= canvas.sizeDelta.y * canvas.pivot.y;

            return temp;
        }

        

        void Start()
        {
            Debug.Log("Map controller started");
            if (mapMaker == null) mapMaker = GetComponent<MapMaker>();

            if (mapMaker == null)
            {
                Debug.LogError("No <MapMaker> component. Add <MapMaker.>");
                return;
            }

            if (mapMaker.biomes == null)
            {
                Debug.LogError("No Maps. Add Biomes to MapMaker.");
                return;
            }

            content = GetComponent<RectTransform>();
            if (!content)
            {
                Debug.LogError("No RectTransform component. Use RectTransform for MapMaker.");
                return;
            }

            List<Biome> bList = new List<Biome>(mapMaker.biomes);
            bList.RemoveAll((b) => { return b == null; });

            if (mapMaker.mapType == MapType.Vertical) bList.Reverse();
            MapLevelButtons = new List<LevelButton>();
            foreach (var b in bList)
            {
                MapLevelButtons.AddRange(b.levelButtons);
            }

            //MapLevelButtons[Common.maxLevelUnlocked + 1].imageActive.SetActive(true);

            for (int i = 0; i < MapLevelButtons.Count; i++)
            {
                int scene = i;
                MapLevelButtons[i].button.onClick.AddListener(() =>
                {
                    currentLevel = scene;
                    Common.SetLevelNumberNeedLoad(currentLevel);
                    if (MSound) MSound.SoundPlayClick(0, null);
                    Debug.Log("` scene : " + scene);
                    // if(SceneLoader.Instance) SceneLoader.Instance.LoadScene(2, () =>
                    //     LifeCount.Instance.OnButtonConsumePressed());
                    Debug.Log("on consume");
                    LifeCount.Instance.OnButtonConsumePressed();
                    if (LifeCount.Instance.checkLoadLevel)
                    {
                        currentLevel = scene;
                        if (MSound) MSound.SoundPlayClick(0, null);
                        Debug.Log("load scene : " + scene);
                        if (SceneLoader.Instance) SceneLoader.Instance.LoadScene(2, () => { });
                        Debug.Log("LOAD DONE R NHE");
                    }
                });

                SetButtonActive(scene,  scene == Common.maxScore,
                    (Common.maxScore >= scene));
                MapLevelButtons[i].numberText.text = (scene).ToString();
            }

            parentCanvas = GetComponentInParent<Canvas>();
            sRect = GetComponentInParent<ScrollRect>();

            //Vector2 temp = WorldToCanvasPosition(arrow,Camera.main, MapLevelButtons[Common.maxLevelUnlocked+1].transform.position );
            // Vector3 worldPos = testRect.transform.position;
            // arrow.transform.localPosition = WorldToCanvasPosition((RectTransform)parentCanvas.transform,Camera.main, worldPos);
            
            // Vector3 relPos = content.InverseTransformPoint(ActiveButton.transform.position);
            // arrow.position = relPos;

            if (scrollToActiveButton) StartCoroutine(SetMapPositionToAciveButton());
        }

        IEnumerator SetMapPositionToAciveButton()
        {
            yield return new WaitForSeconds(0.1f);
            if (sRect)
            {
                int bCount = mapMaker.biomes.Count;
                if (mapMaker.mapType == MapType.Vertical)
                {
                    float contentSizeY = content.sizeDelta.y / (bCount) * (bCount - 1.0f);
                    float relPos = content.InverseTransformPoint(ActiveButton.transform.position)
                        .y; // Debug.Log("contentY : " + contentSizeY +  " ;relpos : " + relPos + " : " + relPos / contentSizeY);
                    float vpos = (-contentSizeY / (bCount * 2.0f) + relPos) / contentSizeY; // 
                    //  sRect.verticalNormalizedPosition = Mathf.Clamp01(vpos); // Debug.Log("vpos : " + Mathf.Clamp01(vpos));

                    SimpleTween.Cancel(gameObject, false);
                    float start = sRect.verticalNormalizedPosition;

                    SimpleTween.Value(gameObject, start, vpos, 0.25f).SetOnUpdate((float f) =>
                    {
                        sRect.verticalNormalizedPosition = Mathf.Clamp01(f);
                    });
                }
                else
                {
                    float contentSizeX = content.sizeDelta.x / (bCount) * (bCount - 1.0f);
                    float relPos = content.InverseTransformPoint(ActiveButton.transform.position).x;
                    float hpos = (-contentSizeX / (bCount * 2.0f) + relPos) / contentSizeX; // 
                    //sRect.horizontalNormalizedPosition = Mathf.Clamp01(hpos);

                    SimpleTween.Cancel(gameObject, false);
                    float start = sRect.horizontalNormalizedPosition;

                    SimpleTween.Value(gameObject, start, hpos, 0.25f).SetOnUpdate((float f) =>
                    {
                        sRect.horizontalNormalizedPosition = Mathf.Clamp01(f);
                    });
                }
            }
            else
            {
                Debug.Log("no scrolling rect");
            }
            
            // Vector3 worldPos = MapLevelButtons[Common.maxLevelUnlocked + 1].transform.position;
            // arrow.position = CanvasPositioningExtensions.WorldToCanvasPosition(parentCanvas, worldPos);
            
             //RectTransform rectTransform =(RectTransform) MapLevelButtons[Common.maxLevelUnlocked + 1].transform;
             //var screenToWorldPosition = Camera.main.ScreenToWorldPoint(testRect.position);
             arrow.transform.position = activeButton.transform.position;
        }

        private void SetButtonActive(int sceneNumber, bool active, bool isPassed)
        {
            string saveKey = sceneNumber.ToString() + "_stars_";
            int activeStarsCount = (PlayerPrefs.HasKey(saveKey)) ? PlayerPrefs.GetInt(saveKey) : 0;
            MapLevelButtons[sceneNumber].SetActive(active, activeStarsCount, isPassed);
        }

        public void SetControlActivity(bool activity)
        {
            for (int i = 0; i < MapLevelButtons.Count; i++)
            {
                if (!activity) MapLevelButtons[i].button.interactable = activity;
                else
                {
                    MapLevelButtons[i].button.interactable = MapLevelButtons[i].Interactable;
                }
            }
        }

        void Update_rem()
        {
            Debug.Log(content.sizeDelta.y + " : " + content.InverseTransformPoint(ActiveButton.transform.position).y);
            Debug.Log("sRect.verticalNormalizedPosition: " + sRect.verticalNormalizedPosition);
            Debug.Log("sRect.verticalNormalizedPosition: " + sRect.horizontalNormalizedPosition);
        }
    }
}