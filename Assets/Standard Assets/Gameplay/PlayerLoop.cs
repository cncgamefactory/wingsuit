using UnityEngine;
using System.Collections;

public class PlayerLoop : MonoBehaviour {
	
	public string mGameState = "MainMenu";
	private GameLoop loop; 
	
	public GameObject spdBoost;
	public GameObject heightBoost;
	
	public GameObject[] ArmTrails; 
	public GameObject[] LegTrails; 
	public GameObject HeightParticles;
	
	private float heightBoostTimer = 0;
	private float speedBoostTimer = 0;
	
	private float mMaxSpeed = 0; 
	private float mMaxHeight = 0; 
	private float mMaxDistance = 0; 
	
	private float mph = 0;
	private float distance = 0;
	private float height = 0; 
	
	public int numSpeedBoostersHit = 0;
	public int numHeightBoostersHit = 0;
	
	void Start () 
	{
		loop = GameObject.Find("Logic").GetComponent<GameLoop>();
	}
	
	void DoBestPlayerInput()
	{
		float xPush = Input.acceleration.y * loop.MAX_X_FORCE * .5f;
		rigidbody.AddRelativeForce(0,0,xPush,ForceMode.VelocityChange);

//		Debug.Log("Player Rotation: " + transform.rotation.x + "," + transform.rotation.y + "," + transform.rotation.z); 
		
//		transform.RotateAroundLocal(new Vector3(0,.5f,.5f),.2f);
		
//		float rotSpeed = 100.0f * Input.acceleration.y; 
		
//        transform.Rotate(Vector3.up * (Input.acceleration.y * Time.deltaTime), Space.World);
//        transform.Rotate(new Vector3(0,.5f,.5f) * (rotSpeed * Time.deltaTime), Space.Self);
		
		float rotSpeed = .5f * Input.acceleration.y; 
        transform.RotateAroundLocal(new Vector3(0,.5f,.5f), (rotSpeed * Time.deltaTime));

		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (mGameState == "Gameplay")
		{
			
			DoBestPlayerInput();
			
			// HEIGHT!
			if (heightBoostTimer > 0)
			{
				Vector3 force = constantForce.force;
				force.y = loop.BOOST_Y_FORCE;
				constantForce.force = force;
	
				heightBoostTimer -= Time.fixedDeltaTime;
			}
			if (heightBoostTimer < 0)
			{
				heightBoostTimer = 0; 
	
				Vector3 force = constantForce.force;
				force.y = loop.DEFAULT_Y_FORCE;
				constantForce.force = force;
				
				HandleHeightBoost(false); 
			}
			
			// SPEED!
			if (speedBoostTimer > 0)
			{
				Vector3 force = constantForce.force;
				force.z = loop.BOOST_Z_FORCE;
				constantForce.force = force;
	
				speedBoostTimer -= Time.fixedDeltaTime;
			}
			if (speedBoostTimer < 0)
			{
				speedBoostTimer = 0; 
	
				Vector3 force = constantForce.force;
				force.z = loop.DEFAULT_Z_FORCE;
				constantForce.force = force;

				HandleSpeedBoost(false); 

			}			
			
			mph = rigidbody.velocity.magnitude * 2.237f * 7;
			height = transform.position.y;
			distance = transform.position.z * 10;
		
			if (mph > mMaxSpeed)
			{
				mMaxSpeed = mph; 
			}
			if (height > mMaxHeight)
			{
				mMaxHeight = height; 
			}
			if (distance > mMaxDistance)
			{
				mMaxDistance = distance;
			}
		}
		
		if (mGameState == "Limbo" && rigidbody.velocity.z < 5 )
		{
			// update the post-game results screen
			GameObject pgscreen = GameObject.Find ("Screen_PostGame");
			UI_PostGame script = pgscreen.GetComponent<UI_PostGame>();
			script.RefreshScore(mMaxDistance, mMaxSpeed, mMaxHeight);
	
			HandleSpeedBoost(false);
			HandleHeightBoost(false); 
			
			// we've displayed the stats to the user now, so we can save them now
			CheckForHighScores();
	
			// for missions that pass we need to update the UI
			if (DidPassAMission())
			{
				loop.SwitchState("Missions");
			}
			else
			{
				loop.SwitchState("PostGame");
			}
		
		}
		
		if (mGameState == "PostGame")
		{
			mMaxDistance = 0; 
			mMaxHeight = 0; 
			mMaxSpeed = 0; 
			numSpeedBoostersHit = 0;
			numHeightBoostersHit = 0; 
		}
	}
	
