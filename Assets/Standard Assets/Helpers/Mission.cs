using UnityEngine;
using System.Collections;

public class Mission {

	public string Id			{ get; set; }
	public string Name			{ get; set; }
	public string Condition1	{ get; set; }
	public string Operator1		{ get; set; }
	public float  Value1		{ get; set; }
	public string Condition2	{ get; set; }
	public string Operator2		{ get; set; }
	public float  Value2		{ get; set; }
	public string RewardType	{ get; set; }
	public float  RewardValue	{ get; set; }
	public string UnlockId		{ get; set; }	
}
