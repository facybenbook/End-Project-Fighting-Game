using UnityEngine;
using System.Collections;

public class Characters : MonoBehaviour {
	
	public Object prefab;
	public string name;
	public GUIStyle profilepic;
	public string text;
	
	
	// Use this for initialization
	void Start () {
		text = text.Replace("@",System.Environment.NewLine);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
