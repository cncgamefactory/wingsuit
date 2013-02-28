using UnityEngine;
using System.Collections;

public class PlayerLoop : MonoBehaviour {
	
	public string mGameState = "MainMenu";
	private GameLoop loop; 
	
	private float heightBoostTimer = 0;
	private float speedBoostTimer = 0;
	
	private float mMaxSpeed = 0; 
	private float mMaxHeight = 0; 
	private float mMaxDistance = 0; 
	
	void Start () 
	{
		loop = GameObject.Find("Logic").GetComponent<GameLoop>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (mGameState == "Gameplay")
		{
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
			}			
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
		}
	}
	
	
	void OnGUI()
	{
		if (mGameState == "Gameplay")
		{
			float mph = rigidbody.velocity.magnitude * 2.237f;
			float height = transform.position.y * 3.28084f - 15;
			float distance = transform.position.z;
		
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
			
			string text = "Speed: " + mph.ToString("N2") + " mph\n" + 
				"Altitude: " + height.ToString("N2") + " ft\n" + 
					"Distance: " + distance.ToString("N2") + " meters\n" + 
					"(click to restart)";
			
			
			if (GUI.Button(new Rect(412,650,200,100),text))
			{
				loop.SwitchState("MainMenu");
			}
		}
		
		if (mGameState == "Limbo" && rigidbody.velocity.z < 5)
		{
			if (DidPassAMission())
			{
				loop.SwitchState("Missions");
			}
			else
			{
				loop.SwitchState("PostGame");
			}
		}
	}
	
	private	bool DidPassAMission()
	{
		bool didPass = false; 
		
		Mission missionToCheck = PersistentData.mPersistentData.mMissionData[PersistentData.mPersistentData.mUserData.Mission1Id] as Mission;
		if (missionToCheck != null)
		{
			if (CheckOperator(GetValToCheck(missionToCheck.Condition1),missionToCheck.Operator1,missionToCheck.Value1) || CheckOperator(GetValToCheck(missionToCheck.Condition2),missionToCheck.Operator2,missionToCheck.Value2))
			{
				Debug.Log ("Passed mission " + missionToCheck.Id + "! Unlocking mission " + missionToCheck.UnlockId);
				PersistentData.mPersistentData.mUserData.Mission1Id = missionToCheck.UnlockId;
				didPass = true; 
			}
		}

		missionToCheck = PersistentData.mPersistentData.mMissionData[PersistentData.mPersistentData.mUserData.Mission2Id] as Mission;
		if (missionToCheck != null)
		{
			if (CheckOperator(GetValToCheck(missionToCheck.Condition1),missionToCheck.Operator1,missionToCheck.Value1) || CheckOperator(GetValToCheck(missionToCheck.Condition2),missionToCheck.Operator2,missionToCheck.Value2))
			{
				Debug.Log ("Passed mission " + missionToCheck.Id + "! Unlocking mission " + missionToCheck.UnlockId);
				PersistentData.mPersistentData.mUserData.Mission2Id = missionToCheck.UnlockId;
				didPass = true; 
			}
		}
		
		missionToCheck = PersistentData.mPersistentData.mMissionData[PersistentData.mPersistentData.mUserData.Mission3Id] as Mission;
		if (missionToCheck != null)
		{
			if (CheckOperator(GetValToCheck(missionToCheck.Condition1),missionToCheck.Operator1,missionToCheck.Value1) || CheckOperator(GetValToCheck(missionToCheck.Condition2),missionToCheck.Operator2,missionToCheck.Value2))
			{
				Debug.Log ("Passed mission " + missionToCheck.Id + "! Unlocking mission " + missionToCheck.UnlockId);
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
	
	public void AddHeightBoost(float time)
	{
		heightBoostTimer += time; 
	}
	
	public void AddSpeedBoost(float time)
	{
		speedBoostTimer += time; 
	}
}
