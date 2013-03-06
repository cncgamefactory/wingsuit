using UnityEngine;
using System.Collections;

public class GameLoop : MonoBehaviour {
	
	public float MAX_X_FORCE = 3.0f;
	public float DEFAULT_Y_FORCE = 7.0f;
	public float DEFAULT_Z_FORCE = 10.0f;
	public float BOOST_Z_FORCE = 15.0f;
	public float BOOST_Y_FORCE = 14.0f;
	
	public Transform player;
	public Transform[] TERRAIN_PREFABS;
	private ArrayList mPrefabArray; 

	public AudioClip AUDIO_MENU;
	public AudioClip AUDIO_GAMEPLAY;
	public AudioSource WIND_SRC;
	private AudioSource mAudioSrc; 
	
	private ConstantForce mPlayerForce;
	private int terrainCounter = 0; 
	private int lastDestroyedTerrainIndex = 0; 
	
	private string mGameState = "MainMenu";
	private string mCurScreen = "Screen_MainMenu";
	
	private float terrainGenTimer = 0; 
	private Quaternion InitCameraRotation; 
	
	private float terrainScale = 100; 
	
	// Use this for initialization
	void Start () 
	{
		InitCameraRotation = GameObject.Find("Main Camera").transform.rotation; 
		
		mPrefabArray = new ArrayList();
		for (int i = 0; i < TERRAIN_PREFABS.Length; i++)
		{
			mPrefabArray.Add(TERRAIN_PREFABS[i]);
		}
		
		TERRAIN_PREFABS = ShufflePrefabs(mPrefabArray);
		
		mPlayerForce = player.constantForce; 
		mAudioSrc = gameObject.GetComponent<AudioSource>(); 
		
		SwitchState(mGameState); 
	}
	
	private void PopulateTerrain()
	{

		// pull 3 of the prefabs out to make the first 3 segments. 
		for (int i = 0; i < 4; i++)
		{
			Vector3 position = Vector3.zero;
			position.z = terrainScale * i;
			Transform newTerrain = Instantiate(TERRAIN_PREFABS[0], position, Quaternion.identity) as Transform;
			newTerrain.name = "gen_terrain_" + terrainCounter;

			terrainCounter++;
			
			// don't generate any balloon things in the first terrain piece
			if (i > 0)
			{
				GenerateMarkers((int)position.z, newTerrain);
			}
		}
		
	}
	
	private void CheckForTerrainGeneration()
	{
		terrainGenTimer -= Time.fixedDeltaTime;
		
		if ( player.transform.position.z > ((lastDestroyedTerrainIndex + 1) * terrainScale) && 
			terrainGenTimer <= 0)
		{
			lastDestroyedTerrainIndex++;
			GenerateTerrainPiece();
			terrainGenTimer += 3;
			Debug.Log("Crossed terrain marker");
		}
	}

	
	private void GenerateTerrainPiece()
	{
		int i = Random.Range(0,mPrefabArray.Count);
		Vector3 position = Vector3.zero;
		position.z = terrainScale * terrainCounter;
		
		Transform newTerrain = Instantiate(TERRAIN_PREFABS[i], position, Quaternion.identity) as Transform;
		newTerrain.name = "gen_terrain_" + terrainCounter;
		terrainCounter++;
		
		GenerateMarkers((int)position.z, newTerrain);
		
		if (terrainCounter > 4)
		{
			GameObject terrainToKill = GameObject.Find("gen_terrain_" + (terrainCounter - 5));
			Destroy(terrainToKill);
		}
	}
	
	private void GenerateMarkers(int startZ, Transform myParent)
	{
		int xPos = 0;
		
		int numMarkers = 0;
		int numBoosters = 0; 
		
		for (float j = startZ; j < (startZ + terrainScale); j++)
		{
			xPos = Random.Range(-30,30);
			GameObject marker = (GameObject)Instantiate(Resources.Load ("Marker"));
			marker.transform.parent = myParent;
			marker.transform.position = new Vector3(xPos, 0, j); 

			float min = terrainScale *.3f;
			float max = terrainScale *.7f;
			j += Random.Range(min, max);
			numMarkers++;
		}
		
		for (float i = startZ; i < (startZ + terrainScale); i++)
		{
			xPos = Random.Range(-30,30);
			GameObject booster = (GameObject)Instantiate(Resources.Load ("Booster"));
			booster.transform.parent = myParent;
			booster.transform.position = new Vector3(xPos, Random.Range(6,25), i); 
			DoRandomRingRotation(booster); 
			i+= Random.Range(terrainScale *.45f,terrainScale*.7f);
			numBoosters++;
		}
		
		Debug.Log("Generated " + numMarkers + " markers and " + numBoosters + " boosters for " + myParent.name);
	}
	
