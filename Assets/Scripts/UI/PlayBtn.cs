
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
            if (Common.isFirstTime)
            {
                if(SceneLoader.Instance) SceneLoader.Instance.LoadScene(2, () =>
                {
                } );
            }
            else
            {
                LifeCount.Instance.OnButtonConsumePressed();
                if (LifeCount.Instance.checkLoadLevel)
                {
                    if(SceneLoader.Instance) SceneLoader.Instance.LoadScene(3, () =>
                    {
                    } );
                }
            }
        };
    }
}
