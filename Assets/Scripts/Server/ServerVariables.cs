using UnityEngine;
using System.Collections;

public class ServerVariables
{

    public const int LOGIN_FACEBOOK = 1;
    public const int LOGIN_GOOGLE = 2;
    public const int LOGIN_EMAIL = 4;
    public const int LOGIN_GAMECENTRE = 3;

    public const int RESPONSE_SUCCESS = 1;
    public const int RESPONSE_ID_PASS_MATCH_INCORRECT = 231;
    public const int RESPONSE_INVALID_TOKEN = 204;
    public const int RESPONSE_NOT_ENOUGH_INVENTORY = 248;
    public const int RESPONSE_NOT_ENOUGH_COINS = 244;
    public const int RESPONSE_USER_BLOCKED = 249;
    public const int RESPONSE_CURRENT_USER_NOT_IN_RAGE = 253;
    public const int RESPONSE_OTHER_USER_RECOVERING = 254;
    public const int RESPONSE_OTHER_USER_CAMOUFLAGE_ACTIVE = 255;
    public const int RESPONSE_OTHER_USER_BLOCKED = 257;

    //    public const int RESPONSE_INTERACTION_SENT = 246;

    public const int INVITE_ROOM_TYPE = 2;
    public const int INVITE_EXHAUSTED = 244;
    public const int INVITE_ACCEPT = 4;
    public const int INVALID_INVITE_TOKEN = 243;

    public const int FRIENDLYBATTLE_ROOM_TYPE = 3;
    public const int FRIENDLYBATTLE_TOKEN = 243;
    public const int KINGDOM_ACCEPTED = 5;
    public const int KINGDOM_REJECTED = 6;

    public const int PIC_AVATAR = 1;
    public const int PIC_GALLERY = 2;

    public enum FriendStatus
    {
        FRIEND = 1,
        NOT_FRIEND = 2,
        FRIEND_REQUEST_SENT = 11
    }

    public enum RoomSearchStatus
    {
        SEARCH = 1,
        CANCEL = 2
    }

    public enum RewardType
    {
        //1-Cube earned during match; 4-Copper cube; 5-Bronze cube
        CUBE_MATCH = 1,
        CUBE_COPPER = 4,
        CUBE_BRONZE = 5
    }

    public enum CubeType
    {
        FIRECRACKER = 1,// fire cracker
        BOMB = 2, //bomb
        ROCKET = 3, //rocket
        DYNAMIC = 4,//dynamic
        METALBOMB = 5 //metal bomb
    }

    public enum RarityType
    {
        COMMON = 0,// COMMON CARD
        RARE = 1, //RARE CARD
        EPIC = 2, //APIC CARD
        ULTRAEPIC = 3,//ULTRAEPIC

    }

    public enum DeckStatus
    {
        IN_DECK = 1,
        NOT_IN_DECK = 2
    }

    public enum RewardStatus
    {
        NONE = 0,
        NOT_CLAIMED = 1,
        IN_PROCESS = 2,
        CAN_CLAIM = 3,
        LOCKED = 4
    }

    public enum PrivacyState
    {
        SELF = 1,
        FRIENDS = 2,
        EVERYONE = 3
    }

    public enum VideoRewardStatus
    {
        ACHIEVED = 1,
        PENDING = 2
    }

    public enum RoomStatus
    {
        ACTIVE = 1,
        INACTIVE = 2
    }
    //old
    //  public const string baseApiURL = "http://www.juegostudio.in/EPIC-ROYALE/TEST/rest.php?methodName=";
    //  public const string baseApiURL = "http://juegostudio.in/EPIC-ROYALE/TEST-V2/rest.php?methodName=";
    //public const string baseApiURL = "http://203.193.140.8/epicRoyale/rest.php?methodName=";  							
    //public const string baseApiURL = "http://18.189.73.235/epicNew/rest.php?methodName=";
    //public const string baseApiURL = "http://3.11.29.238/EPIKO-ROYAL/staging/rest.php?methodName="
    //public const string baseApiURL = "http://3.11.29.238/EPIKO-ROYAL/PROD/rest.php?methodName="
    //new
    //public static string baseApiURL = "http://18.130.129.45/EPIKO/playstore/rest.php?methodName="; // playestore upload
    //public const string baseApiURL = "http://18.130.129.45/EPIKO/staging/rest.php?methodName="; //Working
    //public const string baseApiURL = "http://18.130.129.45/EPIKO/PROD/rest.php?methodName=";   //TESTING for team
    // new after aws change
    //public static string baseApiURL = "http://35.176.252.22/EPIKO/staging/rest.php?methodName="; //Working
   public static string baseApiURL = "https://epikoregalapi.com/EPIKO/playstore/epiko-backend/rest.php?methodName=";
    //public const string baseApiURL = "http://35.176.252.22/EPIKO/PROD/rest.php?methodName="; //Working
   //public static string baseApiURL = "http://35.176.252.22/EPIKO/playstorev1/rest.php?methodName="; //Working

