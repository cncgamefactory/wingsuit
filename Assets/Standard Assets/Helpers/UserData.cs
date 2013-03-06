using UnityEngine;
using System.Collections;

public class UserData 
{
	public string Id 			{ get; set; }
	public string Email			{ get; set; }
	public string Mission1Id 	{ get; set; }
	public string Mission2Id 	{ get; set; }
	public string Mission3Id 	{ get; set; }
	public string PrevMission1Id 	{ get; set; }
	public string PrevMission2Id 	{ get; set; }
	public string PrevMission3Id 	{ get; set; }
	public float BestHeight 	{ get; set; }
	public float BestSpeed 		{ get; set; }
	public float BestDistance 	{ get; set; }
	public bool DidTutorial		{ get; set; }
	public int PrestigeLevel	{ get; set; }
}
