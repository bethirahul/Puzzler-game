﻿using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour
{
	public GameObject go_player;
	public GameObject go_startUI, go_finishUI;
	public GameObject go_startPoint, go_playPoint, go_finishPoint;
	public GameObject[] go_arr_puzzleSphere;
	private int numOfSpheres = 5;
	private int[] arr_sequenceOrder;
	public float seqAnimSpeed = 1.0f;

	private int currentIndex;
	private bool isBallGlowing = false;

	void Start()
	{
		// Update 'player' to be the camera's parent gameobject, i.e. GvrEditorEmulator, instead of the camera itself.
		// Required because GVR resets camera position to 0, 0, 0.
		// player = player.transform.parent.gameObject;

		arr_sequenceOrder = new int[numOfSpheres];

		// Move player to the start position.
		go_player.transform.position = go_startPoint.transform.position;
	}

	public void fn_startGame()
	{
		go_startUI.SetActive(false);
		fn_initGame ();
	}

	public void fn_restartGame()
	{
		go_finishUI.SetActive(false);
		fn_initGame ();
	}

	private void fn_initGame()
	{
		fn_movePlayerToPoint(go_playPoint.transform.position);
		for (int i = 0; i < numOfSpheres; i++)
		{
			arr_sequenceOrder[i] = Random.Range(0, numOfSpheres);
		}
		Debug.Log ("The puzzle sequence is " + arr_sequenceOrder[0] + ", " + arr_sequenceOrder[1] + ", " + arr_sequenceOrder[2] + ", " + arr_sequenceOrder[3] + ", " + arr_sequenceOrder[4]);
		currentIndex = 0;

		isBallGlowing = false;
		fn_dimmAllSpheres();
		currentIndex = 0;
		InvokeRepeating ("fn_showSequence", 3.0f, seqAnimSpeed);
	}

	public void fn_showSequence()
	{
		if(isBallGlowing == false)
		{
			fn_glowBall(arr_sequenceOrder[currentIndex]);
		}
		else
		{
			fn_dimmAllSpheres();
			currentIndex++;
			if(currentIndex >= numOfSpheres)
			{
				CancelInvoke("fn_showSequence");
			}
		}

		isBallGlowing = !isBallGlowing;

	}

	private void fn_glowBall(int ballIndex)
	{
		//go_arr_puzzleSphere[ballindex].fn_glow(true);
	}

	private void fn_dimmAllSpheres()
	{
		for (int i = 0; i < numOfSpheres; i++)
		{
			//go_arr_puzzleSphere[i].fn_glow(false);
		}
	}

	private void fn_movePlayerToPoint(Vector3 point)
	{
		iTween.MoveTo
		(
			go_player,
			iTween.Hash
			(
				"position", point,
				"time", 2,
				"easetype", "linear"
			)
		);
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0) && go_player.transform.position == go_playPoint.transform.position)
		{
			fn_gameWon();
		}
	}

	private void fn_gameWon()
	{
		fn_movePlayerToPoint(go_finishPoint.transform.position);
		go_finishUI.SetActive (true);
	}
}
