using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    public GameObject HUDSpace;
    public GameObject SpectatorHUD;
    public GameObject PawnHUD;

    /// <summary>
    /// Show Input for this controler, Super Spammy when true. 
    /// </summary>
    public bool LogInputStateInfo = false;

    protected InputPoller inputPoller;
    protected InputState InputCurrent;
    protected InputState InputPrevious;


    protected override void Start()
    {
        base.Start();
        IsHuman = true;

        inputPoller = InputPoller.Self;
        if (!inputPoller)
        {
            LOG_ERROR("****PLAYER CONTROLER: No Input Poller in Scene");
            return;
        }
    }

    protected void Update()
    {
        GetInput();
        ProcessInput();
        InputPrevious = InputCurrent;
    }

    protected virtual void GetInput()
    {
        if (!inputPoller)
        {
            LOG_ERROR("****PLAYER CONTROLER (" + gameObject.name + "): No Input Poller in Scene");
            return;
        }

        InputCurrent = InputPoller.Self.GetPlayerInput(InputPlayerNumber);

        if (LogInputStateInfo)
        {
            LOG(InputCurrent.ToString());
        }
    }

    protected virtual void ProcessInput()
    {
        if (!ControlledPawn)
        {
            // If we don't have a pawn, don't bother processing input. 
            return;
        }

        if (InputCurrent.ButtonNorth)
        {
            Fire1(InputCurrent.ButtonNorth);
        }

        if (InputCurrent.ButtonSouth)
        {
            Fire2(InputCurrent.ButtonSouth);
        }

        if (InputCurrent.ButtonEast)
        {
            Fire3(InputCurrent.ButtonEast);
        }

        if (InputCurrent.ButtonWest)
        {
            Fire4(InputCurrent.ButtonWest);
        }

        Look();
        Move(InputCurrent.HorizontalLeft, InputCurrent.VerticalLeft);
    }

    public virtual void Move(float x, float z)
    {
        Pawn pawn = ((Pawn)ControlledPawn);
        if (pawn)
        {
            pawn.Move(x, z);
        }
    }

    public virtual void Look()
    {
        Pawn pawn = ((Pawn)ControlledPawn);
        if (pawn)
        {
            pawn.Look();
        }
    }

    public virtual void Fire1(bool value)
    {
        if (value)
        {
            LOG("Del-Fire1");
            Pawn pawn = ((Pawn)ControlledPawn);
            if (pawn)
            {
                pawn.Fire1(value);
            }
        }
    }

    public virtual void Fire2(bool value)
    {
        if (value)
        {
            LOG("Del-Fire2");
            Pawn pawn = ((Pawn)ControlledPawn);
            if (pawn)
            {
                pawn.Fire2(value);
            }
        }
    }

    public virtual void Fire3(bool value)
    {
        if (value)
        {
            LOG("Del-Fire3");
            Pawn pawn = ((Pawn)ControlledPawn);
            if (pawn)
            {
                pawn.Fire3(value);
            }
        }
    }

    public virtual void Fire4(bool value)
    {
        if (value)
        {
            LOG("Del-Fire4");
            Pawn pawn = ((Pawn)ControlledPawn);
            if (pawn)
            {
                pawn.Fire4(value);
            }
        }
    }

    public override bool RequestSpectator()
    {
        ReleasePawn(ControlledPawn);
        ControlPawn(SpectatorInstance);

        return base.RequestSpectator();
    }

    public override bool RequestSpawn()
    {
        ReleasePawn(ControlledPawn);

        Game GM = GameObject.FindObjectOfType<Game>();

        Spawnpoint spawn = GM.ReturnSpawn();

        GameObject newPawn = Factory(newPawnInstance, spawn.Location, spawn.Rotation);

        ControlPawn(newPawn);

        return base.RequestSpawn();
    }
}