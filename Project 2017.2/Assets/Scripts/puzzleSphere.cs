using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzleSphere : MonoBehaviour
{
	private Renderer rend;
	public  Color origColor;
	public  Color glowColor;
	private bool  bool_isGlowing;
	private bool  bool_playerLooking = false;

	public GameObject go_gameLogic;

	void Start()
	{
		rend = GetComponent<Renderer>();
		bool_isGlowing = false;
	}

	public void fn_glow()
	{
		rend.material.SetColor("_Color", glowColor);
	}

	public void fn_dimm()
	{
		rend.material.SetColor("_Color", origColor);
	}

	public void fn_buttonPressed()
	{
		if(go_gameLogic.GetComponent<GameLogic>().bool_takeInput == true && bool_playerLooking == true)
			fn_glow();
	}

	public void fn_buttonReleased()
	{
		if(go_gameLogic.GetComponent<GameLogic>().bool_takeInput == true && bool_playerLooking == true)
			fn_dimm();
	}

	public void fn_playerStartedLooking()
	{
		bool_playerLooking = true;
		Debug.Log(this.gameObject.name + ": Player started looking at me");
	}

	public void fn_playerStoppedLooking()
	{
		bool_playerLooking = false;
		Debug.Log(this.gameObject.name + ": Player stopped looking at me");
	}
}
