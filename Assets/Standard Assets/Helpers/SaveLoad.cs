using UnityEngine;
using System.Collections;

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
		return empty; 
	}
	
	public bool SaveUser(UserData uData)
	{
		return true; 
	}
	
}
