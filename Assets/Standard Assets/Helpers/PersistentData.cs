using UnityEngine;
using System.Collections;

public class PersistentData : MonoBehaviour {
	
	public static PersistentData mPersistentData;
	
	public SaveLoad mSaveLoad;
	public UserData mUserData; 
	public Hashtable mMissionData;

	public TextAsset MISSION_TEXT_ASSET;

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
		mSaveLoad.SetMissionTextAsset(MISSION_TEXT_ASSET); 
		mUserData = mSaveLoad.LoadUser(); 
		mMissionData = mSaveLoad.LoadMissionData(); 
		
		Application.LoadLevel("Gameplay");
	}
	
	public string Md5Sum(string strToEncrypt)
	{
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);
	 
		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);
	 
		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";
	 
		for (int i = 0; i < hashBytes.Length; i++)
		{
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}
	 
		return hashString.PadLeft(32, '0');
	}
}
