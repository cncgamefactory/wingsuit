using UnityEngine;
using System.Collections;

public class PlayerLoop : MonoBehaviour {
	
	public string mGameState = "MainMenu";
	private GameLoop loop; 
	
	private float boostTimer = 0;
	
	void Start () 
	{
		loop = GameObject.Find("Logic").GetComponent<GameLoop>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (boostTimer > 0)
		{
			Vector3 force = constantForce.force;
			force.y = loop.BOOST_Y_FORCE;
			constantForce.force = force;

			boostTimer -= Time.fixedDeltaTime;
		}
		if (boostTimer < 0)
		{
			boostTimer = 0; 

			Vector3 force = constantForce.force;
			force.y = loop.DEFAULT_Y_FORCE;
			constantForce.force = force;
		}
	}
	
	void OnCollisionEnter(Collision col)
	{
		if (mGameState == "Gameplay")
		{
			Debug.Log("COLLIDED WITH " + col.gameObject.name);
			
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
			loop.SwitchState("PostGame");
		}
	}
	
	public void AddBoost(float time)
	{
		boostTimer += time; 
	}
}
