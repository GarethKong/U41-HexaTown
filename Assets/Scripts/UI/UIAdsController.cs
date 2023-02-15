using System;
using BeautifulTransitions.Scripts.Transitions;
using CodeMonkey.Utils;
using ExaGames.Common;
using Mkey;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIAdsController : MonoBehaviour
    {
        public TextMeshProUGUI TimeCountNextLife;
        public TextMeshProUGUI lblTime;
        //public TextMeshProUGUI NumberLifeCount;

        public Button_UI btnClaim;
        public static UIAdsController Instance;

        public bool claimAds = false;
        
        public GameObject background;


        void Awake()
        {
            Instance = this;
            Hide();
        }

        private void Start()
        {
            btnClaim.ClickFunc = () =>
            {
                //TODO Show ads
                LivesManager.instance.GiveOneLife();
                LifeCount.Instance.OnButtonConsumePressed();
                if (LifeCount.Instance.checkLoadLevel)
                {
                    if(SceneLoader.Instance) SceneLoader.Instance.LoadScene(2, () =>
                    {
                    } );
                }
            };
        }

        private void Show()
        {
            if (claimAds)
            {
                int lives = LivesManager.instance.Lives;
                lives++;
                string life = lives.ToString();
                //NumberLifeCount.text = "You have " + life + "/5 live";
            }
            else
            {
                //NumberLifeCount.text = "You have " + LivesManager.instance.LivesText + "/5 live";
            }

            gameObject.SetActive(true);
            TransitionHelper.TransitionIn(Instance.gameObject);
        }

        private void Update()
        {
            if (LivesManager.instance.Lives >= 5)
            {
                TimeCountNextLife.text = "";
                lblTime.text = "Full of Heart";
            }
            else
            {
                lblTime.text = "Refill Heart";
                TimeCountNextLife.text = "Next life in " + LivesManager.instance.RemainingTimeString;
            }
            //NumberLifeCount.text = "You have " + LivesManager.instance.LivesText + "/5 live";
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ShowStatic()
        {
            Instance.Show();
            background.SetActive(true);
        }

        public void HideStatic()
        {
            Instance.Hide();
            background.SetActive(false);
        }
    }
}