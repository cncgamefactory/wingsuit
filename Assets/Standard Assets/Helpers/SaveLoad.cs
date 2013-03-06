using UnityEngine;
using System.Collections;
using System.IO;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class SaveLoad  
{
	private TextAsset missionTextAsset;
	
	public void SetMissionTextAsset(TextAsset ta)
	{
		missionTextAsset = ta; 
	}
	
	public UserData LoadUser()
	{
		UserData curUser = new UserData();
	
		// force new user every time
		bool debug = true;
		if(debug)
		{
			PlayerPrefs.DeleteAll();
		}
		
		// if !file exists
		if (PlayerPrefs.HasKey("id"))
		{
			curUser = LoadExistingUserData();
		}
		else
		{
			curUser = CreateEmptyUser(); 
		}
		
		return curUser; 
	}
	
	private UserData CreateEmptyUser()
	{
		Debug.Log("CREATING DEFAULT USER PREFS");

		UserData empty = new UserData(); 
		empty.Id = "fakeuser123";
		empty.Email = "no@thanks.com";
		empty.Mission1Id = "0";
		empty.Mission2Id = "1000";
		empty.Mission3Id = "2000";
		empty.PrevMission1Id = empty.Mission1Id;
		empty.PrevMission2Id = empty.Mission2Id;
		empty.PrevMission3Id = empty.Mission3Id;
		empty.BestDistance = 0;
		empty.BestHeight = 0;
		empty.BestSpeed = 0; 
		empty.DidTutorial = false; 
		empty.PrestigeLevel = 0; 
		
		return empty; 
	}
	
	private UserData LoadExistingUserData()
	{
		Debug.Log("LOADING EXISTING USER PREFS");

		UserData exUser = new UserData(); 
		
		exUser.Id = PlayerPrefs.GetString("id");
		exUser.Email = PlayerPrefs.GetString("email");
		exUser.Mission1Id = PlayerPrefs.GetString("m1");
		exUser.Mission2Id = PlayerPrefs.GetString("m2");
		exUser.Mission3Id = PlayerPrefs.GetString("m3");
		exUser.PrevMission1Id = PlayerPrefs.GetString("prevm1");
		exUser.PrevMission2Id = PlayerPrefs.GetString("prevm2");
		exUser.PrevMission3Id = PlayerPrefs.GetString("prevm3");
		exUser.BestDistance = PlayerPrefs.GetFloat("bestdistance");
		exUser.BestHeight = PlayerPrefs.GetFloat("bestheight");
		exUser.BestSpeed = PlayerPrefs.GetFloat("bestspeed");
		exUser.DidTutorial = PlayerPrefs.GetBool("didtutorial"); 
		exUser.PrestigeLevel = PlayerPrefs.GetInt("prestigelevel"); 
		
		return exUser;
		
	}
	
	public bool SaveUser(UserData uData)
	{
		Debug.Log("SAVING USER PREFS");

		PlayerPrefs.SetString("id",uData.Id);
		PlayerPrefs.SetString ("email", uData.Email);
		PlayerPrefs.SetString("m1",uData.Mission1Id);
		PlayerPrefs.SetString("m2",uData.Mission2Id);
		PlayerPrefs.SetString("m3",uData.Mission3Id);
		PlayerPrefs.SetString("prevm1",uData.PrevMission1Id);
		PlayerPrefs.SetString("prevm2",uData.PrevMission2Id);
		PlayerPrefs.SetString("prevm3",uData.PrevMission3Id);
		PlayerPrefs.SetFloat("bestheight",uData.BestHeight);
		PlayerPrefs.SetFloat("bestdistance",uData.BestDistance);
		PlayerPrefs.SetFloat("bestspeed",uData.BestSpeed);
		PlayerPrefs.SetBool("didtutorial", uData.DidTutorial); 
		PlayerPrefs.SetInt("prestigelevel", uData.PrestigeLevel); 
		
		PlayerPrefs.Flush();
		
		return true; 
	}
	
	public Hashtable LoadMissionData()
	{
		Hashtable missions = new Hashtable(); 
		
		Debug.Log ("Loading missions: " + missionTextAsset.name);
		
		StringReader sr = new StringReader(missionTextAsset.text); 
		string line = sr.ReadLine();
		
		while (line != null)
		{
			string[] parsed = line.Split('|'); 

			//id|name|condition1|operator1|value1|condition2|operator2|value2|rewardtype|rewardvalue|unlockid
			Mission m = new Mission(); 
			m.Id = parsed[0];
			m.Name = parsed[1];
			m.Condition1 = parsed[2];
			m.Operator1 = parsed[3];
			m.Value1 = NoNullNumbers(parsed[4]);
			m.Condition2 = parsed[5];
			m.Operator2 = parsed[6];
			m.Value2 = NoNullNumbers(parsed[7]);
			m.RewardType = parsed[8];
			m.RewardValue = NoNullNumbers(parsed[9]);
			m.UnlockId = parsed[10];

			missions[m.Id] = m;  
			
			line = sr.ReadLine(); 
		}
		
		return missions; 
		
	}
	
	private float NoNullNumbers(string num)
	{
		float retNum = -1; 
		
		if(num != null && num != "")
		{
//			Debug.Log ("Parsing " + num);
			retNum = float.Parse(num); 
		}
		
		return retNum;
	}
	
}
