using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Mkey;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class HomeScript : MonoBehaviour, IStoreListener
{
    [NonSerialized] public int one_day_time = 86400;

    public static HomeScript Instance;

    [SerializeField] private Button playButton;
    [SerializeField] private Button rankingButton;
    [SerializeField] private Button removeAdsButton;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private TextMeshProUGUI lblBestScore;

    int goldProductId = 0;

    private bool isInitAds = false;

    private void Awake()
    {
        Instance = this;
        playButton.onClick.AddListener(playBtn);
        rankingButton.onClick.AddListener(rankingBtn);
        removeAdsButton.onClick.AddListener(removeAds);
        tutorialButton.onClick.AddListener(tutorialBtn);

        if (Application.platform == RuntimePlatform.Android)
        {
            // PlayGamesPlatform.DebugLogEnabled = true;
            // PlayGamesPlatform.Activate();
        }
        loadData();
        // vibrateBtn.onClick.AddListener(onVibrateBtn);
    }

    private void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("Unity Android SDK");
            LoginToGooglePlay();
        }
    }

    //Android 
    public bool IsConnectedToGooglePlay = false;


    private void LoginToGooglePlay()
    {
        // PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }


    private void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            IsConnectedToGooglePlay = true;
            Debug.Log("Login suceess");
        }
        else
        {
            IsConnectedToGooglePlay = false;
        }
    }

    public void tutorialBtn()
    {
        if(SceneLoader.Instance) SceneLoader.Instance.LoadScene(2, () =>
        {
        } );
    }

    public void shopBtn()
    {
    }

    public void playBtn()
    {
    }

    public void rankingBtn()
    {
        if (!IsConnectedToGooglePlay)
        {
            LoginToGooglePlay();
        }

        Social.ShowLeaderboardUI();
    }

    public void loadData()
    {
        Common.loadPlayerData();
        lblBestScore.text = "Best Score" + "\r\n" + Common.maxScore;
    }

    void removeAds()
    {
    }


    void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //Add products that will be purchasable and indicate its type.
        //builder.AddProduct(goldProductId, ProductType.Consumable);
        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log($"In-App Purchasing initialize failed: {error}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        //Retrieve the purchased product
        var product = args.purchasedProduct;

        //Add the purchased product to the players inventory


        Debug.Log($"Purchase Complete - Product: {product.definition.id}");

        //We return Complete, informing IAP that the processing on our side is done and the transaction can be closed.
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        throw new NotImplementedException();
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        throw new NotImplementedException();
    }

    public void ShowBonusLife()
    {
        SoundMaster.Instance.SoundPlayClick(0, null);
        UIAdsController.Instance.ShowStatic();
    }
}