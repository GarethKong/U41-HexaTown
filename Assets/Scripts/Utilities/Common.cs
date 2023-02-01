using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;


// import FBGlobal from "../facebook/FBGlobal";

public static class Common
{
    public static int TOTAL_LEVEL = 100;
    public static bool isRemovedAds;

    public static int maxLevelUnlocked = 0;
    public static int currentStageLoad = 0;
    public static bool isTutorialLevel = false;

    public static void savePlayerData()
    {
        DataLocal localData = new DataLocal();
        localData.maxLevelUnlocked = maxLevelUnlocked;
        localData.isRemovedAds = isRemovedAds;
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
            maxLevelUnlocked = data.maxLevelUnlocked;
            SetLevelNumberUnlocked(maxLevelUnlocked);
        }
        else
        {
            Debug.Log("GameDots_PlayerData-loadPlayerData-NoDataReset");
            resetPlayerData();
        }
    }

    public static void resetPlayerData()
    {
        currentStageLoad = 0;
        savePlayerData();
    }

    public static void saveScore(int score)
    {
        if (maxLevelUnlocked < score)
        {
            maxLevelUnlocked = score;
            //TODO LEADERBOARD GameServices.ReportScore(bestScore, EM_GameServicesConstants.Leaderboard_High_Score);
            //CloudOnceUtils.LeaderboardUtils.SubmitScore(bestScore);
        }

        savePlayerData();
    }


    public static void SaveNextStage()
    {
        maxLevelUnlocked += 1;
        SetLevelNumberUnlocked(maxLevelUnlocked);
        savePlayerData();
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
        isTutorialLevel = levelNeedLoad == 0;
        return currentStageLoad = levelNeedLoad;
    }

    public static void removeAdsPurchase()
    {
        isRemovedAds = true;
        savePlayerData();
    }

    public static bool checkRemoveAds()
    {
        return isRemovedAds;
    }
}