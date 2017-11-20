using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzleSphere : MonoBehaviour
{
	private Renderer rend;
	public  Color origColor;
	public  Color glowColor;
	public  int   id;

	public GameObject go_gameLogic;

	void Start()
	{
		rend = GetComponent<Renderer>();
	}

	public void fn_glow()
	{
		rend.material.SetColor("_Color", glowColor);
	}

	public void fn_dimm()
	{
		rend.material.SetColor("_Color", origColor);
	}

	public void fn_glowOnce()
	{
		rend.material.SetColor("_Color", glowColor);
		Invoke("fn_dimm",0.33f); 
	}

	/*public void fn_buttonPressed()
	{
		if(go_gameLogic.GetComponent<GameLogic>().bool_takeInput == true)
		{
			fn_glow();
			Debug.Log(this.gameObject.name + ": pressed");
		}
	}*/

	public void fn_clicked()
	{
		if(go_gameLogic.GetComponent<GameLogic>().bool_takeInput == true)
		{
			Debug.Log(this.gameObject.name + ": clicked");
			go_gameLogic.GetComponent<GameLogic>().fn_registerPoint(this.id);
		}
	}

	public void fn_playerStartedLooking()
	{
		if(go_gameLogic.GetComponent<GameLogic>().bool_takeInput == true)
		{
			fn_glow();
			Debug.Log(this.gameObject.name + ": Player started looking at me");
		}
	}

	public void fn_playerStoppedLooking()
	{
		if(go_gameLogic.GetComponent<GameLogic>().bool_takeInput == true)
		{
			fn_dimm();
			Debug.Log(this.gameObject.name + ": Player stopped looking at me");
		}
	}
}
