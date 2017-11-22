using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour
{
	public  GameObject   go_player;
	public  GameObject   go_startUI, go_gameWonUI, go_gameLostUI;
	public  GameObject   go_startPoint, go_playPoint, go_finishPoint;
	public  GameObject[] go_arr_light;
	public  GameObject[] go_arr_puzzleSphere;
	public  AudioSource  audioSource;
	public  AudioClip    failAudioClip;
	public  AudioClip    winAudioClip;
	private int[] arr_generatedSequence;
	private int[] arr_inputSequence;

	/// Random sequence animation speed
	public  float seqAnimSpeed;
	private int   flashLightsCounter;
	private int  currentIndex;
	public  int  totalChances;
	private int  chancesFinished;
	public  bool bool_takeInput = false;

	//   S T A R T   //                                                                                                
	void Start()
	{
		/// Declaring Arrays for random Sequences
		arr_generatedSequence = new int[go_arr_puzzleSphere.Length];
		arr_inputSequence     = new int[go_arr_puzzleSphere.Length];

		/// Move player to the start position
		go_player.transform.position = go_startPoint.transform.position;
	}
	
	//   B U T T O N S   //
	public void fn_startGame()
	{
		/// Disable Start menu
		go_startUI.SetActive(false);

		/// Start Game
		fn_initGame();
	}

	public void fn_restartGame()
	{
		/// Disable Restart Menu
		go_gameWonUI.SetActive(false);
		go_gameLostUI.SetActive(false);

		/// Start Game
		fn_initGame();
	}
	
	//   I N I T   //
	public void fn_initGame()
	{
		/*Debug.Log("Initiating Game");*/

		/// Boolean for disabling input while displaying the random sequence
		bool_takeInput = false;

		/// Move Player to the Play Area
		fn_movePlayerToPoint(go_playPoint.transform.position);

		/// Generating Random Sequence
		for (int i = 0; i < arr_generatedSequence.Length; i++)
		{
			arr_generatedSequence[i] = Random.Range(0, arr_generatedSequence.Length);
		}
		/* Debug.Log ("The puzzle sequence is " + arr_generatedSequence[0] + ", " + arr_generatedSequence[1] + ", "
											 + arr_generatedSequence[2] + ", " + arr_generatedSequence[3] + ", "
                                             + arr_generatedSequence[4]);*/
        /// Resetting number of failures. Game Lost of more than 3
        chancesFinished = 0;

        /// Flash lights - for player attention
        fn_setLightsActive(true);
        CancelInvoke("fn_flashLights");
        flashLightsCounter = 0;
        /// Call to turn lights on or off atlernatively - repeatedly
		InvokeRepeating("fn_flashLights", 1.0f, 0.29f);

        /// Animate Random Sequence
		CancelInvoke("fn_animSequence");
		currentIndex = 0;
		fn_dimmBalls();
		/// Call to show animation - one after another ball - repeatedly
		InvokeRepeating("fn_animSequence", 4.5f, seqAnimSpeed);
	}

	//   L I G H T S   //
	/// Disabling or enabling all spot lights at once
	private void fn_setLightsActive(bool bool_status)
	{
		for(int i = 0; i < go_arr_light.Length; i++)
		{
			go_arr_light[i].SetActive(bool_status);
		}
	}

	/// Flashing spot lights
	private void fn_flashLights()
	{
		fn_setLightsActive(!go_arr_light[0].activeSelf);
		flashLightsCounter++;
		/// Stop flashing lights after 3 flashes (6 for on and off both)
		if(flashLightsCounter >= 6)
		{
			CancelInvoke("fn_flashLights");
		}
	}

	//   B A L L S   //
	/// Stop glowing all balls at once
	private void fn_dimmBalls()
	{
		for(int i = 0; i < go_arr_puzzleSphere.Length; i++)
		{
			go_arr_puzzleSphere[i].GetComponent<puzzleSphere>().fn_dimm();
		}
	}

	/// Glow all balls once
	private void fn_glowBallsOnce()
	{
		for(int i = 0; i < go_arr_puzzleSphere.Length; i++)
		{
			go_arr_puzzleSphere[i].GetComponent<puzzleSphere>().fn_glowOnce();
		}
	}

	//   G A M E   L O G I C   //
	/// Animate Random Sequence 
	private void fn_animSequence()
	{
		/// Alternatively glow the ball in random sequence
		if(currentIndex % 2 == 0)
		{
			go_arr_puzzleSphere[arr_generatedSequence[currentIndex / 2]].GetComponent<puzzleSphere>().fn_glow(true);
		}
		/// Stop glowing the last ball
		else
			go_arr_puzzleSphere[arr_generatedSequence[currentIndex / 2]].GetComponent<puzzleSphere>().fn_dimm();

		/// counting number of times this function needs to be called
		currentIndex++;
		/// if counter exceeds double the number of balls (both on and off for glowing)
		if(currentIndex >= (go_arr_puzzleSphere.Length * 2))
		{
			/// Stop Animating Sequence
			CancelInvoke("fn_animSequence");
			/// Call next step (flashing all balls once) after animation finishes
			Invoke("fn_startTakingInput", 0.66f);
		}
	}

	/// Glow all balls once to show that the random sequence animation is finished
	private void fn_startTakingInput()
	{
		fn_glowBallsOnce();

		/// Call next step (taking input) after flasing all lights
		Invoke("fn_acceptInput", 0.33f);
	}

	/// Accept input
	private void fn_acceptInput()
	{
		currentIndex = 0;
		/// start taking input
		bool_takeInput = true;
		/*Debug.Log("Accepting Input");*/
	}

	/// This function is called when a ball is pressed - ball id is sent
	public void fn_registerPoint(int p_id)
	{
		/// store the ball id which is clicked
		arr_inputSequence[currentIndex] = p_id;
		/// count for number of inputs
		currentIndex++;

		/// if number of inputs are reached, then stop taking input and check if the input is matched
		if(currentIndex >= go_arr_puzzleSphere.Length)
		{
			/// Input sequence complete
			bool_takeInput = false;
			/*Debug.Log("Input complete: Stopped taking Input");*/
			/// Check input sequence
			fn_checkInput();
		}
	}

	/// Matching Input Sequence
	private void fn_checkInput()
	{
		for(int i = 0; i < go_arr_puzzleSphere.Length; i++)
		{
			/// If input is not matched in order, then wrong input
			if(arr_generatedSequence[i] != arr_inputSequence[i])
			{
				/// Play fail audio
				audioSource.clip = failAudioClip;
				audioSource.Play();
				/*Debug.Log("Input wrong");*/
				/// One chance finished
				chancesFinished++;
				/// If all chances are finished, then game lost
				if(chancesFinished >= totalChances)
					fn_gameOver(false);
				/// If there are more chances, start taking input
				else
					fn_startTakingInput();
				return;
			}
		}
		/*Debug.Log("Input Correct");*/
		/// If now rong matches, then input is correct, Game Won
		fn_gameOver(true);
	}
	
	//   P L A Y E R   M O V E M E N T   //
	/// iTween movement to a point
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
	
	//   G A M E   O V E R   //
	/// Game Over function
	private void fn_gameOver(bool bool_status)
	{
		/// Exit the room
		fn_movePlayerToPoint(go_finishPoint.transform.position);

		/// true is sent for game won
		if(bool_status)
		{
			/// Game won audio
			audioSource.clip = winAudioClip;
			audioSource.Play();
			/*Debug.Log("Game Won");*/

			/// Show game Won Restart Menu
			go_gameWonUI.SetActive(true);
		}
		/// False is recieved if game is lost
		else
		{
			/// Game Lost Restart Menu
			go_gameLostUI.SetActive(true);
		}
	}

	void Update()
	{
		/// To quit the application when X button is pressed
		if(Input.GetKeyDown(KeyCode.Escape))
		{
        	Application.Quit();
    	}
	}
}