    //API links
    //Working = team share(epiko_royal_prod)
    //TESTING for team on project(epiko_prod)
    //playstore to upload(playstore)

    public const string applicationKey = "applicationKey";
    public const int paramAppKey = 12345;












    public static string fetchuniqueDeviceID()
    {
        string hasID = "";

#if UNITY_IOS
        string saveUUID = KeyChain.BindGetKeyChainUser();
        //Debug.Log("saveUUID " + saveUUID);
      
        DeviceInfo _response = JsonUtility.FromJson<DeviceInfo>(saveUUID);
        //Debug.Log("gamelaunch before device id" + _response.userId);
        //string saveUUID = KeyChain.BindGetKeyChainUser();
        if (!string.IsNullOrEmpty(_response.userId))
        {
            //Debug.Log("fetchuniqueIDforIOS BindGetKeyChainUser" + _response.userId);

            hasID = _response.userId;
        }
        else
        {
           
           //KeyChain.BindSetKeyChainUser("0", SystemInfo.deviceUniqueIdentifier);
           string currentUUID = SystemInfo.deviceUniqueIdentifier;
            KeyChain.BindSetKeyChainUser(currentUUID);
            //Debug.Log("fetchuniqueIDforIOS BindSetKeyChainUser" + KeyChain.BindGetKeyChainUser()+ "currentUUID= "+ currentUUID);
            //hasID = KeyChain.BindGetKeyChainUser();
            hasID = currentUUID;
        }

        
        Debug.Log("fetchuniqueIDforIOS device id" + hasID);
        
        //Debug.Log("fetchuniqueIDforIOS hasID " + KeyChain.BindGetKeyChainUser());
#else
        hasID  = SystemInfo.deviceUniqueIdentifier;
#endif
        return hasID;
    }



#if UNITY_EDITOR
    public static string deviceIdentifier = "DeviceOne3";
    //public static string deviceIdentifier = "DeviceOne2";
    //public static string deviceIdentifier = "c29cd31e546eb2691ac202bb85df2465";
    //public static string deviceIdentifier = "c6ca4678e0fe78b171559a233665b969";
    // public static string deviceIdentifier = "DeviceOne5";
    //public static string deviceIdentifier = "DeviceOne6";
    //public static string deviceIdentifier = "DeviceOne7";
    // public static string deviceIdentifier = "DeviceOne89";
    //public static string deviceIdentifier = "DeviceOne9";
    // public static string deviceIdentifier = "DeviceOne50";//"1380";
    // public static string deviceIdentifier = "1380";
    //public static string deviceIdentifier = "FB1224B1-A66A-4B72-825A-8264854A9673";
    //public static string deviceIdentifier = "f4232f7cb2ae25cd9d7b61ed8e5e3a9d";
    //public static string deviceIdentifier = "af32a39f3de8cbb0c958652033a3c8fa"; //kavita vivo
    //public static string deviceIdentifier = "Devicekav1234"; //playstorev1
    // public static string deviceIdentifier = "af32a39f3de8cbb0c958652033a3c8fa"; // Rajat
    //public static string deviceIdentifier = "DeviceR2jt"; // Rajat
    // public static string deviceIdentifier = "a0a1dca52ad554531f46aef17dbbe009"; // Guest 80
    //public static string deviceIdentifier = "DeviceTwo1997";
    //public static string deviceIdentifier = "661acd566fc2a11110b46bce8b20758c";
    //public static string deviceIdentifier = "1e0df92fbf1ccf3d3f623e465d997a37"; //kavita
    //public static string deviceIdentifier =  "62e2659319166f991275afa8642ba00a";//jagddish sir
    //public static string deviceIdentifier = "bbaf7dd5beb32bcec65e6e612823c413";//mustafa
    //public static string deviceIdentifier = "2e9737c70ec11ec704f89590f4c16a2d"; // varun
    //public static string deviceIdentifier = "661acd566fc2a11110b46bce8b20758c";
    //public static string deviceIdentifier = "DC426541-6BBF-4E1D-98E2-A5C798A909A9"; //shreyansh
    //"0de0dac006c86161fd9fce12b87f43e4";// sonali
    //public static string deviceIdentifier = "ac3aef7be12a7bf077f7002b08291aa6";//one plus kavita
    //public static string deviceIdentifier = "ba7f88b82c20062fc394e20425359bba"; //mustafa
    //public static string deviceIdentifier   = "B3B4E449-8808-487C-8926-F6145CCA2A87";
    //public static string deviceIdentifier = "DC426541-6BBF-4E1D-98E2-A5C798A909A9";



