using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using Mkey;
using UnityEngine;

public class PlayBtn : MonoBehaviour
{
    
    public Button_UI playBtn;

    public GameManager gameManager;
    
    void Awake()
    {
        playBtn.ClickFunc = () =>
        {
            SoundMaster.Instance.SoundPlayClick(0, null);
            Common.SetLevelNumberNeedLoad(Common.maxLevelUnlocked);
            LifeCount.Instance.OnButtonConsumePressed();
            if (LifeCount.Instance.checkLoadLevel)
            {
                if(SceneLoader.Instance) SceneLoader.Instance.LoadScene(2, () =>
                {
                } );
            }
            Debug.Log("btn play");
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
