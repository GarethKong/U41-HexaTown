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
    public class PurchaseComplete : MonoBehaviour
    {
        public Button_UI btnClaim;
        public Button_UI btnClose;
        public static PurchaseComplete Instance;
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
                HomeScript.Instance.BuyGold();
                HideStatic();
            };
            
            btnClose.ClickFunc = () =>
            {
                HideStatic();
            };
        }

        private void Show()
        {
            gameObject.SetActive(true);
            TransitionHelper.TransitionIn(Instance.gameObject);
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