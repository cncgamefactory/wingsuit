using UnityEngine;
using System.Collections;

public class GameLoop : MonoBehaviour {
	
	public float MAX_X_FORCE = 3.0f;
	public float DEFAULT_Y_FORCE = 7.0f;
	public float DEFAULT_Z_FORCE = 100.0f;
	public float BOOST_Y_FORCE = 14.0f;
	
	public Transform player;
	
	public Transform[] TERRAIN_PREFABS;
	private ArrayList mPrefabArray; 
	
	private ConstantForce mPlayerForce;
	private int terrainCounter = 0; 
	private int lastDestroyedTerrainIndex = 0; 
	
	private string mGameState = "MainMenu";
	
	private string mCurScreen = "Screen_MainMenu";
	
	// Use this for initialization
	void Start () 
	{
		PopulateTerrain();
		mPlayerForce = player.constantForce; 
		
		SwitchState(mGameState); 
	}
	
	private void PopulateTerrain()
	{
		mPrefabArray = new ArrayList();
		for (int i = 0; i < TERRAIN_PREFABS.Length; i++)
		{
			mPrefabArray.Add(TERRAIN_PREFABS[i]);
		}
		
		mPrefabArray = ShuffleArrayList(mPrefabArray);
		
		// pull 3 of the prefabs out to make the first 3 segments. 
		for (int i = 0; i < 3; i++)
		{
			Vector3 position = Vector3.zero;
			position.z = 1000 * i;
			Instantiate(mPrefabArray[i] as Transform, position, Quaternion.identity);
			terrainCounter++;
			
			// don't generate any balloon things in the first terrain piece
			if (i > 0)
			{
				GenerateMarkers((int)position.z);
			}
		}
		
	}
	
	private void CheckForTerrainGeneration()
	{
		if ( player.transform.position.z > ((lastDestroyedTerrainIndex + 1) * 1000) && 
			player.transform.position.z < ((lastDestroyedTerrainIndex + 1) * 1000 + 3) )
		{
			lastDestroyedTerrainIndex++;
			GenerateTerrainPiece();
			Debug.Log("Crossed terrain marker");
		}
	}

	private void GenerateTerrainPiece()
	{
		int i = Random.Range(0,mPrefabArray.Count);
		Vector3 position = Vector3.zero;
		position.z = 1000 * terrainCounter;
		
		Instantiate(mPrefabArray[i] as Transform, position, Quaternion.identity);
		terrainCounter++;
		
		GenerateMarkers((int)position.z);
	}
	
	private void GenerateMarkers(int startZ)
	{
		for (int j = startZ; j < (startZ + 1000); j++)
		{
			int xPos = Random.Range(-120,120);
			GameObject marker = (GameObject)Instantiate(Resources.Load ("Marker"));
			marker.transform.parent = gameObject.transform;
			marker.transform.position = new Vector3(xPos, 0, j); 
			j+= Random.Range(50,200);
		}
		
	}
	
	private ArrayList ShuffleArrayList(ArrayList source)
	{
		ArrayList sortedList = new ArrayList(); 
		
    	while (source.Count > 0)
    	{
        	int position = Random.Range(0,source.Count);
        	sortedList.Add(source[position]);
        	source.RemoveAt(position);
    	}

    	return sortedList;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (mGameState == "Gameplay")
		{
			CheckForTerrainGeneration();
			
			// ROTATION OF GUY
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				iTween.RotateTo(player.gameObject,new Vector3(30,270,60), 6.0f);
			}
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				iTween.RotateTo(player.gameObject,new Vector3(-30,270,60), 6.0f);
			}
			if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
			{
				iTween.RotateTo(player.gameObject,new Vector3(0,270,60), 6.0f);
			}
			
			// Hack crash
			if (Input.GetKeyDown(KeyCode.Space))
			{
				Vector3 force = new Vector3(0,-1000,0);
				player.rigidbody.AddForce(force); 
			}
			
			// FORCES ON GUY
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
		}
	}
	
	void OnGUI()
	{
		if (mGameState == "MainMenu")
		{
			if (GUI.Button(new Rect(150,400,200,80),"START"))
			{
				SwitchState("Gameplay");
			}
		}
		
		if (mGameState == "PostGame")
		{
			// End the screen Recording
			if (Everyplay.SharedInstance.IsSupported())
			{
				Everyplay.SharedInstance.StopRecording();
				ShowThumbnailToTheUserInTheUI(); 
			}
			
			// UI
			
			GUI.Button(new Rect(0,50,80,80),"Twitter");

			if (GUI.Button(new Rect(0,500,60,180),"Missions"))
			{
				SwitchState("Missions");
			}
			
			GUI.TextField(new Rect(100,150,300,200), player.transform.position.z.ToString("N2") + "m");
			
			if (Everyplay.SharedInstance.IsSupported())
			{
				if(GUI.Button(new Rect(100,400,200,180),"Replay"))
				{
					Everyplay.SharedInstance.PlayLastRecording();
				}
			}

			GUI.TextField(new Rect(600,150,300,200),"Leaderboards\nIanCummings -- 20000m\n" +
				"Tim Chism -- 15000m\n" +
				"Andrea Dailey -- 10000m");

			
			GUI.Button(new Rect(600,400,200,180),"Store");

			GUI.Button(new Rect(820,400,80,80),"GameCenter");
			
			if (GUI.Button(new Rect(820,500,80,80),"Home"))
			{
				SwitchState("MainMenu");
			}

			if (GUI.Button(new Rect(600,600,300,120),"Play Again"))
			{
				SwitchState("Gameplay");
			}
		}
		
		if (mGameState == "Missions")
		{
			Mission m1 = (Mission)PersistentData.mPersistentData.mMissionData[PersistentData.mPersistentData.mUserData.Mission1Id];
			Mission m2 = (Mission)PersistentData.mPersistentData.mMissionData[PersistentData.mPersistentData.mUserData.Mission2Id];
			Mission m3 = (Mission)PersistentData.mPersistentData.mMissionData[PersistentData.mPersistentData.mUserData.Mission3Id];

			if(GUI.Button(new Rect(100,100,800,600),"Missions\n" +
				m1.Name + "\n" + 
				m2.Name + "\n" + 
				m3.Name))
			{
				SwitchState("PostGame");
			}
		}
		
	}
	
	public void SwitchState(string stateName)
	{
		// let the player know too
		PlayerLoop pLoop = player.GetComponent<PlayerLoop>();

		if (stateName == "MainMenu")
		{
			LoadScreen("Screen_MainMenu", true);
			// init the player to be frozen and off screen and shit. 
			player.rigidbody.drag = 1000.0f;
//			player.transform.position = Vector3.zero;
			
		}
		
		if (stateName == "Gameplay")
		{
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
			player.transform.position = new Vector3(0,50,0);	
			iTween.RotateTo(player.gameObject,new Vector3(0,270,60), 0.1f);
			
		}
		
		if (stateName == "Missions")
		{
			LoadScreen("Screen_Missions", false); 
		}
		
		
		if (stateName == "PostGame")
		{
			LoadScreen("Screen_PostGame", false);
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
		GameObject.Find("Polaroid").renderer.material.mainTexture = texture;
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
		float speed = 1.0f; 
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
		float speed = 1.0f; 
		if (immediate)
		{
			speed = 0.0f;
		}

		GameObject screen = GameObject.Find (name);
		iTween.MoveTo(screen, new Vector3(0,-110,0), speed);

	}
	
}
