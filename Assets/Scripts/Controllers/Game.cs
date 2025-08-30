using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

public enum FBStatus
{
    none,
    share,
    invite,
    loginPopup
}

[Serializable]
public enum CardType
{
    None = -1,
    Virabhadra,
    Ghatotkacha,
    Shakuni,
    Angad,
    Sahadeva,
    Jatayu,
    Satyaki,
    Jamvanta,
    Nakul,
    Meghanath,
    Rocktower,
    BrahmosBuilding,
    FireHayBall,
    Agniastra,
    Nagastra,
    Lightning,
    Tornado,
    Chakravyuha,
    TreeMonkeyBuilding,
    Monkey,
    //3 Monkey tree 
    Trident,
    Narayanastra,
    //new character
    AngadMinion,
    JamvantaMinion,
    JatayuMinion,
    Arjuna,
    AngadMinionSingle,
    JamvantaMinionSingle,
    JatayuMinionSingle,
    Shurpanakha,
    DragonFlySingle,
    Ahis,
    AhiSingle,
    Karana,
    FruitsTree,
    FruitHealer,
    MonkeyShack,
    SpearMonkey,
    FirecrackerShack,
    Firecracker,
    Jwala,
    Toph,
    DarkStone,
    Kinnara,
    DragonFly,
    CoconutArmy,
    CoconutSingle,
    Sampathi
    



}

[Serializable]
public enum PlanetType
{
    None = -1,
    Earth,
    Rock,
    Water,
    Air,
    Fire,
    Ether
}

    [System.Serializable]
public class HelpContent
{
    public string title;
    public string message;
}

public enum TutorialStatus
{
    Incompleted = 2,
    Completed = 1
}

public enum Quadrants
{
    None,
    First,
    Second,
    Third,
    Fourth
}

public class Game
{
    public bool commingfromkathika = false;
    public static string android_push_token;
    public static string ios_push_token;
    public static int tutorialStatus = (int)TutorialStatus.Completed;
    public static int totalCrystals = 50;
    //kavita change before build
    //public static int totalGold = 50;
    public static int totalGold = 50;
    public static string FACEBOOK_URL = "https://www.facebook.com/epikoregal/";
    public static string TWITTER_URL = "https://twitter.com/epikoregal";
    public static string WHATSAPP_URL = "";
    public static string LINKEDIN_URL = "https://www.linkedin.com/company/epikoregal/";
    public static string INSTAGRAM_URL = "https://www.instagram.com/epikoregalofficial/";
    public static string TELEGRAM_URL = "https://t.me/epikoregal";
    public static string POLICY_URL = "https://www.epikoregal.com/privacy-policy.html";
    //public static string POLICY_URL = "https://wharfstreetstudios.com/privacy-policy.html";
    public static string FORUM_URL = "https://discord.gg/BnkNDdY6vD";
    public static string SUPPORT_URL = "";
    public static string TERMOFSERVICE_URL = "https://wharfstreetstudios.com/terms-and-conditions.html";

    //NOTE : To enable/diable multiplayer. USE ONLY THIS VARIABLE to toggle betweem player modes.
    //In release build, this will always remain true.
    //by default vaule is true;

    private static bool ForceMultiplayerEnabled = true;
    private static bool multiplayer = true;

    public static bool Multiplayer
    {
        get
        {
            if (!ForceMultiplayerEnabled)
                return false;

            return multiplayer;
        }
        set
        {
            if (ForceMultiplayerEnabled)
            {
                multiplayer = value;
            }
        }
    }


    //public static bool Fromkathika
    //{
    //    get
    //    {
    //        if (!commingfromkathika)
    //            return false;

    //        return multiplayer;
    //    }
    //    set
    //    {
    //        if (commingfromkathika)
    //        {
    //            multiplayer = value;
    //        }
    //    }
    //}

    //Default value true
    private static bool ForceAIEnabled = true;

    //by default the value is false
    private static bool activateAI = false;

    public static bool ActivateAI
    {
        get
        {
            if (!ForceAIEnabled)
                return false;

            return activateAI;
        }
        set
        {
            if (ForceAIEnabled)
            {
                activateAI = value;
            }
        }
    }

    public static bool AIdebugging = false;

    public enum ToggleState
    {
        On = 0,
        Off = 1
    }


    /// <summary>
    /// Determines if is connected to internet.
    /// </summary>
    /// <returns><c>true</c> if is connected to internet; otherwise, <c>false</c>.</returns>
    public static bool IsConnectedToInternet()
    {
        try
        {
            System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient("www.google.com", 80);
            client.Close();
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
            return false;
        }
    }

    public static string GetTimeString(int h, int m, int s)
    {

        string mSt = System.String.Empty;
        string hSt = System.String.Empty;
        string sSt = System.String.Empty;

        mSt = (m < 10) ? "0" + m : m.ToString();
        sSt = (s < 10) ? "0" + s : s.ToString();
        hSt = (h < 10) ? "0" + h : h.ToString();

        return string.Format(" {0} {1} ", hSt + "H", mSt + "MIN ");//, sSt + " SEC ");
    }


    public static string GetTimeseasondayString(int d, int h, int m)
    {

        string mSt = System.String.Empty;
        string hSt = System.String.Empty;
        //string sSt = System.String.Empty;
        string dSt = System.String.Empty;

        mSt = (m < 10) ? "0" + m : m.ToString();
        //sSt = (s < 10) ? "0" + s : s.ToString();
        hSt = (h < 10) ? "0" + h : h.ToString();
        dSt = (d < 10) ? "0" + d : d.ToString();

        return string.Format(" {0} {1} ", dSt + "d ", hSt + "h ", mSt + "m ");//, sSt + " SEC ");
    }

