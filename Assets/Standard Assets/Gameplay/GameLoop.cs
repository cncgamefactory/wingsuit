using UnityEngine;
using System.Collections;

public class GameLoop : MonoBehaviour {
	
	public float MAX_X_FORCE = 3.0f;
	public float DEFAULT_Y_FORCE = 7.0f;
	public float DEFAULT_Z_FORCE = 100.0f;
	public float BOOST_Z_FORCE = 150.0f;
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
		
		TERRAIN_PREFABS = ShufflePrefabs(mPrefabArray);
		
		// pull 3 of the prefabs out to make the first 3 segments. 
		for (int i = 0; i < 3; i++)
		{
			Vector3 position = Vector3.zero;
			position.z = 1000 * i;
			Transform newTerrain = Instantiate(TERRAIN_PREFABS[i], position, Quaternion.identity) as Transform;
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
		
		for (int j = startZ; j < (startZ + 1000); j++)
		{
			xPos = Random.Range(-120,120);
			GameObject marker = (GameObject)Instantiate(Resources.Load ("Marker"));
			marker.transform.parent = myParent;
			marker.transform.position = new Vector3(xPos, 0, j); 
			// thin out the height boosters the farther we go
			int min = 50 + (terrainCounter * 10);
			int max = 200 + (terrainCounter * 10);
			j+= Random.Range(min, max);
			numMarkers++;
		}
		
		for (int i = startZ; i < (startZ + 1000); i++)
		{
			xPos = Random.Range(-120,120);
			GameObject booster = (GameObject)Instantiate(Resources.Load ("Booster"));
			booster.transform.parent = myParent;
			booster.transform.position = new Vector3(xPos, player.transform.position.y - 20, i); 
			i+= Random.Range(300,800);
			numBoosters++;
		}
		
		Debug.Log("Generated " + numMarkers + " markers and " + numBoosters + " boosters for " + myParent.name);
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
			// with the mission data, since it has special shit, we have to call and refresh it. 
			GameObject screenObj = GameObject.Find ("Screen_Missions");
			UI_Missions script = screenObj.GetComponent<UI_Missions>();
			script.RefreshMissionData(); 
			Hashtable myhash = iTween.Hash("time",.5f,"y",-10,"onComplete","OnScreenLoaded","onCompleteTarget",screenObj);
			LoadScreen("Screen_Missions", myhash); 
		}
		
		
		if (stateName == "PostGame")
		{
			// End the screen Recording
			if (Everyplay.SharedInstance.IsSupported() && Everyplay.SharedInstance.IsRecording())
			{
				Everyplay.SharedInstance.StopRecording();
				ShowThumbnailToTheUserInTheUI(); 
			}

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
