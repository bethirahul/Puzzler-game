using UnityEngine;
using System.Collections;
using UnityEditor;

public class GameLogic : MonoBehaviour
{
	public  GameObject   go_player;
	public  GameObject   go_startUI, go_finishUI;
	public  GameObject   go_startPoint, go_playPoint, go_finishPoint;
	public  GameObject[] go_arr_light;
	public  GameObject[] go_arr_puzzleSphere;
	private int[] arr_sequenceOrder;
	public  float seqAnimSpeed;
	private int flashLightsCounter;

	private int  currentIndex;
	/// private bool isBallGlowing = false;

	//   S T A R T   //                                                                                                
	void Start()
	{
		/// Update 'player' to be the camera's parent gameobject, i.e. GvrEditorEmulator, instead of the camera itself.
		/// Required because GVR resets camera position to 0, 0, 0.
		/// player = player.transform.parent.gameObject;

		arr_sequenceOrder = new int[go_arr_puzzleSphere.Length];

		/// Move player to the start position. 
		go_player.transform.position = go_startPoint.transform.position;
	}
	
	//   B U T T O N S   //
	public void fn_startGame()
	{
		go_startUI.SetActive(false);
		fn_initGame();
	}

	public void fn_restartGame()
	{
		go_finishUI.SetActive(false);
		fn_initGame();
	}
	
	//   I N I T   //
	private void fn_initGame()
	{
		Debug.Log("Initiating Game");
		/// Move Player to the Play Area.
		fn_movePlayerToPoint(go_playPoint.transform.position);

		/// Generating Random Sequence
		for (int i = 0; i < arr_sequenceOrder.Length; i++)
		{
			arr_sequenceOrder[i] = Random.Range(0, arr_sequenceOrder.Length);
		}
		Debug.Log ("The puzzle sequence is " + arr_sequenceOrder[0] + ", " + arr_sequenceOrder[1] + ", "
											 + arr_sequenceOrder[2] + ", " + arr_sequenceOrder[3] + ", "
                                             + arr_sequenceOrder[4]);

        /// Flash lights - for player attention
        fn_setLightsActive(true);
        CancelInvoke("fn_flashLights");
        flashLightsCounter = 0;
		InvokeRepeating("fn_flashLights", 2.0f, 0.29f);

        /// Animate Sequence
		CancelInvoke("fn_animSequence");
		currentIndex = 0;
		InvokeRepeating("fn_animSequence", 4.5f, seqAnimSpeed);
	}

	//   L I G H T S   //
	private void fn_setLightsActive(bool bool_status)
	{
		for(int i = 0; i < go_arr_light.Length; i++)
		{
			go_arr_light[i].SetActive(bool_status);
		}
	}

	private void fn_flashLights()
	{
		fn_setLightsActive(!go_arr_light[0].activeSelf);
		flashLightsCounter++;
		if(flashLightsCounter >= 6)
		{
			CancelInvoke("fn_flashLights");
		}
	}

	//   B A L L S   //
	private void fn_glowBalls(bool bool_status)
	{
		for(int i = 0; i < go_arr_puzzleSphere.Length; i++)
		{
			go_arr_puzzleSphere[i].GetComponent<puzzleSphere>().fn_resetColor();
		}
	}

	private void fn_animSequence()
	{
		fn_glowBalls(false);

		if(currentIndex%2 == 0)
			go_arr_puzzleSphere[arr_sequenceOrder[currentIndex/2]].GetComponent<puzzleSphere>().fn_glow();

		currentIndex++;
		if(currentIndex >= (go_arr_puzzleSphere.Length*2))
			CancelInvoke("fn_animSequence");
	}

	/*public void fn_showSequence()
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

	}*/

	/*private void fn_glowBall(int ballIndex)
	{
		/// go_arr_puzzleSphere[ballindex].fn_glow(true);
	}*/

	/*private void fn_dimmAllSpheres()
	{
		for (int i = 0; i < numOfSpheres; i++)
		{
			/// go_arr_puzzleSphere[i].fn_glow(false);
		}
	}*/
	
	//   P L A Y E R   M O V E M E N T   //
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

	//   U P D A T E   //                                                                                              
	void Update()
	{
		if (Input.GetMouseButtonDown(0) && go_player.transform.position == go_playPoint.transform.position)
		{
			fn_gameWon();
		}
	}
	
	//   W I N   //
	private void fn_gameWon()
	{
		fn_movePlayerToPoint(go_finishPoint.transform.position);
		go_finishUI.SetActive (true);
	}
}
