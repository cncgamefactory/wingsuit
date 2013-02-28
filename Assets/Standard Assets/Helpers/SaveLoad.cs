using UnityEngine;
using System.Collections;
using System.IO;

public class SaveLoad  
{
	public UserData LoadUser()
	{
		UserData curUser = new UserData();
	
		// if !file exists
		curUser = CreateEmptyUser(); 
		
		// else
		//curUser = Serialize(file);
		
		return curUser; 
	}
	
	private UserData CreateEmptyUser()
	{
		UserData empty = new UserData(); 
		empty.Id = "fakeuser123";
		empty.Email = "no@thanks.com";
		empty.Mission1Id = "0";
		empty.Mission2Id = "1000";
		empty.Mission3Id = "2000";
		return empty; 
	}
	
	public bool SaveUser(UserData uData)
	{
		return true; 
	}
	
	public Hashtable LoadMissionData()
	{
		Hashtable missions = new Hashtable(); 
		
		string dataPath = Application.dataPath;
		string fileName = dataPath + "/missions.txt";
		Debug.Log ("Loading " + fileName + " from disc.");
		
		StreamReader sr = File.OpenText(fileName); 
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
