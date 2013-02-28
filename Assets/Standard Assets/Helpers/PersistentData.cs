using UnityEngine;
using System.Collections;

public class PersistentData : MonoBehaviour {
	
	public static PersistentData mPersistentData;
	
	public SaveLoad mSaveLoad;
	public UserData mUserData; 
	public Hashtable mMissionData;
	
	void Awake()
	{
		if (mPersistentData != null && mPersistentData != this)
		{
			Destroy(gameObject);
		}
		else
		{
			DontDestroyOnLoad(gameObject);
			mPersistentData = this; 
		}		
	}
	
	void Start () 
	{
		mSaveLoad = new SaveLoad();
		mUserData = mSaveLoad.LoadUser(); 
		mMissionData = mSaveLoad.LoadMissionData(); 
		
		Application.LoadLevel("Gameplay");
	}

}
