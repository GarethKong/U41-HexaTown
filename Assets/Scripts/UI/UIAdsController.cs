using System;
using BeautifulTransitions.Scripts.Transitions;
using CodeMonkey.Utils;
using DG.Tweening;
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
        public Button_UI btnClose;
        public static UIAdsController Instance;

        public bool claimAds = false;

        public GameObject background;
        public bool isOnGamePlay;

        public TextMeshProUGUI lblBtnAdsInfo;
        public TextMeshProUGUI lblNumberLife;
        public GameObject heartTopScreen;
        public GameObject heartEffect;
        public GameObject effectNode;
        public GameObject heartOnDialog;

        void Awake()
        {
            Instance = this;
            Hide();
        }

        private void Start()
        {
            btnClaim.ClickFunc = () =>
            {
                if (!Common.isRemovedAds)
                {
                    GoogleAdMobController.Instance.ShowRewardedAd();
                }
                else
                {
                    RewardAdsVideo();
                }
                //RewardAdsVideo();
            };

            btnClose.ClickFunc = () => { HideStatic(); };
        }


        public void RewardAdsVideo()
        {
            if (!isOnGamePlay)
            {
                if (Common.checkWatchTut())
                {
                    if (LifeCount.Instance.checkLoadLevel)
                    {
                        if (SceneLoader.Instance) SceneLoader.Instance.LoadScene(2, () => { });
                    }
                }
                else
                {
                    LifeCount.Instance.OnButtonConsumePressed();
                    if (LifeCount.Instance.checkLoadLevel)
                    {
                        if (SceneLoader.Instance) SceneLoader.Instance.LoadScene(3, () => { });
                    }
                }
            }
            StartAnimAds(heartTopScreen.transform.position);
        }

        public void StartAnimAds(Vector3 targetPos)
        {
            var startPos = effectNode.transform.position;
            var heart = Instantiate(heartEffect, startPos, Quaternion.identity);
            heart.transform.parent = effectNode.transform;
            heart.transform.DOMove(targetPos, 2).OnComplete(() =>
            {
                LivesManager.instance.GiveOneLife();
                Destroy(heart);
                HideStatic();
            });
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
                lblBtnAdsInfo.gameObject.SetActive(true);
                btnClaim.gameObject.SetActive(false);
                lblNumberLife.text = "5/5";
            }
            else
            {
                lblTime.text = "Refill Heart";
                TimeCountNextLife.text = "Next heart in " + LivesManager.instance.RemainingTimeString;
                lblBtnAdsInfo.gameObject.SetActive(false);
                btnClaim.gameObject.SetActive(true);
                lblNumberLife.text = "+1";
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