    public static string GetTimeseasonString(int h, int m, int s)
    {

        string mSt = System.String.Empty;
        string hSt = System.String.Empty;
        string sSt = System.String.Empty;

        mSt = (m < 10) ? "0" + m : m.ToString();
        sSt = (s < 10) ? "0" + s : s.ToString();
        hSt = (h < 10) ? "0" + h : h.ToString();

        return string.Format(" {0} {1} ", hSt + "h", mSt + "m ");//, sSt + " SEC ");
    }


    public static string GetTimeseasonString( int m, int s)
    {

        string mSt = System.String.Empty;
        string hSt = System.String.Empty;
        string sSt = System.String.Empty;

        mSt = (m < 10) ? "0" + m : m.ToString();
        sSt = (s < 10) ? "0" + s : s.ToString();
        //hSt = (h < 10) ? "0" + h : h.ToString();

        return string.Format(" {0} {1} ", mSt + "m ", sSt + "s ");
    }

    public static string GetTimeseasonString( int s)
    {

        //string mSt = System.String.Empty;
        //string hSt = System.String.Empty;
        string sSt = System.String.Empty;

        //mSt = (m < 10) ? "0" + m : m.ToString();
        sSt = (s < 10) ? "0" + s : s.ToString();
        //hSt = (h < 10) ? "0" + h : h.ToString();

        return string.Format("{0} {1}", sSt + "s ");
    }

    //public static string GetTimeString(int h, int m, int s)
    //{

    //    string mSt = System.String.Empty;
    //    string hSt = System.String.Empty;
    //    string sSt = System.String.Empty;

    //    mSt = (m < 10) ? "0" + m : m.ToString();
    //    sSt = (s < 10) ? "0" + s : s.ToString();
    //    hSt = (h < 10) ? "0" + h : h.ToString();

    //    return string.Format(" {0} {1} {2}", hSt + "H", mSt + "MIN ", sSt + " SEC ");
    //}


    public static string GetTimeString(int m, int s)
    {
        string mSt = System.String.Empty;
        string sSt = System.String.Empty;

        mSt = (m < 10) ? "0" + m : m.ToString();
        sSt = (s < 10) ? "0" + s : s.ToString();
        //hSt = (h < 10) ? "0" + h : h.ToString();

        return string.Format("{0}:{1}", mSt, sSt);
    }

    /// <summary>
    /// Sets the toggle status.
    /// </summary>
    /// <param name="stateName">State name.</param>
    /// <param name="status">Status.</param>
    public static void SetToggleStatus(string stateName, int status)
    {
        Debug.Log("stateName" + stateName + "status" + status);
        PlayerPrefs.SetInt(stateName, status);
    }

    /// <summary>
    /// Gets the toggle status.
    /// </summary>
    /// <returns>The toggle status.</returns>
    /// <param name="stateName">State name.</param>
    public static int GetToggleStatus(string stateName)
    {
        return PlayerPrefs.GetInt(stateName);
    }

    public static string GetPlayerPic(string key)
    {
        return PlayerPrefs.GetString(key, "NULL");
    }

    public static void SetPlayerPic(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public static string Getgoogleid(string key)
    {
        return PlayerPrefs.GetString(key, "NULL");
    }

    public static void Setgoogleid(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public static string Getgamecenterid(string key)
    {
        return PlayerPrefs.GetString(key, "NULL");
    }

    public static void Setgamecenterid(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    /// <summary>
    /// Returns the encoded string.
    /// </summary>
    /// <returns>The escape UR.</returns>
    /// <param name="url">URL.</param>
    public static string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }

    public static void ClearData()
    {
        PlayerPrefs.DeleteAll();
    }

    #region CoinManagement

    /// <summary>
    /// Sets the total crystals.
    /// </summary>
    /// <param name="mValue">M value.</param>
    public static void SetCrystals(int mValue)
    {
        totalCrystals = mValue;
    }

    /// <summary>
    /// Sets the total gold.
    /// </summary>
    /// <param name="mValue">M value.</param>
    public static void SetGold(int mValue)
    {
        totalGold = mValue;
    }

    #endregion

    /// <summary>
    /// Sets the tutorial status.
    /// </summary>
    /// <param name="status">Status.</param>
    public static void SetTutorialStatus(int status)
    {
        tutorialStatus = status;
        //if (TutorialPending() &&  RestAPIModule.RestAPIManager.Instance.restUserInfo.tutorial_seq < 1)
        //{
        //    Multiplayer = false;

        //    Debug.Log("Multiplayer is false____________");
        //}
        //else
        //{
        //    Multiplayer = true;
        //}
    }

    /// <summary>
    /// Returns the status of the tutorial.
    /// True if Incomplete
    /// </summary>
    /// <returns><c>true</c>, if status was tutorialed, <c>false</c> otherwise.</returns>
    public static bool TutorialPending()
    {
        //Debug.Log("TutorialStatus.Incompleted" + TutorialStatus.Incompleted);
        if (tutorialStatus == (int)TutorialStatus.Incompleted)
        {
            return true;
        }
        else
        {
            return false;
        }

        //return true;
    }

    public static void CheckAndEnableMultiplayer()
    {
        if (!Multiplayer)
        {
            Multiplayer = true;
        }
    }

    
}