	private void DoRandomRingRotation(GameObject booster)
	{
		int rand = Random.Range (0,100); 
		
		if (rand > 66)
		{
			booster.GetComponent<RingRotate>().XROTAMT = 20 + (terrainCounter * 8);
		}
		else if (rand > 33)
		{
			booster.GetComponent<RingRotate>().YROTAMT = 20 + (terrainCounter * 8);
		}
		else if (rand > 10)
		{
			booster.GetComponent<RingRotate>().ZROTAMT = 20 + (terrainCounter * 8);
		}
		else 
		{
			booster.GetComponent<RingRotate>().XROTAMT = 50 + (terrainCounter * 2);
			booster.GetComponent<RingRotate>().YROTAMT = 50 + (terrainCounter * 2);
			booster.GetComponent<RingRotate>().ZROTAMT = 50 + (terrainCounter * 2);
			
		}
		
	}
	
	private Transform[] ShufflePrefabs(ArrayList source)
	{
		ArrayList sortedList = new ArrayList(); 
		
		Transform[] sortedPrefabs = new Transform[source.Count];
		
    	while (source.Count > 0)
    	{
        	int position = Random.Range(0,source.Count);
        	sortedList.Add(source[position]);
        	source.RemoveAt(position);
    	}
		
		for(int i=0; i<sortedList.Count; i++)
		{
			sortedPrefabs[i] = (Transform)sortedList[i];
		}

    	return sortedPrefabs;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (mGameState == "Gameplay")
		{
			CheckForTerrainGeneration();
			
			// Hack crash
			if (Input.GetKeyDown(KeyCode.A))
			{
				Vector3 force = new Vector3(0,-1000,0);
				player.rigidbody.AddForce(force); 
			}
			
			// [DEBUG] FORCES ON GUY
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				Vector3 force = mPlayerForce.force;
				force.x = -MAX_X_FORCE;
				mPlayerForce.force = force;
			}
			if (Input.GetKey(KeyCode.RightArrow))
			{
				Vector3 force = mPlayerForce.force;
				force.x = MAX_X_FORCE;
				mPlayerForce.force = force;
			}
			if (Input.GetKey(KeyCode.UpArrow))
			{
				Vector3 force = mPlayerForce.force;
				force.y = BOOST_Y_FORCE;
				mPlayerForce.force = force;
			}
			if (Input.GetKey(KeyCode.DownArrow))
			{
				Vector3 force = mPlayerForce.force;
				force.y = DEFAULT_Y_FORCE;
				mPlayerForce.force = force;
			}
			if (Input.GetKey(KeyCode.Space))
			{
				Vector3 force = mPlayerForce.force;
				force.z = BOOST_Z_FORCE;
				mPlayerForce.force = force;
			}
			if (Input.GetKey(KeyCode.LeftShift))
			{
				Vector3 force = mPlayerForce.force;
				force.z = DEFAULT_Z_FORCE;
				mPlayerForce.force = force;
			}
		}
	}

	public void SwitchState(string stateName)
	{
		GA.API.Design.NewEvent("GameState:" + stateName); 

		// if we were in gameplay, now we switch the audio to menu music
		if (mGameState == "Gameplay" && stateName != mGameState)
		{
			mAudioSrc.clip = AUDIO_MENU;
			WIND_SRC.Stop(); 
			mAudioSrc.Play(); 
		}
		
		// to let the player know too
		PlayerLoop pLoop = player.GetComponent<PlayerLoop>();

		if (stateName == "MainMenu")
		{
			LoadScreen("Screen_MainMenu", true);
			// init the player to be frozen and off screen and shit. 
			player.rigidbody.drag = 1000.0f;
//			player.transform.position = Vector3.zero;
			
		}
		
		if (stateName == "Tutorial")
		{
			LoadScreen("Screen_Tutorial", false); 
			UI_SFX.SharedInstance.Play(UI_SFX.SharedInstance.SFX_WHOOSH); 
				
		}
		
		if (stateName == "Gameplay")
		{
			for (int i=0; i < terrainCounter; i++)
			{
				GameObject ter = GameObject.Find("gen_terrain_" + i);
				if (ter != null)
				{
					Destroy(ter); 
				}
			}
			
			terrainCounter = 0; 
			PopulateTerrain(); 

			
			// Start gameplay music
			mAudioSrc.clip = AUDIO_GAMEPLAY; 
			mAudioSrc.Play(); 
			WIND_SRC.Play(); 

			UI_SFX.SharedInstance.Play(UI_SFX.SharedInstance.SFX_THUMP); 
			
			// Start screen Recording
			if (Everyplay.SharedInstance.IsSupported())
			{
				Everyplay.SharedInstance.StartRecording();
			}

			LoadScreen("Screen_Gameplay", true);

			// let that fucker fall!
			Vector3 cForce = new Vector3(0,DEFAULT_Y_FORCE,DEFAULT_Z_FORCE);
			player.constantForce.force = cForce;
			player.rigidbody.drag = 1.0f;
			player.transform.position = new Vector3(0,10,0);	
			iTween.RotateTo(player.gameObject,new Vector3(0,270,45), 0.1f);
			GameObject.Find("Main Camera").transform.rotation = InitCameraRotation; 
			if (PersistentData.mPersistentData.mUserData.PrestigeLevel == 1)
			{
//				player.Find("Tail").gameObject.active = true; 
			}
			
			pLoop.ZeroOutSpeedBoost(); 
			pLoop.ZeroOutHeightBoost(); 
			
			lastDestroyedTerrainIndex = 0; 
			
		}
		
		if (stateName == "Missions")
		{
			// with the mission data, since it has special shit, we have to call and refresh it. 
			GameObject screenObj = GameObject.Find ("Screen_Missions");
			UI_Missions script = screenObj.GetComponent<UI_Missions>();
			script.RefreshMissionData(); 
			Hashtable myhash = iTween.Hash("time",.5f,"y",-10,"onComplete","OnScreenLoaded","onCompleteTarget",screenObj);
			LoadScreen("Screen_Missions", myhash); 
			UI_SFX.SharedInstance.Play(UI_SFX.SharedInstance.SFX_WHOOSH); 
		}
		
		if (stateName == "Settings")
		{
			// with the mission data, since it has special shit, we have to call and refresh it. 
			GameObject screenObj = GameObject.Find ("Screen_Settings");
			UI_Settings script = screenObj.GetComponent<UI_Settings>();
			Hashtable myhash = iTween.Hash("time",.5f,"y",-10,"onComplete","OnScreenLoaded","onCompleteTarget",screenObj);
			LoadScreen("Screen_Settings", myhash); 
			UI_SFX.SharedInstance.Play(UI_SFX.SharedInstance.SFX_WHOOSH); 
		}
		
		if (stateName == "PostGame")
		{
			// End the screen Recording
			if (Everyplay.SharedInstance.IsSupported() && Everyplay.SharedInstance.IsRecording())
			{
				Everyplay.SharedInstance.SetMetadata("level_name","Great Wide Open");
				Everyplay.SharedInstance.SetMetadata("score",player.transform.position.z * 10);
				Everyplay.SharedInstance.SetMetadata("name",PersistentData.mPersistentData.mUserData.Id);
				Everyplay.SharedInstance.StopRecording();
				
				ShowThumbnailToTheUserInTheUI(); 
			}

			GameObject screenObj = GameObject.Find ("Screen_PostGame");
			Hashtable myhash = iTween.Hash("time",.5f,"y",-10,"onComplete","OnScreenLoaded","onCompleteTarget",screenObj);
			LoadScreen("Screen_PostGame", myhash); 
			UI_SFX.SharedInstance.Play(UI_SFX.SharedInstance.SFX_WHOOSH); 
		}

		
		// now update the global var so the game loop can continue appropriately!
		mGameState = stateName; 

		pLoop.mGameState = stateName;

	}
	
	
	// The filepath we're getting from the thumbnail event
	string thumbnailPath;
	
	/* Delegate for event (see section on getting events) */
	public void ThumbnailReadyAtFilePathDelegate(string filePath) 
	{
		this.thumbnailPath = filePath;
	}
	
	/* Define delegate methods */
	// Success delegate
	public void ThumbnailSuccess(Texture2D texture) 
	{
		// Yay, we have a video thumbnail, now we present it to the user
		GameObject.Find("btn_Polaroid").renderer.material.mainTexture = texture;
	}
	
	// Error delegate
	public void ThumbnailError(string error) 
	{
		// Oh noes, something went wrong
		Debug.Log("Thumbnail loading failed: " + error);
	}
	
	// Our own method that is used when the game is in a proper session to load and show the thumbnail
	public void ShowThumbnailToTheUserInTheUI() 
	{
		// Load the thumbnail, using our delegates as parameter
		Everyplay.SharedInstance.LoadThumbnailFromFilePath(this.thumbnailPath, ThumbnailSuccess, ThumbnailError);
	}
	
	private void LoadScreen(string name, bool immediate)
	{
		float speed = 0.5f; 
		if (immediate)
		{
			speed = 0.0f;
		}
		
		UnloadScreen(mCurScreen, immediate);
		mCurScreen = name;
		GameObject screen = GameObject.Find (name);
		iTween.MoveTo(screen, new Vector3(0,-10,0), speed);
		
	}
	
	private void UnloadScreen(string name, bool immediate)
	{
		float speed = 0.5f; 
		if (immediate)
		{
			speed = 0.0f;
		}

		GameObject screen = GameObject.Find (name);
		iTween.MoveTo(screen, new Vector3(0,-110,0), speed);

	}
		
	private void LoadScreen(string name, Hashtable hash)
	{
		UnloadScreen(mCurScreen);
		mCurScreen = name;
		GameObject screen = GameObject.Find (name);
		iTween.MoveTo(screen, hash);

	}
	
	private void UnloadScreen(string name)
	{
		GameObject screen = GameObject.Find (name);
		iTween.MoveTo(screen, new Vector3(0,-110,0), 1.0f);
	}
	
}