    //    if (GUILayout.Button ("UUID", ops)) {
    //			currentUUID = SystemInfo.deviceUniqueIdentifier;
    //			Debug.Log("CurrentUUID: [" + currentUUID + "]");
    //		}

    //GUILayout.TextArea(currentUUID, ops);

    //		if (GUILayout.Button ("Load UUID", ops)) {
    //			saveUUID = KeyChain.BindGetKeyChainUser();
    //			Debug.Log("LoadUUID: [" + saveUUID + "]");
    //		}

    //		GUILayout.TextArea(saveUUID, ops);

    //		if (GUILayout.Button ("Save UUID", ops)) {
    //			currentUUID = SystemInfo.deviceUniqueIdentifier;
    //			KeyChain.BindSetKeyChainUser("0", currentUUID);
    //			Debug.Log("SaveUUID: [" + currentUUID + "]");
    //		}

    //		if (GUILayout.Button ("Delete UUID", ops)) {
    //			KeyChain.BindDeleteKeyChainUser();
    //		}
#elif !UNITY_EDITOR && UNITY_IOS || UNITY_ANDROID
   //public static string deviceIdentifier = SystemInfo.deviceUniqueIdentifier;
    
    //public static string deviceIdentifier = "DeviceOne45688";
    //public static string deviceIdentifier = "DeviceOne1";

    
     public static string deviceIdentifier = fetchuniqueDeviceID();


#elif !UNITY_EDITOR
    //public static string deviceIdentifier = "DeviceFive";

#endif
    public const string apiSaveLogin = "save.login";
   
   
    public const string apiStadiumlist = "master.stadiumList";

    #region Card Property Vars

    public const string hit_points = "hit_points";
    public const string damage = "damage";
    public const string hit_speed = "hit_speed";
    public const string walk_speed = "walk_speed";
    public const string mana_cost = "mana_cost";
    public const string deploy_time = "deploy_time";
    public const string dice_count = "dice_count";
    public const string dice_spawn_time = "dice_spawn_time";
    public const string dice_damage = "dice_damage";
    public const string range_value = "range_value";
    public const string jump_distance = "jump_distance";
    public const string area_damage = "area_damage";
    public const string tower_damage = "tower_damage";
    public const string life_time = "life_time";
    public const string brahmos_damage = "brahmos_damage";
    public const string spawn_speed = "spawn_speed";
    public const string monkey_damage = "monkey_damage";
    public const string monkey_level = "monkey_level";
    public const string monkey_count = "monkey_count";
    public const string damage_per_second = "damage_per_second";
    public const string duration = "duration";
    public const string travel_distance = "travel_distance";
    public const string tower_dps = "tower_dps";


    #endregion

    #region ADDRESSABLES_LABELS
    public const string cardiconLabel = "cardicon";
    public const string stadiumimagesLabel = "stadiumimages";


    #endregion

}

public class PunPropertyName
{
    public const string PlayerReady = "player.ready";
}
