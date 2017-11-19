using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzleSphere : MonoBehaviour
{
	private Renderer rend;
	public  Color origColor;
	public  Color glowColor;
	private bool bool_isGlowing;

	void Start()
	{
		rend = GetComponent<Renderer>();
		bool_isGlowing = false;
	}

	public void fn_resetColor()
	{
		rend.material.SetColor("_Color", origColor);
	}


}
