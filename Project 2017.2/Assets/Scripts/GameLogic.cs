using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour
{
	/*public GameObject startUI, restartUI;

	public void ToggleUI()
	{
		startUI.SetActive(!startUI.activeSelf);
		restartUI.SetActive(!restartUI.activeSelf);
	}

	// Placeholder method to prevent compiler errors caused by this method being called from LightUp.cs.
	public void PlayerSelection(GameObject sphere)
	{
		// Will be completed later in the course.
	}*/


	public GameObject player;
	public GameObject startUI, finishUI;
	public GameObject startPoint, playPoint, finishPoint;
	public GameObject[] puzzleSphere;
	public int numOfSpheres = 5;
	private int[] sequenceOrder;
	public float puzzleAnimSpeed = 1.0f;

	public void startGame()
	{
		startUI.SetActive (false);
		initGame ();
		iTween.MoveTo(player,
			iTween.Hash(
				"position", playPoint.transform.position,
				"time", 2,
				"easetype", "linear"
			)
		);
	}

	public void restartGame ()
	{
		finishUI.SetActive (false);
		startUI.SetActive (true);
		initGame ();
	}

	private void initGame()
	{
		player.transform.position = startPoint.transform.position;
		//createPuzzleSequence ();

	}

	void Start()
	{
		// Update 'player' to be the camera's parent gameobject, i.e. GvrEditorEmulator, instead of the camera itself.
		// Required because GVR resets camera position to 0, 0, 0.
		// player = player.transform.parent.gameObject;

		sequenceOrder = new int[numOfSpheres];

		// Move player to the start position.
		initGame();
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0) && player.transform.position == playPoint.transform.position)
		{
			gameWon();
		}
	}

	public void gameWon()
	{
		iTween.MoveTo(player,
			iTween.Hash(
				"position", finishPoint.transform.position,
				"time", 2,
				"easetype", "linear"
			)
		);
		finishUI.SetActive (true);
	}



}
