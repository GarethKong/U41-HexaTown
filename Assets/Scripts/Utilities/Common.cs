using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;


// import FBGlobal from "../facebook/FBGlobal";

public static class Common
{
    public static int TOTAL_LEVEL = 100;
    public static bool isRemovedAds;
    public static bool isFirstTime = true;

    public static int maxScore = 0;
    public static int currentStageLoad = 0;
    public static int curScore = 0;
    public static int sessionNumber = 0;

    public static void savePlayerData()
    {
        DataLocal localData = new DataLocal();
        localData.maxScore = maxScore;
        localData.isRemovedAds = isRemovedAds;
        localData.isFirstTime = isFirstTime;
        var dataString = JsonUtility.ToJson(localData);
        PlayerPrefs.SetString("GameDots_PlayerData", dataString);
    }


    public static void loadPlayerData()
    {
        var dataString = "";
        dataString = PlayerPrefs.GetString("GameDots_PlayerData");
        initPlayerData(dataString);
    }


    public static void initPlayerData(string dataString)
    {
        if (dataString != "")
        {
            //console.log("GameDots_PlayerData-loadPlayerData:" + dataString);
            DataLocal data = JsonConvert.DeserializeObject<DataLocal>(dataString);
            isRemovedAds = data.isRemovedAds;
            maxScore = data.maxScore;
            isFirstTime = data.isFirstTime;
            SetLevelNumberUnlocked(maxScore);
        }
        else
        {
            Debug.Log("GameDots_PlayerData-loadPlayerData-NoDataReset");
            resetPlayerData();
        }
    }

    public static void resetPlayerData()
    {
        isFirstTime = true;
        savePlayerData();
    }

    public static void saveScore(int score)
    {
        if (maxScore < score)
        {
            maxScore = score;
            if (Application.platform == RuntimePlatform.Android)
            {
                Debug.Log("Save score android");
                if (HomeScript.Instance.IsConnectedToGooglePlay)
                {
                    Social.ReportScore(Common.curScore, GPGSIds.leaderboard_leader_board,
                        (bool success) => { Debug.Log("Save score" + success); });
                }
            }
            else
            {
                if (score > 50)
                {
                    ReportAchievement50();
                }

                Social.ReportScore(Common.curScore, "com.kongsoftware.hexatown.highscore",
                    (bool success) => { Debug.Log("Save score" + success); });
            }
            //TODO LEADERBOARD GameServices.ReportScore(bestScore, EM_GameServicesConstants.Leaderboard_High_Score);
            //CloudOnceUtils.LeaderboardUtils.SubmitScore(bestScore);
        }

        savePlayerData();
    }


    static void ReportAchievement50()
    {
        // Social.ReportProgress("com.kongsoftware.hexatown.level", 100,
        //     (result) => { Debug.Log(result ? "Reported achievement" : "Failed to report achievement"); });
    }


    public static void SetLevelNumberUnlocked(int num)
    {
        PlayerPrefs.SetInt("numberLevelUnlocked", num);
    }

    public static int GetLevelNumberUnlocked()
    {
        var numberUnlocked = PlayerPrefs.GetInt("numberLevelUnlocked");
        if (numberUnlocked != 0)
        {
            return numberUnlocked;
        }

        return currentStageLoad;
    }

    public static int GetLevelNumberNeedLoad()
    {
        return currentStageLoad;
    }

    public static int SetLevelNumberNeedLoad(int levelNeedLoad)
    {
        return currentStageLoad = levelNeedLoad;
    }

    public static void removeAdsPurchase()
    {
        isRemovedAds = true;
        savePlayerData();
    }

    public static void SetWatchTut()
    {
        isFirstTime = false;
        savePlayerData();
    }

    public static bool checkRemoveAds()
    {
        return isRemovedAds;
    }

    public static bool checkWatchTut()
    {
        return isFirstTime;
    }
    
    
    
#if UNITY_ANDROID
 public static string GetAndroidAdMobID() {
     UnityEngine.AndroidJavaClass up = new UnityEngine.AndroidJavaClass("com.unity3d.player.UnityPlayer");
     UnityEngine.AndroidJavaObject currentActivity = up.GetStatic<UnityEngine.AndroidJavaObject>("currentActivity");
     UnityEngine.AndroidJavaObject contentResolver = currentActivity.Call<UnityEngine.AndroidJavaObject>("getContentResolver");
     UnityEngine.AndroidJavaObject secure = new UnityEngine.AndroidJavaObject("android.provider.Settings$Secure");
     string deviceID = secure.CallStatic<string>("getString", contentResolver, "android_id");
     return Md5Sum(deviceID).ToUpper();
 }
#endif
 
#if UNITY_IPHONE
    public static string GetIOSAdMobID() {
        return Md5Sum(UnityEngine.iOS.Device.advertisingIdentifier);
    }
#endif
    public static string Md5Sum(string strToEncrypt) {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);
     
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);
     
        string hashString = ""; 
        for (int i = 0; i < hashBytes.Length; i++) {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }
     
        return hashString.PadLeft(32, '0');
    }
}