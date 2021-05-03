using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinimapController : MonoBehaviour
{
	public bool exactPosition = false;
	public List<GameObject> objectsOnRadar;
	public List<GameObject> RadarNotches;
	public Color NotchColor;
	public int maxCheckDistance = 15;
	public int minCheckDistance = 5;

	PlayerPawn Player;
	Transform Target;

	public float distanceToTarget;

	private void Update()
	{
		//Get player. Set up for only a single player -- More testing is needed for MP
		Player = FindObjectOfType<PlayerPawn>();

		//Swap radar from exact or relative -- for testing purposes
		if (Input.GetKeyDown(KeyCode.E))
		{
			if (exactPosition == true)
			{
				exactPosition = false;
			}
			else if (exactPosition == false)
			{
				exactPosition = true;
			}
		}

		//Clear list to update objects that can be rendered - for when enemies die or spawn
		objectsOnRadar.Clear();
		List<GameObject> Radar = new List<GameObject>(objectsOnRadar);

		Radar.AddRange(GameObject.FindGameObjectsWithTag("render_on_radar"));

		//Used for displaying items in the list in the editor
		objectsOnRadar = Radar;

		//Add objects to list
		foreach (GameObject notch in RadarNotches)
		{
			//Image nothcImage = notch.GetComponent<Image>();
			//nothcImage.color = new Color(255, 255, 255, 0);
		}
	}

	void LateUpdate()
	{
		//Check if radar is for exact position or relative
		if (exactPosition == true)
		{
			foreach (GameObject pawn in objectsOnRadar)
			{
				//If the pawn is to be rendered on the radar
				if (pawn.tag.Contains("render_on_radar"))
				{
					//If the pawn is not the player
					if (!pawn.GetComponent<PlayerPawn>())
					{
						//Turn on sprite for exact position
						pawn.GetComponent<SpriteRenderer>().enabled = true;
					}
				}
			}
		}
		else
		{
			foreach (GameObject pawn in objectsOnRadar)
			{
				//If the pawn is to be rendered on the radar
				if (pawn.tag.Contains("render_on_radar"))
				{
					//If the pawn is not the player
					if (!pawn.GetComponent<PlayerPawn>())
					{
						//Turn of sprite for exact position
						pawn.GetComponent<SpriteRenderer>().enabled = false;

						//Get parent transform of pawn target object
						Target = pawn.GetComponentInParent<Transform>().parent;

						//Get relative position between target and player
						Vector3 relativePos = Target.gameObject.transform.position - Player.gameObject.transform.position;

						//Get player forward vector
						Vector3 forward = Player.gameObject.transform.forward;

						//Get angle from player forward and enemy position
						float angle = Vector3.SignedAngle(relativePos, forward, Vector3.up);

						//Get distance to target
						distanceToTarget = Vector3.Distance(Target.position, Player.gameObject.transform.position);

						//Activate Radar Notches if there are any
						if (RadarNotches.Count != 0)
						{
							//Ontop
							if (distanceToTarget < minCheckDistance)
							{
								RadarNotches[0].SetActive(true);
							}

							//Front
							if (angle > -22.5 && angle < 22.5 && distanceToTarget < maxCheckDistance && distanceToTarget > minCheckDistance)
							{
								RadarNotches[1].SetActive(true);
							}

							//Left Back
							if (angle > 157.5 && distanceToTarget < maxCheckDistance && distanceToTarget > minCheckDistance)
							{
								RadarNotches[2].SetActive(true);
							}
							//Right Back
							else if (angle < -157.5 && distanceToTarget < maxCheckDistance && distanceToTarget > minCheckDistance)
							{
								RadarNotches[2].SetActive(true);
							}


							//Left
							if (angle > 67.5 && angle < 112.5 && distanceToTarget < maxCheckDistance && distanceToTarget > minCheckDistance)
							{
								RadarNotches[3].SetActive(true);
							}	

							//Right
							if (angle < -67.5 && angle > -112.5 && distanceToTarget < maxCheckDistance && distanceToTarget > minCheckDistance)
							{
								RadarNotches[4].SetActive(true);
							}

							//Front-Left
							if (angle > 22.5 && angle < 67.5 && distanceToTarget < maxCheckDistance && distanceToTarget > minCheckDistance)
							{
								RadarNotches[5].SetActive(true);
							}

							//Front-Right
							if (angle < -22.5 && angle > -67.5 && distanceToTarget < maxCheckDistance && distanceToTarget > minCheckDistance)
							{
								RadarNotches[6].SetActive(true);
							}

							//Back-Left
							if (angle > 112.5 && angle < 157.5 && distanceToTarget < maxCheckDistance && distanceToTarget > minCheckDistance)
							{
								RadarNotches[7].SetActive(true);
							}

							//Back-Right
							if (angle < -112.5 && angle > -157.5 && distanceToTarget < maxCheckDistance && distanceToTarget > minCheckDistance)
							{
								RadarNotches[8].SetActive(true);
							}
						}
					}
				}
			}
		}
	}

	private void FixedUpdate()
	{
		//Deactivate Radar Notches
		foreach (GameObject notch in RadarNotches)
		{
			if (RadarNotches.Count != 0)
			{
				Image notchImg = notch.GetComponent<Image>();

				notchImg.color = NotchColor;
				notch.SetActive(false);
			}
		}
	}
}