	void OnCollisionEnter(Collision col)
	{
		if (mGameState == "Gameplay")
		{
			// stop his downward force and make a big impact 
			constantForce.force = Vector3.zero;
			rigidbody.AddForceAtPosition(new Vector3(0,-40,-5), new Vector3(0,1,4), ForceMode.Impulse);

			// change game state
			loop.SwitchState("Limbo");
			
			GA.API.Design.NewEvent("GameOver:Crashed", transform.position);
		}
	}
	
	private void CheckForHighScores()
	{
		bool needSave = false; 
		
		if(mMaxSpeed > PersistentData.mPersistentData.mUserData.BestSpeed)
		{
			PersistentData.mPersistentData.mUserData.BestSpeed = mMaxSpeed;
			needSave = true; 
		}
		if(mMaxHeight > PersistentData.mPersistentData.mUserData.BestHeight)
		{
			PersistentData.mPersistentData.mUserData.BestHeight = mMaxHeight;
			needSave = true; 
		}
		if(mMaxDistance > PersistentData.mPersistentData.mUserData.BestDistance)
		{
			PersistentData.mPersistentData.mUserData.BestDistance = mMaxDistance;
			needSave = true; 
//			StartCoroutine(PostScores(PersistentData.mPersistentData.mUserData.Id,mMaxDistance));
		}
		
		if (needSave)
		{
			PersistentData.mPersistentData.mSaveLoad.SaveUser(PersistentData.mPersistentData.mUserData);
		}
	}
	
	private bool passedMission(Mission missionToCheck)
	{
		if ( 
			(
			CheckOperator(GetValToCheck(missionToCheck.Condition1),missionToCheck.Operator1,missionToCheck.Value1) &&
			missionToCheck.Condition2 == ""
			)
			|| 
			(
			CheckOperator(GetValToCheck(missionToCheck.Condition1),missionToCheck.Operator1,missionToCheck.Value1) &&
			CheckOperator(GetValToCheck(missionToCheck.Condition2),missionToCheck.Operator2,missionToCheck.Value2)
			)
		)
		{
			return true; 
		}
		else
		{
			return false; 
		}
	
	}
	
	private	bool DidPassAMission()
	{
		bool didPass = false; 
		
		Mission missionToCheck = PersistentData.mPersistentData.mMissionData[PersistentData.mPersistentData.mUserData.Mission1Id] as Mission;
		if (missionToCheck != null)
		{
			if (passedMission(missionToCheck))
			{
				Debug.Log ("Passed mission " + missionToCheck.Id + "! Unlocking mission " + missionToCheck.UnlockId);
				PersistentData.mPersistentData.mUserData.PrevMission1Id = PersistentData.mPersistentData.mUserData.Mission1Id;
				PersistentData.mPersistentData.mUserData.Mission1Id = missionToCheck.UnlockId;
				didPass = true; 
			}
		}

		missionToCheck = PersistentData.mPersistentData.mMissionData[PersistentData.mPersistentData.mUserData.Mission2Id] as Mission;
		if (missionToCheck != null)
		{
			if (passedMission(missionToCheck))
			{
				Debug.Log ("Passed mission " + missionToCheck.Id + "! Unlocking mission " + missionToCheck.UnlockId);
				PersistentData.mPersistentData.mUserData.PrevMission2Id = PersistentData.mPersistentData.mUserData.Mission2Id;
				PersistentData.mPersistentData.mUserData.Mission2Id = missionToCheck.UnlockId;
				didPass = true; 
			}
		}
		
		missionToCheck = PersistentData.mPersistentData.mMissionData[PersistentData.mPersistentData.mUserData.Mission3Id] as Mission;
		if (missionToCheck != null)
		{
			if (passedMission(missionToCheck))
			{
				Debug.Log ("Passed mission " + missionToCheck.Id + "! Unlocking mission " + missionToCheck.UnlockId);
				PersistentData.mPersistentData.mUserData.PrevMission3Id = PersistentData.mPersistentData.mUserData.Mission3Id;
				PersistentData.mPersistentData.mUserData.Mission3Id = missionToCheck.UnlockId;
				didPass = true; 
			}
		}
		
		return didPass;
	
	}

