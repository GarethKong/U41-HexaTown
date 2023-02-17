using System;
using System.Collections;
using DG.Tweening;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Mkey;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeSettingScript : MonoBehaviour
{
    public static HomeSettingScript instance;

    [SerializeField] private Button musicBtn;
    [SerializeField] private Button soundBtn;
    [SerializeField] private Sprite[] spList;
    [SerializeField] private RectTransform[] list;
    [SerializeField] private Transform coveredNode;
    [SerializeField] private bool isGamePlay;
    Vector3 original_pos = new Vector3(0, 0, 0);

    int totalButton = 0;
    private bool isShowed = false;
    bool isSliding = false;
    int distance = 0;

    void Awake()
    {
        instance = this;
        int spMusic = SoundMaster.Instance.MusicOn ? 0 : 1;
        musicBtn.transform.GetComponent<Image>().sprite = this.spList[spMusic];
        int spSound = SoundMaster.Instance.SoundOn ? 2 : 3;
        soundBtn.transform.GetComponent<Image>().sprite = this.spList[spSound];
        int spVibrate = SoundMaster.Instance.VibrationOn ? 4 : 5;
        //vibrateBtn.transform.GetComponent<Image>().sprite = this.spList[spVibrate];
        totalButton = this.isGamePlay ? 4 : 3;

        musicBtn.onClick.AddListener(onMusicBtn);
        soundBtn.onClick.AddListener(onSoundBtn);
    }

  

    // public void onSettingBtnUp()
    // {
    //     distance = 100;
    //     if (isSliding) return;
    //     isCollapse = !isCollapse;
    //     isSliding = true;
    //     show(isCollapse);
    //     Debug.Log("Setting" + "this.layoutState" + isCollapse);
    // }

    public void onSettingBtnDown()
    {
        distance = -150;
        if (isSliding) return;
        isSliding = true;
        Debug.Log("Setting" + "this.layoutState" + isShowed);
        show(isShowed);
    }

    void show(bool _isShowed)
    {
        HomeSettingScript self = this;
        int move_distance = _isShowed ? 0 : distance;
        coveredNode.gameObject.SetActive(!_isShowed);

        if (!_isShowed)
        {
            for (var i = 1; i < totalButton; i++) {
                list[i].transform.parent.gameObject.SetActive(true);
            }
        }

        for (int i = 1; i < totalButton; i++)
        {
            RectTransform node = this.list[i];
            Vector2 pos = node.localPosition;
            node.DOLocalMove(new Vector3(pos.x, original_pos.y + move_distance * i, 0), 0.4f).OnComplete(() =>
            {
                self.isSliding = false;
                node.parent.gameObject.SetActive(!_isShowed);
            });
        }
        isShowed = !_isShowed;
    }

    void onMusicBtn()
    {
        Debug.Log("Music");
        
        SoundMaster.Instance.SoundPlayClick(0, null);
        bool currentState = SoundMaster.Instance.MusicOn;
        SoundMaster.Instance.SetMusic(!currentState);
        int sp = SoundMaster.Instance.MusicOn ? 0 : 1;
        musicBtn.transform.GetComponent<Image>().sprite = this.spList[sp];
    }

    void onSoundBtn()
    {
        Debug.Log("Sound");
   
        
        bool currentState = SoundMaster.Instance.SoundOn;
        SoundMaster.Instance.SetSound(!currentState);
        int sp = SoundMaster.Instance.SoundOn ? 2 : 3;
        this.soundBtn.transform.GetComponent<Image>().sprite = this.spList[sp];
    }

    void onVibrateBtn()
    {
        Debug.Log("Vibrate");
        bool currentState = SoundMaster.Instance.VibrationOn;
        SoundMaster.Instance.SetVibration(!currentState);

        int sp = SoundMaster.Instance.VibrationOn ? 4 : 5;
       // this.vibrateBtn.transform.GetComponent<Image>().sprite = this.spList[sp];
    }

    public void onHomeBtn()
    {
        SceneManager.LoadScene ("HomeScreen");
    }
}