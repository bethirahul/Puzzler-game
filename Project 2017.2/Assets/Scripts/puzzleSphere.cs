using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzleSphere : MonoBehaviour
{
	private bool isGlowing = false;

	public void fn_glow(bool isCondition)
	{
		if(isCondition != isGlowing)
		{
			if(isGlowing)
			{
				/// dimm
			}
			else
			{
				/// glow
			}
		}
	}
}
