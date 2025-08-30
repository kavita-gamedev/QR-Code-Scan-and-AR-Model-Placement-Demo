using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


 

[System.Serializable]
public class UserInfo
{
    public int user_id;
    public string username;
    public string email;

}

[System.Serializable]
public class Qrdetail
{
    public int qr_id;
    public string qr_string;
    public string video_link;
   
}

[System.Serializable]
public class LoginData : ResponseMessage
{
    public int user_id;
    public string access_token;
    public string message;
    public int is_alert;
    public int is_delete;
    public string google_id;
    public string game_center_id;

}

[System.Serializable]
public class QrdetailList : ResponseMessage
{
    public int user_id;
    public string access_token;
    public List<Qrdetail> Qrdetail_list;
}

[System.Serializable]
public class Card
{
	public int master_id;
}

[System.Serializable]
public class DeckDetail
{
	public int deck_id;
	public List<Card> cards;
}
[System.Serializable]
public class DeckDetailList : ResponseMessage
{
	public int current_deck_number;
	public List<DeckDetail> deck_details;
}


//[System.Serializable]
//public class LoginResponse : ServerResponse<LoginData>
//{
//}
//
//[System.Serializable]
//public class UserInfoResponse : ServerResponse<UserInfo>
//{
//}
//
//[System.Serializable]
//public class UserCardsResponse : ServerResponse<UserCardList>
//{
//}
[System.Serializable]
public class InviteDetails:ResponseMessage
{
    public string referrer_name;
    public string invite_token;
    public int invite_user_id;
}

[System.Serializable]
public class NotificationData
{
    public int user_id;
    public string user_name;
    public string invite_token;
    public int is_room_active;
    public int accepted_user_id;
    public int room_id;
}

[System.Serializable]
public class KingdomNotificationData
{
    
    public int kingdom_id;
    public string kingdom_message;

}

[System.Serializable]
public class NotificationDetails
{
    public int notification_id;
    public int notification_type;
    public NotificationData data;
    public KingdomNotificationData kingdomdata;
    public string message;
}

[System.Serializable]
public class NotificationResponse:ResponseMessage
{
    public List<NotificationDetails> content;
    public int last_notification_id;
}

[System.Serializable]
public class ValidateUser : ResponseMessage
{
    public int is_valid;
}

public class GenericResponse : ResponseMessage
{
	
}

[System.Serializable]
public class ServerResponse<T> where T:ResponseMessage
{
    public int responseCode;
    public T responseMsg;
    public string responseInfo;
}

[System.Serializable]
public class ResponseMessage
{

}

//[System.Serializable]
//public class Cell
//{
//    public int x;
//    public int y;
//    public float G;
//    public float H;
//    public float F;
//    //public Cell parent;
//    public INode cell;

//    //public static implicit operator Cell(AStar.Cell v)
//    //{
//    //    throw new NotImplementedException();
//    //}
//}

