using System;
#if UNITY_ANDROID
        using GooglePlayGames;
        using GooglePlayGames.BasicApi;
#endif

using Mkey;
using TMPro;
using UI;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
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
        tutorialButton.onClick.AddListener(tutorialBtn);
        Application.targetFrameRate = 120;



#if UNITY_ANDROID
                 //PlayGamesPlatform.DebugLogEnabled = true;
                 PlayGamesPlatform.Activate();
#endif


        Initialize(OnSuccess, OnError);
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

        InitializePurchasing();
    }

    //Android 
    public bool IsConnectedToGooglePlay = false;
    public bool IsConnectedToGamecenter = false;


    private void LoginToGooglePlay()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
#endif
    }

    private void LoginToGameCenter()
    {
#if UNITY_IOS
        Social.localUser.Authenticate(ProcessAuthentication);
#endif
    }


#if UNITY_ANDROID
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
#endif

#if UNITY_IOS
    void ProcessAuthentication(bool success)
    {
        if (success)
        {
            IsConnectedToGamecenter = true;
        }
        else
        {
            IsConnectedToGamecenter = false;
        }
    }
#endif


    public void tutorialBtn()
    {
        GameConfig.isTutFromHomePlay = false;
        if (SceneLoader.Instance) SceneLoader.Instance.LoadScene(2, () => { });
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
        if (!IsConnectedToGamecenter)
        {
            LoginToGameCenter();
        }

        Social.ShowLeaderboardUI();
    }

    public void loadData()
    {
        Common.loadPlayerData();
        lblBestScore.text = "Best Score" + "\r\n" + Common.maxScore;
    }

    public void ShowBonusLife()
    {
        SoundMaster.Instance.SoundPlayClick(0, null);
        UIAdsController.Instance.ShowStatic();
    }


    IStoreController m_StoreController; // The Unity Purchasing system.

    //Your products IDs. They should match the ids of your products in your store.
    public string removeAdsProductId = "com.kongsoftware.hexatown.removeads";

    void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //Add products that will be purchasable and indicate its type.
        builder.AddProduct(removeAdsProductId, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void BuyGold()
    {
        m_StoreController.InitiatePurchase(removeAdsProductId);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("In-App Purchasing successfully initialized");
        m_StoreController = controller;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log($"In-App Purchasing initialize failed: {error}");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        //Retrieve the purchased product
        var product = args.purchasedProduct;

        //Add the purchased product to the players inventory
        if (product.definition.id == removeAdsProductId)
        {
            RemoveAds();
        }
        //Show dialog complete purchase ads

        Debug.Log($"Purchase Complete - Product: {product.definition.id}");

        //We return Complete, informing IAP that the processing on our side is done and the transaction can be closed.
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
    }

    void RemoveAds()
    {
        Common.removeAdsPurchase();
        GoogleAdMobController.Instance.DestroyBannerAd();
        PurchaseComplete.Instance.ShowStatic();
    }

    const string k_Environment = "production";

    void Initialize(Action onSuccess, Action<string> onError)
    {
        try
        {
            var options = new InitializationOptions().SetEnvironmentName(k_Environment);

            UnityServices.InitializeAsync(options).ContinueWith(task => onSuccess());
        }
        catch (Exception exception)
        {
            onError(exception.Message);
        }
    }

    void OnSuccess()
    {
        var text = "Congratulations!\nUnity Gaming Services has been successfully initialized.";
        Debug.Log(text);
    }

    void OnError(string message)
    {
        var text = $"Unity Gaming Services failed to initialize with error: {message}.";
        Debug.LogError(text);
    }

    public void CloseSetting()
    {
        HomeSettingScript.instance.onSettingBtnDown();
    }

    public GameObject effectTouch;
    public GameObject TouchList;

    void Update()
    {
        // Handle screen touches.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                var touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                var touching = Instantiate(effectTouch, touchPos, effectTouch.transform.rotation);
                touching.transform.parent = TouchList.transform.parent;
                var pos = touching.transform.localPosition;
                touching.transform.localPosition = new Vector3(pos.x, pos.y, 100);
            }
        }
    }
}