	float GetValToCheck(string lhs)
	{
		float valToCheck = -1.0f;

		switch (lhs)
		{
			case "distance":
				valToCheck = mMaxDistance;
				break;
			case "height":
				valToCheck = mMaxHeight;
				break;
			case "speed":
				valToCheck = mMaxSpeed;
				break;
			case "speedboosts":
				valToCheck = numSpeedBoostersHit;
				break;
			case "heightboosts":
				valToCheck = numHeightBoostersHit;
				break;
			
			default:
				break;
		}
		
		return valToCheck;
		
	}
	
	bool CheckOperator(float lhs, string op, float rhs)
	{
		bool completed = false; 
		
		if (op != null && op != "")
		{
			switch (op)
			{
			case ">":
				if (lhs > rhs)
				{
					completed = true; 
				}
				break;
			case ">=":
				if (lhs >= rhs)
				{
					completed = true; 
				}
				break;
			case "<":
				if (lhs < rhs)
				{
					completed = true; 
				}
				break;
			case "<=":
				if (lhs <= rhs)
				{
					completed = true; 
				}
				break;
			case "!=":
				if (lhs != rhs)
				{
					completed = true; 
				}
				break;
			case "=":
			case "==":
				if (lhs == rhs)
				{
					completed = true; 
				}
				break;
			default:
				break;
			}
			
		}
		
		
		return completed;
	}
	
	public void Bounce(GameObject obj)
	{
		iTween.PunchScale(obj, new Vector3(.5f, .5f, .5f), .5f);
	}
	
	public void AddHeightBoost(float time)
	{
		HandleHeightBoost(true); 
		heightBoostTimer += time; 
	}
	
	public void AddSpeedBoost(float time)
	{
		HandleSpeedBoost(true); 
		speedBoostTimer += time; 
	}
	
	public void ZeroOutSpeedBoost()
	{
		speedBoostTimer = 0; 
		HandleSpeedBoost(false);
	}

	public void ZeroOutHeightBoost()
	{
		heightBoostTimer = 0; 
		HandleHeightBoost(false); 
	}
	
	private void HandleHeightBoost(bool enabled)
	{
		heightBoost.active = enabled; 
		HeightParticles.active = enabled; 

		if (enabled)
		{
			Bounce (heightBoost);
			CameraJitter.sharedInstance.ShakeIntensity = .007f;
		}
		else
		{
			CameraJitter.sharedInstance.ShakeIntensity = .004f;
		}
		
	}
	
	private void HandleSpeedBoost(bool enabled)
	{
		spdBoost.active = enabled; 

		if (enabled)
		{
			ArmTrails[0].GetComponent<TrailRenderer>().time = .3f;
			ArmTrails[1].GetComponent<TrailRenderer>().time = .3f;
			Bounce (spdBoost);
			CameraJitter.sharedInstance.ShakeIntensity = .02f;
		}
		else
		{
			ArmTrails[0].GetComponent<TrailRenderer>().time = .15f;
			ArmTrails[1].GetComponent<TrailRenderer>().time = .15f;
			CameraJitter.sharedInstance.ShakeIntensity = .004f;
		}
		
	}
	
    private string secretKey = "POOhm0ox5^t!$c0vo505c@#i5hk+0f0bsrs!@7pdojf0ymf#2)oPOO"; // Edit this value and make sure it's the same as the one stored on the server
    private string addScoreURL = "http://www.rowshambow.com/itctest/addscore.php?"; //be sure to add a ? to your url
    private string highscoreURL = "http://www.rowshambow.com/itctest/display.php";
	
	// remember to use StartCoroutine when calling this function!
    IEnumerator PostScores(string name, float score)
    {
        //This connects to a server side php script that will add the name and score to a MySQL DB.
        // Supply it with a string representing the players name and the players score.
        string hash = PersistentData.mPersistentData.Md5Sum(name + score + secretKey);
 
        string post_url = addScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash;
 
        // Post the URL to the site and create a download object to get the result.
        WWW hs_post = new WWW(post_url);
        yield return hs_post; // Wait until the download is done
 
        if (hs_post.error != null)
        {
            Debug.Log("There was an error posting the high score: " + hs_post.error);
        }
    }
 
    // Get the scores from the MySQL DB to display in a GUIText.
    // remember to use StartCoroutine when calling this function!
    IEnumerator GetScores()
    {
        gameObject.guiText.text = "Loading Scores";
        WWW hs_get = new WWW(highscoreURL);
        yield return hs_get;
 
        if (hs_get.error != null)
        {
            Debug.Log("There was an error getting the high score: " + hs_get.error);
        }
        else
        {
//            gameObject.guiText.text = hs_get.text; // this is a GUIText that will display the scores in game.
        }
    }	
	
}
