using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;
using System;

namespace RestAPIModule
{
    public struct RestAPIEntry
    {
        public string key { get; private set; }

        public string value { get; private set; }

        public RestAPIEntry(object key, object value)
        {
            this.key = key.ToString();
            this.value = value.ToString();
        }
    }

    public class RestAPIManager : MonoBehaviour
    {

        #region Inspector Variables

        private LoginData restLoginInfo;
        public UserInfo restUserInfo;
        public bool enableForceLogin = false;
        public QrdetailList qr_list;


        #endregion

        #region UnitySpecific

        public static RestAPIManager Instance;

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        void Start()
        {
            //if (enableForceLogin)
            //{
            //    DeviceLogin();
            //}
        }

        void OnEnable()
        {
            if (enableForceLogin)
            {
                Messenger<float>.AddListener(EventVariables.userLoginSuccess, (val) =>
                {
                    if (val > 0.45f)
                    {
                        Debug.Log("get card list_____1");
                        //GetCardList();
                    }
                    else if (val >= 0.25)
                    {
                        //GetMasterCardList();
                    }
                });
            }

            //Messenger<int, int>.AddListener(EventVariables.refreshPlayerInfo, (_gold, _crystal) =>
            //    {
            //        if (_gold >= 0)
            //            restUserInfo.total_gold = _gold;
            //        restUserInfo.total_crystal = _crystal;
            //    });
            //Messenger<int, int>.AddListener(EventVariables.refreshPlayerLevel, (_level, _xp) =>
            //    {
            //        restUserInfo.level_id = _level;
            //        restUserInfo.xp = _xp;
            //    });
            //Messenger<string>.AddListener(EventVariables.refreshPlayerName, (_name) =>
            //    {
            //        restUserInfo.name = _name;
            //    });
            //Messenger<int>.AddListener(EventVariables.refreshPlayerGender, (_status) =>
            //{
            //    restUserInfo.kingQueen_status = _status;
            //});

        }

        void OnDisable()
        {

        }

        //public int GetUserID()
        //{
        //    return restLoginInfo.user_id;
        //}


        ////public int GetKingdomID()
        ////{
        ////    return restUserInfo.kingdom_id;
        ////}

        //public bool UserLoggedIn
        //{
        //    get
        //    {
        //        return restLoginInfo != null;
        //    }
        //}

        //public bool GetCurrentUser
        //{
        //    get
        //    {
        //        return restUserInfo != null;
        //    }
        //}

        #endregion


        #region APICalls

        public void SendRequestGET(string apimethod, List<RestAPIEntry> entries, Action<string> success = null, Action failure = null)
        {

            HTTPRequest req = new HTTPRequest(new Uri(ServerVariables.baseApiURL + apimethod),
                                  HTTPMethods.Get, ((originalRequest, response) =>
                                  {
                                      if (response.StatusCode == 200)
                                      {
                                          if (success != null)
                                              success.Invoke(response.DataAsText);
                                      }
                                      else
                                      {
                                          if (failure != null)
                                              failure.Invoke();
                                      }
                                  }));

            for (int i = 0; i < entries.Count; i++)
            {
                req.AddField(entries[i].key, entries[i].value);
            }
            req.Send();
            Debug.Log("apimethod______2" + apimethod + "req" + req);
        }

        public void SendRequestPOST(string apimethod, List<RestAPIEntry> entries, Action<string> success = null, Action failure = null)
        {
            //Debug.Log("apimethod____2" + apimethod);
            try
            {
                HTTPRequest req = new HTTPRequest(new Uri(ServerVariables.baseApiURL + apimethod),
                                      HTTPMethods.Post, ((originalRequest, response) =>
                                      {


                                          if (response.StatusCode == 200)
                                          {
                                              if (success != null)

                                                  success.Invoke(response.DataAsText);
                                          }
                                          else
                                          {
                                              if (failure != null)
                                                  failure.Invoke();
                                          }
                                      }));

                Debug.Log("req = " + req + "apimethod" + apimethod);
                for (int i = 0; i < entries.Count; i++)
                {
                    req.AddField(entries[i].key, entries[i].value);
                }
                req.Send();

                Debug.Log("apimethod______3" + apimethod + "req" + req);
            }
            catch (Exception ex)
            {
                Debug.LogError("Caught at - " + apimethod);
                Debug.LogError("Exception is" + ex);
                if (failure != null)
                {
                    failure.Invoke();
                }
            }
        }

        public void SendRequestPOST<T>(string apimethod, List<RestAPIEntry> entries, Action<T> success = null, Action<int> failure = null) where T : ResponseMessage
        {

            HTTPRequest req = new HTTPRequest(new Uri(ServerVariables.baseApiURL + apimethod),
                                  HTTPMethods.Post, ((originalRequest, response) =>
                                  {
                                      if (response == null)
                                      {
                                          if (failure != null)
                                              failure.Invoke(400);
                                          return;
                                      }

                                      if (response.StatusCode == 200)
                                      {
                                          //Debug.Log("esponse.StatusCode" + response.DataAsText);
                                          GetResponse<T>(response.DataAsText, success, failure);
                                      }
                                      else
                                      {
                                          if (failure != null)
                                              failure.Invoke(response.StatusCode);
                                      }
                                  }));

            for (int i = 0; i < entries.Count; i++)
            {
                req.AddField(entries[i].key, entries[i].value);
            }
            req.Send();
            //Debug.Log("apimethod______3" + apimethod + "req" + req);
        }

        /// <summary>
        /// Gets the response and parses it as per the Type entered.
        /// </summary>
        /// <param name="jsonData">Json data in string format.</param>
        /// <param name="success">Success Callback.</param>
        /// <param name="failure">Failure Callback.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        private void GetResponse<T>(string jsonData, Action<T> success = null, Action<int> failure = null) where T : ResponseMessage
        {
            Debug.Log("jsonData__________" + jsonData);
            ServerResponse<T> _response = JsonUtility.FromJson<ServerResponse<T>>(jsonData);
            //Debug.Log(_response.responseCode + "_response.responseCode = " + _response.responseCode);
            if (_response.responseCode != ServerVariables.RESPONSE_SUCCESS)
            {
                if (failure != null)
                {
                    Debug.Log("failure" + failure + _response.responseCode);
                    failure.Invoke(_response.responseCode);
                }
            }
            else if (success != null)
            {
                //Debug.Log("success"+ success);
                //Debug.Log("_response" + _response.responseMsg.ToString());
                success.Invoke(_response.responseMsg);
            }
        }

