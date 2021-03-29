using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : Info {

    public bool IsAI = false;
    public bool IsHuman = false;
    public GameObject newPawnInstance;
       
    /// <summary>
    /// Player Number for Grabbing Input
    /// </summary>
    public int InputPlayerNumber = 0;

    /// <summary>
    /// Player Number in the game
    /// </summary>
    public int PlayerNumber = 0;

    protected Pawn ControlledPawn;

    public GameObject SpectatorInstance;

    protected virtual void Start()
    {
        //SpectatorInstance = gameObject.AddComponent<Spectator>();

        ControlPawn(SpectatorInstance);
    
    }

    public Pawn GetPawn()
    {
        return ControlledPawn; 
    }

    public virtual bool ControlPawn(Pawn p)
    {
        
        if (!(p.BecomeControlledBy(this)))
         {
            LOG_ERROR("Controler - Possession Failure"); 
            return false; 
        }

        // If we have a Possessed Pawn already, Call Unpossess on it. 
        if (ControlledPawn)
        {
            ControlledPawn.BecomeReleased();
        }

        ControlledPawn = p; 
        return true; 
    }

    /// <summary>
    /// PossesPawn version taking GameObject with Pawn Script attached to it. 
    /// </summary>
    /// <param name="PawnGameObject">Game Object with Pawn Script Attached to it</param>
    /// <returns></returns>
    public virtual bool ControlPawn(GameObject PawnGameObject)
    {
        Pawn p = PawnGameObject.GetComponent<Pawn>(); 
        if (!p)
        {
            LOG_ERROR("GameObject " + PawnGameObject.name + " is not a pawn.");
            return false; 
        }
        return ControlPawn(p);
    }

    public virtual bool ReleasePawn(Pawn p)
    {
        //Debug.Log("Release Pawn");

        p.BecomeReleased(); 

        ControlledPawn = null;
        return true;
    }

    public virtual bool RequestSpectator()
	{
        ReleasePawn(ControlledPawn);

        ControlPawn(SpectatorInstance);

        //Debug.Log("Request Spectator " + SpectatorInstance);

        return true;
	}

    public virtual bool RequestSpawn()
	{
        //Debug.Log("Request Spawn");

        return true;
	}
}
