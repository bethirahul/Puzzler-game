using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour
{
	public GameObject player;
	public GameObject startUI, finishUI;
	public GameObject startPoint, playPoint, finishPoint;
	public GameObject[] puzzleSphere;
	private int numOfSpheres = 5;
	private int[] sequenceOrder;
	public float puzzleAnimSpeed = 1.0f;

	void Start()
	{
		// Update 'player' to be the camera's parent gameobject, i.e. GvrEditorEmulator, instead of the camera itself.
		// Required because GVR resets camera position to 0, 0, 0.
		// player = player.transform.parent.gameObject;

		sequenceOrder = new int[numOfSpheres];

		// Move player to the start position.
		player.transform.position = startPoint.transform.position;
	}

	public void startGame()
	{
		startUI.SetActive(false);
		initGame ();
	}

	public void restartGame()
	{
		finishUI.SetActive(false);
		initGame ();
	}

	private void initGame()
	{
		movePlayerToPoint(playPoint.transform.position);
		for (int i = 0; i < numOfSpheres; i++)
		{
			sequenceOrder[i] = Random.Range(0, numOfSpheres);
		}
		Debug.Log ("The puzzle sequence is " + sequenceOrder[0] + ", " + sequenceOrder[1] + ", " + sequenceOrder[2] + ", " + sequenceOrder[3] + ", " + sequenceOrder[4]);
	}

	private void movePlayerToPoint(Vector3 point)
	{
		iTween.MoveTo
		(
			player,
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
		if (Input.GetMouseButtonDown(0) && player.transform.position == playPoint.transform.position)
		{
			gameWon();
		}
	}

	private void gameWon()
	{
		movePlayerToPoint(finishPoint.transform.position);
		finishUI.SetActive (true);
	}
}