        #endregion

        #region Test

        //[ContextMenu("Test User login")]
        //public void DeviceLogin(Action<int> failure = null)
        //{

        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //     entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("device_token", ServerVariables.deviceIdentifier);
        //    //Debug.Log("device_token" + ServerVariables.deviceIdentifier + "unique identifier" + SystemInfo.deviceUniqueIdentifier);


        //    SendRequestPOST<LoginData>(ServerVariables.apiUserLogin, entries, OnLoginCompleted, failure);

        //}

       
        #endregion



        public void Savelogindetail(string email, Action<QrdetailList> callback, Action<int> failure)
        {
            List<RestAPIEntry> entries = new List<RestAPIEntry>();
            entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
            entries.Add("user_id", restLoginInfo.user_id);
            entries.Add("access_token", restLoginInfo.access_token);
            entries.Add("email", email);

            Debug.Log("apiNotificationGet ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
               "user_id =" + restLoginInfo.user_id + "\n" +
               "access_token =" + restLoginInfo.access_token + "\n" +
               "last_notification_id =" + email
               );

            SendRequestPOST<QrdetailList>(ServerVariables.apiSaveLogin, entries, callback, failure);
        }


        public void Saveregisterdetail(string email, string name , Action<QrdetailList> callback, Action<int> failure)
        {
            List<RestAPIEntry> entries = new List<RestAPIEntry>();
            entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
            entries.Add("user_id", restLoginInfo.user_id);
            entries.Add("access_token", restLoginInfo.access_token);
            entries.Add("email", email);
            entries.Add("username", name);

            Debug.Log("apiNotificationGet ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
               "user_id =" + restLoginInfo.user_id + "\n" +
               "access_token =" + restLoginInfo.access_token + "\n" +
               "last_notification_id =" + email
               );

            SendRequestPOST<QrdetailList>(ServerVariables.apiSaveLogin, entries, callback, failure);
        }

        //#endregion
        //public void createroom(Action<UserInfo> callback, Action<int> failure)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    //entries.Add("lastroomid", restUserInfo.lastroomid);


        //    Debug.Log("createroom ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //     "user_id =" + restLoginInfo.user_id + "\n" +
        //     "access_token =" + restLoginInfo.access_token + "\n"

        //     //"room_id" + restUserInfo.lastroomid
        //     );

        //    SendRequestPOST<UserInfo>(ServerVariables.apicreateroom, entries, callback, failure);
        //}


        //public void Saveresult(string time, Action<UserInfo> callback, Action<int> failure)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("win_time", time);
        //    entries.Add("user_level", restUserInfo.level_id);


        //    Debug.Log("Saveresult ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //     "user_id =" + restLoginInfo.user_id + "\n" +
        //     "access_token =" + restLoginInfo.access_token + "\n" +
        //     "user_level=" + restUserInfo.level_id + "\n" +
        //     "win_time" + time
        //     ); ;

        //    SendRequestPOST<UserInfo>(ServerVariables.apisaveresult, entries, callback, failure);
        //}


        //public void Sendclue((string clueid, Action<GenericResponse> callback = null, Action<int> error = null)
        //{

        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("device_token", ServerVariables.deviceIdentifier);
        //    entries.Add("device_token", ServerVariables.deviceIdentifier);
        //    Debug.Log("device_token" + ServerVariables.deviceIdentifier + "unique identifier" + SystemInfo.deviceUniqueIdentifier);


        //    SendRequestPOST<GenericResponse>(ServerVariables.apiUserLogin, entries , error);

        //    //  Debug.Log("apiUserLogin ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //    //   "access_token =" + restLoginInfo.access_token
        //    //);
        //}

        //#region Notifications

        //public void GetNotification(int lastNotificationId, Action<NotificationResponse> callback, Action<int> failure)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("last_notification_id", lastNotificationId);

        //    Debug.Log("apiNotificationGet ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //       "user_id =" + restLoginInfo.user_id + "\n" +
        //       "access_token =" + restLoginInfo.access_token + "\n" +
        //       "last_notification_id =" + lastNotificationId
        //       );

        //    SendRequestPOST<NotificationResponse>(ServerVariables.apiNotificationGet, entries, callback, failure);
        //}
        //#endregion

        //public void UpdateUserName(string userName, Action<UserInfo> callback, Action<int> failure)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("name", userName);
        //    //entries.Add("editname_count", editname_count);

        //    Debug.Log("apiUserUpdate ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //         "user_id =" + restLoginInfo.user_id + "\n" +
        //         "access_token =" + restLoginInfo.access_token + "\n" +
        //         "name =" + userName
        //   );

        //    SendRequestPOST<UserInfo>(ServerVariables.apiUserUpdate, entries, callback, failure);
        //}


        //public void GetUserDetails()
        //{
        //    //restLoginInfo.user_id = 5;
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);

        //    Debug.Log("apiGetUser ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //       "user_id =" + restLoginInfo.user_id + "\n" +
        //       "access_token =" + restLoginInfo.access_token
        //    );


        //    SendRequestPOST(ServerVariables.apiGetUser, entries, (message) =>
        //    {
        //        GetResponse<UserInfo>(message, (responseData) =>
        //        {
        //            restUserInfo = responseData;
        //            //if (string.IsNullOrEmpty(restUserInfo.name))
        //            //{
        //            //    restUserInfo.name = "Guest " + restLoginInfo.user_id; //"You";
        //            //}



        //            Messenger<float>.Broadcast(EventVariables.userLoginSuccess, 0.5f, MessengerMode.DONT_REQUIRE_LISTENER);
        //            //Game.SetCrystals(restUserInfo.total_crystal);
        //            //Game.SetGold(restUserInfo.total_gold);

        //            //if (restUserInfo.is_tutorial_completed == 2)
        //            //{
        //            //    restUserInfo.is_tutorial_completed = 1;
        //            //}
        //            //Game.SetTutorialStatus(restUserInfo.is_tutorial_completed);


        //            //Debug.Log(restUserInfo.is_tutorial_completed + "=//////");
        //        });
        //    });
        //}


        //public void GetUserLevelupdateDetails()
        //{
        //    Debug.Log("GetUserLevelupdateDetails_____________________-");
        //    //restLoginInfo.user_id = 5;
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("level_up", 1);

        //    Debug.Log("apiGetUser ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //       "user_id =" + restLoginInfo.user_id + "\n" +
        //       "access_token =" + restLoginInfo.access_token
        //    );


