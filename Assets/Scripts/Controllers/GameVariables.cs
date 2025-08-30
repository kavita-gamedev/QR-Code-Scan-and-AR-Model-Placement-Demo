using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameVariables
{

	public const string soundOn = "soundOn";
	public const string musicOn = "musicOn";
	public const string facebookOn = "facebookOn";
	public const string googleOn = "googleOn";
	public const string gameCentreOn = "gameCentreOn";
	public const string googleid = "googleid";
	public const string gameCentreid = "gameCentreid";
	public const string fbProfilePic = "fbProfilePic";
	public const string lastNotificationId = "lastNotificationId";
	public const string triggerWalk = "Walk";
	public const string triggerAttack = "Attack";
	public const string triggerAttack2 = "Attack2";
	public const string state = "State";
	public const string moveX = "MoveX";
	public const string moveY = "MoveY";
	public const string horseDash = "HorseDash";
	public const string Blend = "Blend";

	public const string NO_CRYSTAL_MSG = "You're out of Gems. Visit the shop to get some more";
	public const string NO_CRYSTAL_HEADING = "Not enough Gems!";
	public const string NO_COINS_HEADING = "Not enough coins!";
	public const string NO_COINS_MSG = "You're out of Coins. Visit the shop to get some more";
	public const string NO_TROPHIES_MSG = "Not enough trophies!";
	
	public const string MEMBER_LIMIT_EXCEED = "Kingdom members limit exceeded";
	public const string EMPTY_COLLECTION_MSG = "YOUR COLLECTION IS EMPTY.";
	public const string EMPTY_LOCKED_CARDS_MSG = "ALL CARDS HAVE BEEN UNLOCKED.";
	public const string EXIT_MSG = "Are you sure you want to exit Epiko Regal?";
	public const string DOUBLE_MANA_MSG = "X2 RASA";
	public const string OVERTIME_MSG = "OVERTIME";
	public const string CUBE_UNLOCK_MSG_1 = "ANOTHER UNLOCK IS STILL IN PROCESS";
	public const string CUBE_UNLOCK_MSG_2 = "CUBE UNLOCK IS IN PROCESS";
	public const string ALERT_DUPLICATE_LOGIN = "Duplicate login detected. Restart the game.";
	public const string ALERT_FORCE_LOGIN = "This account is already in use! Do you want to switch accounts?";
	public const string ALERT_RESTART = "Unlock successful,Restart game to reflect in UI.";
	public const string SEARCH_CANCELLED_MSG = "Search cancelled";
	public const string NO_OPPONENTS_FOUND_MSG = "No Opponents Found";
	public const string ALERT_NO_INTERNET = "Unable to connect with the server.\nCheck your internet conncetion and try again.";
	public const string PURCHASE_FAILED = "Purchase has been canceled. Please try again later.";
	public const string GAME_CENTER_LOGIN_FAIL = "No Game Center account found.\n\nPlease login to Game Center and try again.";
	public const string OPPONENT_DISCONNECTED = "Opponent has left/disconnected from the battle!";

	public const string TUTORIAL_STATE_A_MSG = "These are the cards in your hand.\n\nYou have to Drag and Drop on the playing field";
	public const string TUTORIAL_STATE_B_MSG = "This is the rasa meter which keeps filling overtime.\nThe rasa is required to deploy cards in the field";
	public const string TUTORIAL_STATE_C_MSG = "Each card will have different rasa counts to summon and once you deploy that rasa will be deducted from the meter.";
	public const string TUTORIAL_STATE_D_MSG = "Drag and Drop the card on to the playing field.";
	public const string TUTORIAL_STATE_E_MSG = "You cannot place the cards in the red area of the playing field.";
	public const string TUTORIAL_STATE_F_MSG = "Each troop will have deployment time to spawn.\nA small clock icon is displayed on top";
	public const string TUTORIAL_STATE_G_MSG = "Once deployed the character will automatically move to the nearest target.";
	public const string TUTORIAL_STATE_H_MSG = "Now you know how to deploy your cards.\nUse your cards and destroy the enemy God Tower to win.";
	public const string TUTORIAL_STATE_I_MSG = "Now deploy one of your card to attack the enemy character";
	public const string TUTORIAL_MSG = "Welcome\nPress Battle button to start.";
	public const string INVITE_EXHAUSTED = "You can send only 5 invites per hour. Try again after some time";
	public const string INVITE_WAITING = " is waiting for a friendly battle ";

	public const string ROOM_CLOSED = "Room Closed...!";
	public const string coinPack1 = "com.lamar.socio.starterpack";
	public const string coinPack2 = "com.lamar.socio.bunchofcoins";
	public const string coinPack3 = "com.lamar.socio.bagofcoins";
	public const string coinPack4 = "com.lamar.socio.chestofcoins";

	public const string maxLevel = "MAX LEVEL";
	public const int cardMaxLevel = 10;

	public const string adCount = "adCount";

	public static string[] IAPPackage =
		{
			"Coin Basic Pack",
			"Coin Booster Pack",
			"Coins Super Pack",
			"Coins Ultimate Pack",
			"Coins Super Ultimate Pack",
			"Coins Bundle Pack"
		};

	public static FBStatus fbStatus = FBStatus.none;

	//public static string clientEmailId = "support@juegostudio.com";
	public static string clientEmailId = "admin@wharfstreetstudios.com";
	public static string clientSubject = "Subject";
	public static string clientMsgBody = "";

#if UNITY_IOS
    public static string gameID= "4375860";
#elif UNITY_ANDROID
    public static string gameID = "4375861";
#else
	public static string gameID = "";
#endif


#if UNITY_ANDROID
    public static string rateURL = "https://play.google.com/store/apps/details?id=com.lamar.socio";
#elif UNITY_IOS
	public static string rateURL = "https://itunes.apple.com/us/app/socio-world/id1234971578?ls=1&mt=8";
#else
	public static string rateURL = "https://www.google.com";
#endif


	public const int ManaFillRate = 3;
	public const int DoubleManaFillRate = 1;

	/// <summary>
	/// The max radius in kms. from the actual room location. If player falls into this category, for any room that is made,
	/// then we generate the player in that room.
	/// </summary>
	public const float MaxRadiusInKms = 8f;

	public const int ObstacleRadiusMetres = 500;

	public const string PushWooshID = "5C7B7-D688C";
	public const string GcmID = "894579338771";
	//"291466660106";

	public const int CoinsToUpdateCharacter = 1000;

	public static string[] loadingMessages =
		{
			"Be aware of your actual surroundings at all times. Stay safe!",
			"Be cautious, you may trip on some obstacle.",
			"Try to stay away from the player with shotgun during Rage Mode to save yourself.",
			"Running out of items? Visit the store and get some.",
			"Is someone annoying you? Just block that player... easy!",
			"Use \"Search Player\" to find a specific player.",
			"Tap on the pin point icon to interact with various options.",
			"Turn on Rage Mode to earn some extra XP for each kill.",
			"Turn on “Camouflage” under Inventory to shield yourself against any attack or obstacle.",
			"Set your privacy preference through “Privacy” under Settings.",
			"Make it to the top of leaderboard by earning more and more XP.",
			"Obstacle set by you is visible only to you.",
			"Customize your avatar by tapping on your avatar and choosing “Customize avatar”.",
			"Keep a Safe distance while driving.",
			"Be aware at all times, there might be an obstacle on your path.",
			"Safety comes in Cans! I can, you can, we can!",
			"Get Smart, Use safety from the start.",
			"Remember your safety ABC's: Always Be Careful.",
			"Stop your rage mode manually to claim the XP bonus!"
		};

	public static string cubeUnlockDebug1 = "{\"responseCode\": 1,\"responseMsg\": {\"reward_unlock_time\": 0,\"gold_bonus\": 50,\"crystal_bonus\": 0,\"total_crystal\": 0,\"total_gold\": 6075,\"card_count\": 3,\"ultra_rare_count\": 0,\"rare_count\": 1,\"card_details\": [{\"master_card_id\": 4,\"card_level\": 1,\"total_card\": 1,\"next_level_card_count\": 9}, {\"master_card_id\": 3,\"card_level\": 1,\"total_card\": 1,\"next_level_card_count\": 5}, {\"master_card_id\": 14,\"card_level\": 1,\"total_card\": 1,\"next_level_card_count\": 9}],\"cube_id\": 1,\"master_stadium_id\": 1,\"reward_status\": 10,\"cube_id_message\": \"1-Titanium; 2- Diamond; 3- Platinum; 4.Copper; 5.Bronze\",\"reward_status_message\": \"1- Not claimed; 2- On process; 3- Can claim; 10- Reward completed.\",\"achievement\": []},\"responseInfo\": \"Everything worked as expected\"}";
	public static string cubeUnlockDebug2 = "{\"responseCode\": 1,\"responseMsg\": {\"reward_unlock_time\": 0,\"gold_bonus\": 50,\"crystal_bonus\": 0,\"total_crystal\": 0,\"total_gold\": 6075,\"card_count\": 3,\"ultra_rare_count\": 0,\"rare_count\": 1,\"card_details\": [{\"master_card_id\": 4,\"card_level\": 1,\"total_card\": 1,\"next_level_card_count\": 9}, {\"master_card_id\": 3,\"card_level\": 1,\"total_card\": 1,\"next_level_card_count\": 5}, {\"master_card_id\": 14,\"card_level\": 1,\"total_card\": 1,\"next_level_card_count\": 9}],\"cube_id\": 2,\"master_stadium_id\": 1,\"reward_status\": 10,\"cube_id_message\": \"1-Titanium; 2- Diamond; 3- Platinum; 4.Copper; 5.Bronze\",\"reward_status_message\": \"1- Not claimed; 2- On process; 3- Can claim; 10- Reward completed.\",\"achievement\": []},\"responseInfo\": \"Everything worked as expected\"}";
	public static string cubeUnlockDebug3 = "{\"responseCode\": 1,\"responseMsg\": {\"reward_unlock_time\": 0,\"gold_bonus\": 50,\"crystal_bonus\": 0,\"total_crystal\": 0,\"total_gold\": 6075,\"card_count\": 3,\"ultra_rare_count\": 0,\"rare_count\": 1,\"card_details\": [{\"master_card_id\": 4,\"card_level\": 1,\"total_card\": 1,\"next_level_card_count\": 9}, {\"master_card_id\": 3,\"card_level\": 1,\"total_card\": 1,\"next_level_card_count\": 5}, {\"master_card_id\": 14,\"card_level\": 1,\"total_card\": 1,\"next_level_card_count\": 9}],\"cube_id\": 3,\"master_stadium_id\": 1,\"reward_status\": 10,\"cube_id_message\": \"1-Titanium; 2- Diamond; 3- Platinum; 4.Copper; 5.Bronze\",\"reward_status_message\": \"1- Not claimed; 2- On process; 3- Can claim; 10- Reward completed.\",\"achievement\": []},\"responseInfo\": \"Everything worked as expected\"}";


	//AI constants 

	public static string AreaCPUGodFront = "AreaCPUGodFront";
	public static string AreaCPUGodBack = "AreaCPUGodBack";
	public static string AreaCPULeftTowerFront = "AreaCPULeftTowerFront";
	public static string AreaCPULeftTowerBack = "AreaCPULeftTowerBack";
	public static string AreaCPURightTowerFront = "AreaCPURightTowerFront";
	public static string AreaCPURightTowerBack = "AreaCPURightTowerBack";
	public static string AreaCPULeftConqure = "AreaCPULeftConqure";
	public static string AreaCPURightConqure = "AreaCPURightConqure";


}
