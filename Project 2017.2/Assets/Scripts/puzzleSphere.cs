using System.Collections;
using UnityEngine;

public class puzzleSphere : MonoBehaviour
{
	private Renderer rend;
	public  Material mat_normal;
	public  Material mat_glow;
	public  int   id;
	public  AudioSource audioSource;

	public GameObject go_gameLogic;

	void Start()
	{
		/// Get teh renderer of the ball
		rend = GetComponent<Renderer>();
	}

	/// Function to glow the ball. True is sent to play audio along with it
	public void fn_glow(bool bool_status)
	{
		/// Change material to glow
		rend.material = mat_glow;
		/// Play ball hit audio if true
		if(bool_status)
			audioSource.Play();
	}

	/// Function to reset the material to normal
	public void fn_dimm()
	{
		rend.material = mat_normal;
	}

	/// This function glows the ball for 0.33 sec
	public void fn_glowOnce()
	{
		fn_glow(false);
		/// Reset back to normal after 0.33 sec
		Invoke("fn_dimm",0.33f);
	}

	/// When ball is clicked
	public void fn_clicked()
	{
		/// Accept input only when the game Logic sets te flag to accept input
		if(go_gameLogic.GetComponent<GameLogic>().bool_takeInput == true)
		{
			/*Debug.Log(this.gameObject.name + ": clicked");*/
			/// Call the Register point function in gameLogic
			go_gameLogic.GetComponent<GameLogic>().fn_registerPoint(this.id);
			audioSource.Play();
		}
	}

	/// When player points at the ball
	public void fn_playerStartedLooking()
	{
		/// Glow only when thje gameLogic accepts input
		if(go_gameLogic.GetComponent<GameLogic>().bool_takeInput == true)
		{
			fn_glow(false);
			/*Debug.Log(this.gameObject.name + ": Player started looking at me");*/
		}
	}

	/// When player stops looking at the ball
	public void fn_playerStoppedLooking()
	{
		/// Back to normal
		if(go_gameLogic.GetComponent<GameLogic>().bool_takeInput == true)
		{
			fn_dimm();
			/*Debug.Log(this.gameObject.name + ": Player stopped looking at me");*/
		}
	}
}