        //    SendRequestPOST(ServerVariables.apiGetUser, entries, (message) =>
        //    {
        //        GetResponse<UserInfo>(message, (responseData) =>
        //        {
        //            restUserInfo = responseData;
        //            if (string.IsNullOrEmpty(restUserInfo.name))
        //            {
        //                restUserInfo.name = "You";// "Guest " + restLoginInfo.user_id;
        //            }

        //            //if (PlayfabManager.PFM.useplayfab == true)
        //            //{

        //            //    //kavita
        //            //    if (PlayfabManager.PFM.loginsucesss == true)
        //            //    {
        //            //        PlayfabManager.PFM.setusername(restUserInfo.name);
        //            //    }
        //            //    //kavita
        //            //}

        //            //Messenger<float>.Broadcast(EventVariables.userLoginSuccess, 0.5f, MessengerMode.DONT_REQUIRE_LISTENER);
        //            //Game.SetCrystals(restUserInfo.total_crystal);
        //            //Game.SetGold(restUserInfo.total_gold);
        //            //kavita change for build
        //            ////PlayerPrefs.SetInt("tutorial", 0);
        //            //if (PlayerPrefs.GetInt("tutorial") == 0)
        //            //{
        //            //    Debug.Log("tutorial pending");
        //            //    restUserInfo.is_tutorial_completed = 2;
        //            //}


        //            //Game.SetTutorialStatus(restUserInfo.is_tutorial_completed);

        //            //Debug.Log(restUserInfo.is_tutorial_completed + "=//////");
        //        });
        //    });
        //}



        //        public void UpdateAccessToken(string push_token, Action<GenericResponse> callback = null, Action<int> error = null)
        //        {
        //            List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //            entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //            entries.Add("user_id", restLoginInfo.user_id);
        //            entries.Add("access_token", restLoginInfo.access_token);
        //#if UNITY_IOS
        //            entries.Add("ios_push_token", push_token);
        //#elif UNITY_ANDROID
        //            entries.Add("android_push_token", push_token);
        //#endif

        //            Debug.Log("apiUserUpdate ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //           "user_id =" + restLoginInfo.user_id + "\n" +
        //            "ios_push_token =" + push_token + "\n" +
        //           "access_token =" + restLoginInfo.access_token
        //        );

        //            SendRequestPOST(ServerVariables.apiUserUpdate, entries, callback, error);
        //        }


        //public void UpdateTutorialComplete(int is_tutorial_completed, Action<AchievementList> callback = null, Action<int> error = null)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("is_tutorial_completed", is_tutorial_completed);

        //    Debug.Log("apiUserUpdate ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //    "user_id =" + restLoginInfo.user_id + "\n" +
        //    "access_token =" + restLoginInfo.access_token + "\n" +
        //    "is_tutorial_completed =" + is_tutorial_completed
        // );

        //    SendRequestPOST(ServerVariables.apiUserUpdate, entries, callback, error);
        //}


