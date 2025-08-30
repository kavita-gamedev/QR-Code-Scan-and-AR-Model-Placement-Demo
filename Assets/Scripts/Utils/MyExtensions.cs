using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RestAPIModule;
//using Hashtable = ExitGames.Client.Photon.Hashtable;

public static class MyExtensions
{
    public static RestAPIEntry SetValue(this string _key, object _value)
    {
        return new RestAPIEntry(_key, _value);
    }

    public static void Add(this List<RestAPIEntry> list, string _key, object _value)
    {
        list.Add(new RestAPIEntry(_key, _value));
    }

    public static float RoundOff(this float mValue)
    {
        return Mathf.Round(mValue * 10f) / 10f;
    }

    public static Vector3 RoundOff(this Vector3 thisVec)
    {
        float newX = Mathf.Round(thisVec.x * 10f) / 10f;
        float newY = Mathf.Round(thisVec.y * 10f) / 10f;
        float newZ = Mathf.Round(thisVec.z * 10f) / 10f;

        return new Vector3(newX, newY, newZ);
    }


    //public static void SetPlayerReady(this PhotonPlayer player)
    //{
    //    Hashtable playerState = new Hashtable();  // using PUN's implementation of Hashtable
    //    playerState[PunPropertyName.PlayerReady] = true;

    //    player.SetCustomProperties(playerState);  // this locally sets the score and will sync it in-game asap.
    //    Debug.Log("SetPlayerReady" + playerState);
    //}

    //public static bool IsReady(this PhotonPlayer player)
    //{
    //    object isready;
    //    if (player.CustomProperties.TryGetValue(PunPropertyName.PlayerReady, out isready))
    //    {
    //        return (bool)isready;
    //    }

    //    return false;
    //}

    public static string ToShortString(this string inputString)
    {
        int validCharacters = 10;
        //int maxDots = 2;

        if (inputString.Length > validCharacters)
        {
            inputString = inputString.Remove(10) + "..";
        }

        return inputString;
    }

    //    public static T GetProperty<T>(this CardInfo cardInfo, string key)
    //    {
    //
    //        ServerCardProperty scp = cardInfo.property_list.Find(t => t.property_id.Equals(key));
    //        if (scp == null)
    //            return default (T);
    //
    //        return (T)scp.property_value;
    //    }

//    public static float GetPropertyValue(this CardInfo cardInfo, string key)
//    {

//        ServerCardProperty scp = cardInfo.property_list.Find(t => t.property_id.Equals(key));
////        if (scp == null)
////            return -1;

//        return scp.property_value;
//    }

    public static int ToInt(this float num)
    {
        return (int)num;
    }

    public static Coroutine Execute(this MonoBehaviour monoBehaviour, Action action, float time)
    {
        return monoBehaviour.StartCoroutine(DelayedAction(action, time));
    }
    static IEnumerator DelayedAction(Action action, float time)
    {
        yield return new WaitForSecondsRealtime(time);

        action();
    }
}
