using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class manages player input on a per player basis. 
/// This class is used by the PlayerControler class to provide Input information. 
/// Inherit this class to define player inputs. Support for all 16 players in Unity Supported. 
/// An Example for Player 1 is provided in this class. 
/// </summary>
public class InputPoller : Info {

	/// <summary>
	/// Internal Static Reference 
	/// </summary>
	protected static InputPoller _Self;

	/// <summary>
	/// Public Interface to Applications's Single Reference of this class. 
	/// </summary>
	public static InputPoller Self
	{
		get { return _Self; } 
	}

	/// <summary>
	/// Initalizes the Sington Reference. 
	/// </summary>
	private void Awake()
	{
		if (_Self)
		{
			Debug.LogError("Multiple Input Poller Classes Exist. This is a singleton object and only one should exist EVER.");
			Debug.LogError("Deleting Duplicated Instance from " + gameObject.name);
			Destroy(this); 
			return; 
		}
		_Self = this; 
	}

	public virtual InputState GetPlayerInput(int PlayerNumber)
	{
		InputState newInputState = new InputState();

		if (PlayerNumber == 0) { GetPlayer1Input(newInputState); }
		if (PlayerNumber == 1) { GetPlayer2Input(newInputState); }
		if (PlayerNumber == 2) { GetPlayer3Input(newInputState); }
		if (PlayerNumber == 3) { GetPlayer4Input(newInputState); }

		return newInputState; 
	}

	/// <summary>
	/// Input Setup Method for Specific Player. Add Implementation in Inherited Class for Application
	/// </summary>
	/// <returns>InputState for Requested Player</returns>
	public virtual void GetPlayer1Input(InputState newInputState)
	{
		newInputState.HorizontalLeft = Input.GetAxis("Horizontal");
		newInputState.VerticalLeft = Input.GetAxis("Vertical");
		newInputState.ButtonNorth = Input.GetButtonDown("Fire1");
		newInputState.ButtonSouth = Input.GetButtonDown("Fire2");
		newInputState.ButtonEast = Input.GetButtonDown("Fire3");
		newInputState.ButtonWest = Input.GetButtonDown("Fire4");
	}

	/// <summary>
	/// Input Setup Method for Specific Player. Add Implementation in Inherited Class for Application
	/// </summary>
	/// <returns>InputState for Requested Player</returns>
	public virtual void GetPlayer2Input(InputState newInputState)
	{
		newInputState.HorizontalLeft = Input.GetAxis("P2Horizontal");
		newInputState.VerticalLeft = Input.GetAxis("P2Vertical");
		newInputState.ButtonNorth = Input.GetButtonDown("P2Fire1");
		newInputState.ButtonSouth = Input.GetButtonDown("P2Fire2");
		newInputState.ButtonEast = Input.GetButtonDown("P2Fire3");
		newInputState.ButtonWest = Input.GetButtonDown("P2Fire4");
	}

	/// <summary>
	/// Input Setup Method for Specific Player. Add Implementation in Inherited Class for Application
	/// </summary>
	/// <returns>InputState for Requested Player</returns>
	public virtual void GetPlayer3Input(InputState newInputState)
	{

	}

	/// <summary>
	/// Input Setup Method for Specific Player. Add Implementation in Inherited Class for Application
	/// </summary>
	/// <returns>InputState for Requested Player</returns>
	public virtual void GetPlayer4Input(InputState newInputState)
	{

	}
}