        //[ContextMenu("Get cards")]
        //public void GetCardList()
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);

        //    Debug.Log("apiCardList ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //    "user_id =" + restLoginInfo.user_id + "\n" +
        //    "access_token =" + restLoginInfo.access_token
        // );

        //    SendRequestPOST(ServerVariables.apiCardList, entries, (message) =>
        //        {
        //            GetResponse<UserCardList>(message, (responseData) =>
        //                {
        //                    //foreach (CardInfo card in responseData.user_card_list)
        //                    //{
        //                    //    //if (card.title.Equals("Vira")){

        //                    //    //    foreach (ServerCardProperty property in card.property_list)
        //                    //    //    {
        //                    //    //        Debug.Log("card_property == " + property.property_name);
        //                    //    //    }

        //                    //    //}
        //                    //}

        //                    //cardList = responseData.user_card_list;
        //                    //TODO
        //                    //                            for (int i = 0; i < 3; i++) 
        //                    //                            {
        //                    //                                CardInfo t=cardList[cardList.Count-1];
        //                    //                                cardList.Remove(t);
        //                    //                            }
        //                });
        //        });
        //}

        //public void GetDailyCards(Action<DailyCardsList> success, Action<int> failure)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);

        //    Debug.Log("apiDailyCardList ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //      "user_id =" + restLoginInfo.user_id + "\n" +
        //      "access_token =" + restLoginInfo.access_token
        //   );

        //    SendRequestPOST<DailyCardsList>(ServerVariables.apiDailyCardList, entries, success, failure);
        //}

        //public void PurchaseDailyCards(int masterCardId, int count, Action<GenericResponse> success, Action<int> failure)
        //{

        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("master_card_id", masterCardId);
        //    entries.Add("count", count);

        //    Debug.Log("apiCardPurchase ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //     "user_id =" + restLoginInfo.user_id + "\n" +
        //     "access_token =" + restLoginInfo.access_token + "\n" +
        //      "master_card_id =" + masterCardId + "\n" +
        //       "count =" + count
        //  );


        //    SendRequestPOST(ServerVariables.apiCardPurchase, entries, success, failure);

        //}

        //public void GetDailyRewards(Action<DailyRewardResponse> success, Action<int> failure)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);

        //    Debug.Log("apiDailyRewardList ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //       "user_id =" + restLoginInfo.user_id + "\n" +
        //       "access_token =" + restLoginInfo.access_token
        //    );

        //    SendRequestPOST<DailyRewardResponse>(ServerVariables.apiDailyRewardList, entries, success, failure);
        //}



        //public void UpdatekingstatusName(int kingQueen_status, Action<ResponseMessage> callback = null)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("kingQueen_status", kingQueen_status);
        //    //entries.Add("editname_count", editname_count);

        //    Debug.Log("apiUserUpdate ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //         "user_id =" + restLoginInfo.user_id + "\n" +
        //         "access_token =" + restLoginInfo.access_token + "\n" +
        //         "kingQueen_status =" + kingQueen_status
        //   );

        //    SendRequestPOST(ServerVariables.apiUserUpdate, entries, callback);
        //}


        //#endregion


        //#region ServerRoom

        //[ContextMenu("Room.create")]
        //public void ConnectToRoom()
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);


        //    Debug.Log("apiRoomCreate ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //           "user_id =" + restLoginInfo.user_id + "\n" +
        //           "access_token =" + restLoginInfo.access_token
        //     );

        //    SendRequestPOST(ServerVariables.apiRoomCreate, entries, (message) =>
        //        {
        //            GetResponse<WaitingRoomInfo>(message, (responseData) =>
        //                {
        //                    Debug.Log(responseData.waiting_room_id);
        //                });
        //        });
        //}

        //public void ConnectToRoom(Action<WaitingRoomInfo> callback)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    //   Debug.Log("Connect to room called.");

        //    Debug.Log("apiRoomCreate ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //           "user_id =" + restLoginInfo.user_id + "\n" +
        //           "access_token =" + restLoginInfo.access_token
        //     );

        //    SendRequestPOST<WaitingRoomInfo>(ServerVariables.apiRoomCreate, entries, callback);
        //}

        //[ContextMenu("Room.GetDetails")]
        //public void FetchRoomState()
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("waiting_room_id", "39");

        //    Debug.Log("apiRoomDetail ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //           "user_id =" + restLoginInfo.user_id + "\n" +
        //           "access_token =" + restLoginInfo.access_token + "\n" +
        //           "waiting_room_id =" + "39"
        //     );


        //    SendRequestPOST(ServerVariables.apiRoomDetail, entries, (message) =>
        //        {
        //            GetResponse<CurrentRoomInfo>(message, (responseData) =>
        //                {

        //                    Debug.Log(responseData.users);
        //                });
        //        });
        //}

        //public void FetchRoomState(int waiting_room_id, int query_type, Action<CurrentRoomInfo> callback, Action<int> failure = null)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("waiting_room_id", waiting_room_id);
        //    entries.Add("type", query_type);

        //    Debug.Log("apiRoomDetail ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //           "user_id =" + restLoginInfo.user_id + "\n" +
        //           "access_token =" + restLoginInfo.access_token + "\n" +
        //           "waiting_room_id =" + waiting_room_id + "\n" +
        //           "type =" + query_type
        //     );

        //    SendRequestPOST(ServerVariables.apiRoomDetail, entries, callback, failure);
        //}



        //public void InvitePlayer(Action<InviteDetails> callback, Action<int> failure)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);

        //    Debug.Log("apiCreateInvite ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //     "user_id =" + restLoginInfo.user_id + "\n" +
        //     "access_token =" + restLoginInfo.access_token
        //     );

        //    SendRequestPOST(ServerVariables.apiCreateInvite, entries, callback, failure);
        //}

        //public void ConnectToRoom(int roomType, string inviteToken, Action<WaitingRoomInfo> callback, Action<int> failure)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("room_type", roomType);
        //    entries.Add("invite_token", inviteToken);

        //    Debug.Log("apiRoomCreate ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //     "user_id =" + restLoginInfo.user_id + "\n" +
        //     "access_token =" + restLoginInfo.access_token + "\n" +
        //     "room_type =" + roomType + "\n" +
        //     "invite_token =" + inviteToken
        //     );

        //    SendRequestPOST<WaitingRoomInfo>(ServerVariables.apiRoomCreate, entries, callback, failure);
        //}



        //public void SaveRoomResult(int room_id, int win_status, int circlets_count, int opponent_circlets_count, int opponent_id, int battle_opp_id, Action<GameResult> callback)
        //{
        //    Debug.Log("submit result =  " + room_id + "win_status = " + win_status + "circlets_count = " + circlets_count + "opponent_id" + opponent_id);
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("room_id", room_id);
        //    entries.Add("win_status", win_status);
        //    entries.Add("circlet", circlets_count);
        //entries.Add("opponent_id", opponent_id);
        //entries.Add("opponent_circlet", opponent_circlets_count);
        //entries.Add("opponent_circlet", opponent_circlets_count);
        //entries.Add("battle_opp_id", battle_opp_id);

        //For testing purpose only.
        //            SendRequestPOST(ServerVariables.apiRoomResult, entries, (jsonData) =>
        //                {
        //                    Debug.Log(jsonData);
        //
        //                    GetResponse<GameResult>(jsonData, (responseData) =>
        //                        {
        //                            ScreenManager.Instance.GetScreen<GameOverScreen>().DisplayGameResult(responseData);
        //                        });
        //                });

        //Debug.Log("apiRoomResult ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //        "user_id =" + restLoginInfo.user_id + "\n" +
        //        "access_token =" + restLoginInfo.access_token + "\n" +
        //        "room_id =" + room_id + "\n" +
        //        "win_status =" + win_status + "\n" +
        //        "circlet =" + circlets_count + "\n" +
        //        "opponent_id =" + opponent_id + "\n" +
        //        "battle_opp_id =" + battle_opp_id + "\n" +
        //            "opponent_circlet =" + opponent_circlets_count
        //      ); ;

        //    SendRequestPOST<GameResult>(ServerVariables.apiRoomResult, entries, callback);
        //}

        //#endregion



        //public void Receivechat(int last_msg_id, Action<Listofchat> callback, Action<int> failure = null)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("last_msg_id", last_msg_id);
        //    entries.Add("kingdom_id", restUserInfo.kingdom_id);
        //    //entries.Add("type", query_type);

        //    Debug.Log("Receivechat ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //           "user_id =" + restLoginInfo.user_id + "\n" +
        //           "access_token =" + restLoginInfo.access_token + "\n" +
        //           "lastchatid =" + last_msg_id + "\n" +
        //            "kingdom_id =" + restUserInfo.kingdom_id + "\n"

        //     );

        //    SendRequestPOST(ServerVariables.apiKingdomreceive, entries, callback, failure);
        //}

        //public void Sendchat(Kingdomchat chat , Action<Kingdomsendresponce> success, Action<int> failure = null)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    if (chat.kingdom_msg_type == 1)
        //    {
        //        entries.Add("kingdom_chat_type", chat.kingdom_chat_type);
        //        entries.Add("kingdom_msg_type", chat.kingdom_msg_type);
        //        entries.Add("sender_id", chat.sent_by_id);
        //        entries.Add("kingdom_msg", chat.message);
        //        //entries.Add("kingdom_id", restUserInfo.kingdom_id);
        //    }

        //    //entries.Add("type", query_type);

        //    Debug.Log("apiKingdomsend ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //           "user_id =" + restLoginInfo.user_id + "\n" +
        //           "access_token =" + restLoginInfo.access_token + "\n" +
        //           "kingdom_msg =" + chat.message + "\n"+
        //            "kingdom_chat_type =" + chat.kingdom_chat_type + "\n" +
        //             "msg_type =" + chat.kingdom_msg_type + "\n" +
        //               "sent_by_id =" + chat.sent_by_id + "\n" 
        //     );

        //    SendRequestPOST(ServerVariables.apiKingdomsend, entries, success, failure);
        //}

        //Added by Avinash

        /// <summary>
        /// Saves the game result.
        /// </summary>
        /// <param name="room_id">Room identifier.</param>
        /// <param name="win_status">Window status.</param>-
        /// <param name="circlets_count">Circlets count.</param>_
        ///


        //#region Rewards

        //public void GetGameplayRewards(Action<UserRewards> callback, int user_reward_id = 0)
        //{
        //    GetRewardList(user_reward_id, (int)ServerVariables.RewardType.CUBE_MATCH, callback);
        //}

        //private void GetRewardList(int user_reward_id, int user_reward_type, Action<UserRewards> callback)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("user_reward_id", user_reward_id);
        //    entries.Add("user_reward_type", user_reward_type);

        //    Debug.Log("apiClaimReward ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //            "user_id =" + restLoginInfo.user_id + "\n" +
        //            "access_token =" + restLoginInfo.access_token + "\n" +
        //            "user_reward_id =" + user_reward_id + "\n" +
        //            "user_reward_type =" + user_reward_type
        //      );

        //    SendRequestPOST<UserRewards>(ServerVariables.apiRewardList, entries, callback);
        //}

        //public void ClaimRewards(int user_reward_id, Action<RewardInfo> callback, bool inprocess)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("user_reward_id", user_reward_id);
        //    entries.Add("claim_reward", 1);

        //    if (inprocess == true)
        //    {
        //        Debug.Log("inprocess cube_upgrade_id send ");
        //        entries.Add("cube_upgrade_id", 3);
        //    }



        //    Debug.Log("apiClaimReward ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //              "access_token =" + restLoginInfo.access_token + "\n" +
        //              "user_reward_id =" + user_reward_id + "\n" +
        //              "claim_reward =" + 1

        //        );

        //    SendRequestPOST<RewardInfo>(ServerVariables.apiClaimReward, entries, callback);
        //}

        //public void ClaimKathika(int kathika_id, Action<ClaimKathika> callback)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("kathika_id", kathika_id);




        //    Debug.Log("apiClaimReward ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //              "access_token =" + restLoginInfo.access_token + "\n" +
        //              "kathika_id =" + kathika_id + "\n" +
        //              "claim_reward =" + 1

        //        );

        //    SendRequestPOST<ClaimKathika>(ServerVariables.apiClaimKathika, entries, callback);
        //}


        //#region ARDATA
        //public void GetARdataDetails(Action<ARdata> callback, Action<int> failure)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);

        //    Debug.Log("ar.characterData ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //     "user_id =" + restLoginInfo.user_id + "\n" +
        //     "access_token =" + restLoginInfo.access_token
        //     );


        //    SendRequestPOST<ARdata>(ServerVariables.apiGetARdeatails, entries, callback, failure);

        //}


        //public void ClaimModel(int Model_Id, Action<ClaimModel> callback ,Action<int> failedCallback)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("character_id", Model_Id);

        //    Debug.Log("apiClaimcharacter ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //              "access_token =" + restLoginInfo.access_token + "\n" +
        //              "character_id =" + Model_Id 


        //        );

        //    SendRequestPOST<ClaimModel>(ServerVariables.apiClaimModel, entries, callback , failedCallback);
        //}
        //#endregion

        //public void GetTimedBonus(int user_reward_type, Action<RewardInfo> callback)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("user_reward_type", user_reward_type);


        //    Debug.Log("apiTimedBonus ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //            "user_id =" + restLoginInfo.user_id + "\n" +
        //            "access_token =" + restLoginInfo.access_token + "\n" +
        //            "user_reward_type =" + user_reward_type
        //      );

        //    SendRequestPOST<RewardInfo>(ServerVariables.apiTimedBonus, entries, callback);
        //}

        //#endregion

        //#region Cards

        //public void GetMasterCardList()
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);

        //    Debug.Log("apiMasterCardList ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //           "user_id =" + restLoginInfo.user_id + "\n" +
        //           "access_token =" + restLoginInfo.access_token
        //     );


        //    SendRequestPOST(ServerVariables.apiMasterCardList, entries, (message) =>
        //        {
        //            //                    Debug.Log(message);
        //            GetResponse<UserMasterList>(message, (responseData) =>
        //                {
        //                    Debug.Log("responseData.card_list" + responseData.card_list.ToString());
        //                    MasterDataHolder.Instance.master_card_list = responseData.card_list;
        //                    //TODO
        //                    //                            for (int i = 0; i < 3; i++) 
        //                    //                            {
        //                    //                                CardInfo t=MasterDataHolder.Instance.master_card_list[MasterDataHolder.Instance.master_card_list.Count-1];
        //                    //                                MasterDataHolder.Instance.master_card_list.Remove(t);
        //                    //                            }


        //                });
        //        });
        //}

        //public void AddCardToDeck(int master_card_id, int replace_with_card, Action<CardID> callback, Action<int> failedCallback)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("master_card_id", master_card_id);
        //    entries.Add("replace_with_card", replace_with_card);

        //    Debug.Log("apiAddCardToDeck ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //          "user_id =" + restLoginInfo.user_id + "\n" +
        //          "access_token =" + restLoginInfo.access_token + "\n" +
        //          "master_card_id =" + master_card_id + "\n" +
        //          "replace_with_card =" + replace_with_card
        //    );

        //    SendRequestPOST<CardID>(ServerVariables.apiAddCardToDeck, entries, callback, failedCallback);
        //}

        //public void LevelUpCard(int master_card_id, Action<CardLevelUpResponse> callback)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("master_card_id", master_card_id);
        //    entries.Add("is_level_up", 1);


        //    Debug.Log("apiCardLevelUp ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //        "user_id =" + restLoginInfo.user_id + "\n" +
        //        "access_token =" + restLoginInfo.access_token + "\n" +
        //        "master_card_id =" + master_card_id + "\n" +
        //        "is_level_up =" + 1
        //   );

        //    SendRequestPOST<CardLevelUpResponse>(ServerVariables.apiCardLevelUp, entries, callback);
        //}


        //public void UserLevelUp(int master_card_id, Action<GodLevelupInfo> callback)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("master_card_id", master_card_id);
        //    entries.Add("level_up", 1);


        //    Debug.Log("apiCardLevelUp ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //        "user_id =" + restLoginInfo.user_id + "\n" +
        //        "access_token =" + restLoginInfo.access_token + "\n" +
        //        "master_card_id =" + master_card_id + "\n" +
        //        "is_level_up =" + 1
        //   );

        //    SendRequestPOST<CardLevelUpResponse>(ServerVariables.apiCardLevelUp, entries, callback);
        //}

        //public void UnloackAllCards(Action<ResponseMessage> callback)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);

        //    Debug.Log("unloackAllCards ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //       "user_id =" + restLoginInfo.user_id + "\n" +
        //       "access_token =" + restLoginInfo.access_token
        //  );

        //    SendRequestPOST<ResponseMessage>(ServerVariables.unloackAllCards, entries, callback);
        //}

        //#endregion

        //#region Purchase Cube

        //        public void GetPurchasableCubes(Action<CubeInventoryResponse> success)
        //        {
        //            List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //            entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //            entries.Add("user_id", restLoginInfo.user_id);
        //            entries.Add("access_token", restLoginInfo.access_token);
        //            SendRequestPOST<CubeInventoryResponse>(ServerVariables.apiCubeList, entries, success);
        //        }

        // public void PurchaseCube(int master_cube_inventory_id, Action<RewardInfo> success, Action<int> failure)
        // {
        //     List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //     entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //     entries.Add("user_id", restLoginInfo.user_id);
        //     entries.Add("access_token", restLoginInfo.access_token);
        //     entries.Add("master_cube_inventory_id", master_cube_inventory_id);

        //     Debug.Log("apiCubePurchase ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //     "user_id =" + restLoginInfo.user_id + "\n" +
        //     "access_token =" + restLoginInfo.access_token + "\n" +
        //     "master_cube_inventory_id =" + master_cube_inventory_id
        //);

        //     SendRequestPOST<RewardInfo>(ServerVariables.apiCubePurchase, entries, success, failure);
        // }

        // public void GetCubeInventoryList(Action<CubeInventoryResponse> success, Action<int> failure = null)
        // {
        //     List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //     entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //     entries.Add("user_id", restLoginInfo.user_id);
        //     entries.Add("access_token", restLoginInfo.access_token);

        //     Debug.Log("apiCubeList ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //     "user_id =" + restLoginInfo.user_id + "\n" +
        //     "access_token =" + restLoginInfo.access_token
        //     );

        //     SendRequestPOST<CubeInventoryResponse>(ServerVariables.apiCubeList, entries, success, failure);
        // }

        // #endregion

        // #region Purchase Gold

        // public void GetGoldPurchaseList(Action<CubeInventoryResponse> success)
        // {
        //     List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //     entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //     entries.Add("user_id", restLoginInfo.user_id);
        //     entries.Add("access_token", restLoginInfo.access_token);

        //     Debug.Log("apiGoldList ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //    "user_id =" + restLoginInfo.user_id + "\n" +
        //    "access_token =" + restLoginInfo.access_token
        // );
        //     SendRequestPOST<CubeInventoryResponse>(ServerVariables.apiGoldList, entries, success);
        // }

        // public void PurchaseGold(int master_gold_inventory_id, Action<RewardInfo> success, Action<int> failure)
        // {
        //     List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //     entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //     entries.Add("user_id", restLoginInfo.user_id);
        //     entries.Add("access_token", restLoginInfo.access_token);
        //     entries.Add("master_gold_inventory_id", master_gold_inventory_id);

        //     Debug.Log("apiGoldPurchase ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //     "user_id =" + restLoginInfo.user_id + "\n" +
        //     "access_token =" + restLoginInfo.access_token + "\n" +
        //     "master_gold_inventory_id =" + master_gold_inventory_id
        //     );

        //     SendRequestPOST<RewardInfo>(ServerVariables.apiGoldPurchase, entries, success, failure);
        // }

        // #endregion


        // #region IAP Sync with server

        // public void PurchaseCrystals(int crystal, Action<RewardInfo> success, Action<int> failure)
        // {
        //     List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //     entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //     entries.Add("user_id", restLoginInfo.user_id);
        //     entries.Add("access_token", restLoginInfo.access_token);
        //     entries.Add("crystal", crystal);
        //     entries.Add("data", "blek");

        //     Debug.Log("apiCrystalPurchase ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //    "user_id =" + restLoginInfo.user_id + "\n" +
        //    "access_token =" + restLoginInfo.access_token + "\n" +
        //    "crystal =" + crystal + "\n" +
        //    "data =" + "blek"
        //    );

        //     SendRequestPOST<RewardInfo>(ServerVariables.apiCrystalPurchase, entries, success, failure);
        // }

        // public void ClaimDailyRewards(int dailyRewardId, Action<DailyRewardResponse> success, Action<int> failure)
        // {
        //     List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //     entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //     entries.Add("user_id", restLoginInfo.user_id);
        //     entries.Add("access_token", restLoginInfo.access_token);
        //     entries.Add("daily_reward_id", dailyRewardId);

        //     Debug.Log("apiDailyRewardClaim ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //     "user_id =" + restLoginInfo.user_id + "\n" +
        //     "access_token =" + restLoginInfo.access_token + "\n" +
        //     "daily_reward_id =" + dailyRewardId
        //    );

        //     SendRequestPOST<DailyRewardResponse>(ServerVariables.apiDailyRewardClaim, entries, success, failure);
        // }

        // #endregion

        // #region AchievementList

        // public void GetAchievementList(Action<AchievementResponse> callback, Action<int> failure = null)
        // {
        //     List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //     entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //     entries.Add("user_id", restLoginInfo.user_id);
        //     entries.Add("access_token", restLoginInfo.access_token);

        //     Debug.Log("apiAchievementList ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //     "user_id =" + restLoginInfo.user_id + "\n" +
        //     "access_token =" + restLoginInfo.access_token
        //     );

        //     SendRequestPOST<AchievementResponse>(ServerVariables.apiAchievementList, entries, callback, failure);
        // }

        // #endregion


        //#region Leaderboard

        //[ContextMenu("Get leaderboard")]

        //public void GetLeaderboardList(Action<LeaderboardList> success, Action<int> failure)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);

        //    Debug.Log("apiLeaderboardist ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //      "user_id =" + restLoginInfo.user_id + "\n" +
        //      "access_token =" + restLoginInfo.access_token
        //   );

        //    SendRequestPOST<LeaderboardList>(ServerVariables.apiLeaderboardist, entries, success, failure);
        //}
        //#endregion

        //#region GodLevelList

        //public void GetGodLevelList(Action<MasterLevelUpResponse> callback)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);

        //    Debug.Log("apiLevelupList ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //   "user_id =" + restLoginInfo.user_id + "\n" +
        //   "access_token =" + restLoginInfo.access_token
        //   );

        //    SendRequestPOST<MasterLevelUpResponse>(ServerVariables.apiLevelupList, entries, callback);
        //}

        //#endregion

        //#region RewardedAdsCallback

        //public void SyncCrystalsWithServer(int crystal, Action<RewardAdsResponse> callback)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("crystal", crystal);

        //    Debug.Log("apiUserAddCrystal ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //    "user_id =" + restLoginInfo.user_id + "\n" +
        //    "access_token =" + restLoginInfo.access_token + "\n" +
        //    "crystal =" + crystal
        //    );

        //    SendRequestPOST<RewardAdsResponse>(ServerVariables.apiUserAddCrystal, entries, callback);
        //}

        //#endregion




        //public void GetBadgeMasterList(Action<AllBadgeInfoList> callback, Action<int> failure)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);

        //    Debug.Log("apiGetBadgesMasterList ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //      "user_id =" + restLoginInfo.user_id + "\n" +
        //      "access_token =" + restLoginInfo.access_token
        //      );

        //    SendRequestPOST<AllBadgeInfoList>(ServerVariables.apiGetBadgesMasterList, entries, callback, failure);
        //}

        //#region KathikaList
        //public void GetKathikaDetails(Action<kathikalist> callback, Action<int> failure)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);

        //    Debug.Log("apiGetDeckDetails ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //     "user_id =" + restLoginInfo.user_id + "\n" +
        //     "access_token =" + restLoginInfo.access_token
        //     );


        //    SendRequestPOST<kathikalist>(ServerVariables.apiGetKathikaDetails, entries, callback, failure);

        //}
        //#endregion

        //public void GetDeckDetails(Action<DeckDetailList> callback, Action<int> failure)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);

        //    Debug.Log("apiGetDeckDetails ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //     "user_id =" + restLoginInfo.user_id + "\n" +
        //     "access_token =" + restLoginInfo.access_token
        //     );


        //    SendRequestPOST<DeckDetailList>(ServerVariables.apiGetDeckDetails, entries, callback, failure);

        //}


        //public void SaveDeckdetails(string deckData, Action<GenericResponse> callback, Action<int> failure)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("deck_data", deckData);

        //    Debug.Log("apiSaveDeckDetails ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //     "user_id =" + restLoginInfo.user_id + "\n" +
        //     "access_token =" + restLoginInfo.access_token + "\n" +
        //     "deck_data =" + deckData
        //     );

        //    SendRequestPOST(ServerVariables.apiSaveDeckDetails, entries, callback, failure);
        //}

        //public void Createkingdom(string king_name, string king_disc, int shiel_dID, int req_cup_amt, string King_location, int King_type, Action<Kingdomresponce> success, Action<int> failure)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("kingdom_name", king_name);
        //    entries.Add("kingdom_desc", king_disc);
        //    entries.Add("kingdom_shield_id", shiel_dID);
        //    entries.Add("kingdom_req_cups", req_cup_amt);
        //    entries.Add("kingdom_location", King_location);
        //    entries.Add("kingdom_limit", 50);
        //    entries.Add("kingdom_type", King_type);
        //    //kingdom_limit
        //    // kingdom_type

        //    Debug.Log("Createkingdom ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //   "user_id =" + restLoginInfo.user_id + "\n" +
        //   "access_token =" + restLoginInfo.access_token + "\n" +
        //   "kingdom_shield_id =" + shiel_dID + "\n" +
        //   "kingdom_req_cup_amt =" + req_cup_amt + "\n" +
        //   "King_type =" + King_type
        //   );

        //    SendRequestPOST<Kingdomresponce>(ServerVariables.apiCreateKingdom, entries, success, failure);

        //}


        //public void UpdateKingdom(int kingdom_id, string king_name, string king_disc, int shiel_dID, int req_cup_amt, string King_location, int King_type, Action<Kingdomresponce> success, Action<int> failure)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("kingdom_id", kingdom_id);
        //    entries.Add("kingdom_name", king_name);
        //    entries.Add("kingdom_desc", king_disc);
        //    entries.Add("kingdom_shield_id", shiel_dID);
        //    entries.Add("kingdom_req_cups", req_cup_amt);
        //    entries.Add("kingdom_location", King_location);
        //    entries.Add("kingdom_limit", 50);
        //    entries.Add("kingdom_type", King_type);
        //    //kingdom_limit
        //    // kingdom_type

        //    Debug.Log("Update ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //   "user_id =" + restLoginInfo.user_id + "\n" +
        //   "access_token =" + restLoginInfo.access_token + "\n" +
        //   "kingdom_shield_id =" + shiel_dID + "\n" +
        //   "kingdom_req_cup_amt =" + req_cup_amt + "\n" +
        //   "King_type =" + King_type
        //   );

        //    SendRequestPOST<Kingdomresponce>(ServerVariables.apiUpdateKingdom, entries, success, failure);

        //}


        //public void GetKingdomdata(int Kingdom_ID, Action<Kingdomresponce> success, Action<int> failure)
        //{

        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("kingdom_id", Kingdom_ID);


        //    Debug.Log("GetKingdomdata ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //     "user_id =" + restLoginInfo.user_id + "\n" +
        //     "access_token =" + restLoginInfo.access_token + "\n" +
        //     "Kingdom_ID =" + Kingdom_ID + "\n"

        //     );

        //    SendRequestPOST<Kingdomresponce>(ServerVariables.apiGetKingdomIfo, entries, success, failure);


        //}

        //public void JoinKingdom(int Kingdom_ID, Action<Kingdomresponce> success, Action<int> failure)
        //{

        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("kingdom_id", Kingdom_ID);


        //    Debug.Log("apiRoomCreate ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //     "user_id =" + restLoginInfo.user_id + "\n" +
        //     "access_token =" + restLoginInfo.access_token + "\n" +
        //     "Kingdom_ID =" + Kingdom_ID + "\n"

        //     );

        //    SendRequestPOST<Kingdomresponce>(ServerVariables.apiJoinKingdom, entries, success, failure);


        //}

        //public void AcceptKingdom(int accept_user_id, int msg_id, Action<Kingdomresponce> success, Action<int> failure)
        //{

        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("accept_user_id", accept_user_id);
        //    entries.Add("msg_id", msg_id);


        //    Debug.Log("AcceptKingdom ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //     "user_id =" + restLoginInfo.user_id + "\n" +
        //     "access_token =" + restLoginInfo.access_token + "\n" +
        //     "accept_user_id =" + accept_user_id + "\n"+
        //     "msg_id =" + msg_id + "\n"
        //     );

        //    SendRequestPOST<Kingdomresponce>(ServerVariables.apiAcceptKingdom, entries, success, failure);


        //}


        //public void RejectKingdom(int reject_user_id , int msg_id, Action<Kingdomresponce> success, Action<int> failure)
        //{

        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("reject_user_id", reject_user_id);
        //    entries.Add("msg_id", msg_id);


        //    Debug.Log("RejectKingdom ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //     "user_id =" + restLoginInfo.user_id + "\n" +
        //     "access_token =" + restLoginInfo.access_token + "\n" +
        //     "reject_user_id =" + reject_user_id + "\n"+
        //    "msg_id =" + msg_id + "\n"
        //     );

        //    SendRequestPOST<Kingdomresponce>(ServerVariables.apiRejectKingdom, entries, success, failure);


        //}



        //public void LeaveKingdom(int Kingdom_ID, Action<Kingdomresponce> success, Action<int> failure)
        //{

        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    //entries.Add("kingdom_id", Kingdom_ID);


        //    Debug.Log("LeaveKingdom ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //     "user_id =" + restLoginInfo.user_id + "\n" +
        //     "access_token =" + restLoginInfo.access_token + "\n" +
        //     "Kingdom_ID =" + Kingdom_ID + "\n"

        //     );

        //    SendRequestPOST<Kingdomresponce>(ServerVariables.apiLeaveKingdom, entries, success, failure);


        //}


        //public void RequestKingdom(int Kingdom_ID, Action<Kingdomresponce> success, Action<int> failure)
        //{

        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("kingdom_id", Kingdom_ID);


        //    Debug.Log("RequestKingdom ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //     "user_id =" + restLoginInfo.user_id + "\n" +
        //     "access_token =" + restLoginInfo.access_token + "\n" +
        //     "Kingdom_ID =" + Kingdom_ID + "\n"

        //     );

        //    SendRequestPOST<Kingdomresponce>(ServerVariables.apiRequestKingdom, entries, success, failure);


        //}




        //public void Getkingdomlist(Action<KingdomresponceList> success, Action<int> failure)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);


        //    Debug.Log("Getkingdomlist ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //         "user_id =" + restLoginInfo.user_id + "\n" +
        //         "access_token =" + restLoginInfo.access_token + "\n");

        //    SendRequestPOST<KingdomresponceList>(ServerVariables.apiGetKingdomlist, entries, success, failure);

        //}


        //public void Getkingdomlistafter_serach(int searchtype ,string search_name, int kingdom_type, int req_cups, int req_warrior, Action<KingdomresponceList> success, Action<int> failure)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    if (searchtype == 1)
        //    {
        //        entries.Add("search_name", search_name);
        //    }
        //    else
        //    {
        //        entries.Add("search_name", search_name);
        //        entries.Add("kingdom_type", search_name);
        //        entries.Add("req_cups", req_cups);
        //        entries.Add("req_warrior", req_warrior);
        //    }

        //    Debug.Log("Getkingdomlistafter_serach ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //         "user_id =" + restLoginInfo.user_id + "\n" +
        //         "access_token =" + restLoginInfo.access_token + "\n"+
        //         "search_name =" + search_name + "\n"+
        //           "kingdom_type =" + kingdom_type + "\n"+
        //             "req_cups =" + req_cups + "\n"+
        //               "req_warrior =" + req_warrior + "\n");

        //    SendRequestPOST<KingdomresponceList>(ServerVariables.apiSearchKingdom, entries, success, failure);

        //}


        //public void Rejectnotification_seen( int notificationid ,int Kingdom_ID, Action<GenericResponse> success, Action<int> failure)
        //{

        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("kingdom_id", Kingdom_ID);
        //    entries.Add("notification_id", notificationid);


        //    Debug.Log("apirejectNotificationseen ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //     "user_id =" + restLoginInfo.user_id + "\n" +
        //     "access_token =" + restLoginInfo.access_token + "\n" +
        //     "Kingdom_ID =" + Kingdom_ID + "\n" +
        //      "notificationid =" + notificationid 

        //     );

        //    SendRequestPOST<GenericResponse>(ServerVariables.apirejectNotificationseen, entries, success, failure);


        //}

        //public void Getkingdomuserdetail(int search_user_id, Action<ProfileUserInfo> success, Action<int> failure)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("search_user_id", search_user_id);


        //    Debug.Log("Getkingdomuserdetail ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //         "user_id =" + restLoginInfo.user_id + "\n" +
        //         "access_token =" + restLoginInfo.access_token + "\n" +
        //         "search_user_id =" + search_user_id + "\n");

        //    SendRequestPOST<ProfileUserInfo>(ServerVariables.apiuserdetailKingdom, entries, success, failure);

        //}

        //public void Updatekingdomuserlevel(int level_user_id, string kick_msg, int level_type, Action<Kingdomresponce> success, Action<int> failure)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);
        //    entries.Add("level_user_id", level_user_id);
        //    entries.Add("level_type", level_type);
        //    entries.Add("kick_msg", kick_msg);


        //    Debug.Log("Updatekingdomuserlevel ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //         "user_id =" + restLoginInfo.user_id + "\n" +
        //         "access_token =" + restLoginInfo.access_token + "\n" +
        //         "level_user_id =" + level_user_id + "\n" +
        //         "level_type =" + level_type + "\n" +
        //         "kick_msg =" + kick_msg + "\n");

        //    SendRequestPOST<Kingdomresponce>(ServerVariables.apiUpdateuserLevelKingdom, entries, success, failure);

        //}

        //public void Fetchbattlehistory( Action<ListBattleHistory> callback, Action<int> failure)
        //{
        //    List<RestAPIEntry> entries = new List<RestAPIEntry>();
        //    entries.Add(ServerVariables.applicationKey, ServerVariables.paramAppKey);
        //    entries.Add("user_id", restLoginInfo.user_id);
        //    entries.Add("access_token", restLoginInfo.access_token);

        //    Debug.Log("apiBattleHistory ====  " + "\n" + "ServerVariables.applicationKey =" + ServerVariables.applicationKey + "\n" +
        //              "access_token =" + restLoginInfo.access_token + "\n"+
        //              "user_id =" + restLoginInfo.user_id + "\n"

        //        ); ;

        //    SendRequestPOST<ListBattleHistory>(ServerVariables.apiBattleHistory, entries, callback , failure);
        //}

        //#endregion

    }






}

