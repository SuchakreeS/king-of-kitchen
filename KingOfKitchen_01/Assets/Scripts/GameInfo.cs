﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using UnityEngine.SceneManagement;

public class GameInfo : MonoBehaviour {
    //-------------------------------------------------------------------
    // Initial variable
    //-------------------------------------------------------------------
    public Text score_Text;         // Text
    public Text distanceL_Text;
    public Text distanceR_Text;
    public Text movableL_Text;
    public Text movableR_Text;
    public Text holdL_Text;
    public Text holdR_Text;
    public Text time_Text;
    public Text mode_Text;

    public Object hit_Obj;          // Prefab resources
    public GameObject hand_Obj;
    public GameObject Laser_Obj;
    public GameObject particle_food;
    public AudioSource main_Sound;
    public AudioSource eat_Good;
    public AudioSource eat_Bad;
    public AudioSource select_Sound;

    Color green_Color;              // Material
    Color red_Color;
    
    public int currentScore;        // Common
    public int modeID;              // Simple Ray-Casting number: 0, GoGo number: 1
    public float time_Limit;
    public float time;
    public float maxRange;
    public float hand_Limit;
    public float movable_Limit;
    public float jumpable_Limit;
    public float collect_Limit;
    public float velocity_Limit;
    public float force_Limit;
    public bool holdL;
    public bool holdR;
    public bool jump;
    public bool status;
    public string[] handType = {"Any", "Left", "Right"};
    public string[] modeType = { "Ray-Casting", "Homer"};

    //--------------------------------------------------------------------
    // Function
    //--------------------------------------------------------------------
    void Start()
    {
        
        green_Color = new Color(0, 255, 0);     // load Material
        red_Color = new Color(255, 0, 0);

        modeID = 1;                             // set initial values
        currentScore = 0;
        time_Limit = 90;
        time = time_Limit;
        maxRange = 100f;
        hand_Limit = 0.7f;
        movable_Limit = 1000000f;
        collect_Limit = 1000000f;
        jumpable_Limit = 20f;
        velocity_Limit = 20f;
        force_Limit = 100f;
        holdL = false;
        holdR = false;
        jump = false;
        status = true;
        UpdateMode();
        //add main sound here
        main_Sound.Play();

        DontDestroyOnLoad(transform.gameObject);
    }

    void Update()
    {
        UpdateTimer();
        // Switch Technique
        if (Input.GetKeyDown("m") || GetGrabGripDown(0))
        {
            UpdateMode();
        }

        if (status == false)
        {
            SceneManager.LoadScene("End_Scene");
        }

    } 

    public void AddScore(int score)
    {
        if (score >= 0)
        {
            eat_Good.Play();
        }
        else
        {
            eat_Bad.Play();
        }
        currentScore += score;
        UpdateScore();
    }

    public void UpdateScore()
    {
        score_Text.text = currentScore.ToString();
    }

    public void UpdateDistance(float distance, int handType)
    {
        if (handType == 1)
        {
            distanceL_Text.text = distance.ToString();
        }
        else if (handType == 2)
        {
            distanceR_Text.text = distance.ToString();
        }
    }

    public void UpdateMovable(bool movable, int handType)
    {
        if (handType == 1)
        {
            if (movable)
            {
                movableL_Text.text = "True";
                movableL_Text.color = green_Color;
            }
            else
            {
                movableL_Text.text = "False";
                movableL_Text.color = red_Color;
            }
        }
        else if (handType == 2)
        {
            if (movable)
            {
                movableR_Text.text = "True";
                movableR_Text.color = green_Color;
            }
            else
            {
                movableR_Text.text = "False";
                movableR_Text.color = red_Color;
            }
        }
    }

    public void UpdateHold(bool hold, int handType)
    {
        if (handType == 1)
        {
            if (hold)
            {
                holdL_Text.text = "True";
                holdL_Text.color = green_Color;
                holdL = true;
            }
            else
            {
                holdL_Text.text = "False";
                holdL_Text.color = red_Color;
                holdL = false;
            }
        }
        else if (handType == 2)
        {
            if (hold)
            {
                holdR_Text.text = "True";
                holdR_Text.color = green_Color;
                holdR = true;
            }
            else
            {
                holdR_Text.text = "False";
                holdR_Text.color = red_Color;
                holdR = false;
            }
        }
    }

    public bool GetGrabDown(int handType)
    {
        if (handType == 0)
        {
            return SteamVR_Input._default.inActions.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any);
        }
        else if (handType == 1)
        {
            return SteamVR_Input._default.inActions.GrabPinch.GetStateDown(SteamVR_Input_Sources.LeftHand);
        }
        else if (handType == 2)
        {
            return SteamVR_Input._default.inActions.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand);
        }
        else
        {
            return false;
        }
    }

    public bool GetGrabGripDown(int handType)
    {
        if (handType == 0)
        {
            return SteamVR_Input._default.inActions.GrabGrip.GetStateDown(SteamVR_Input_Sources.Any);
        }
        else if (handType == 1)
        {
            return SteamVR_Input._default.inActions.GrabGrip.GetStateDown(SteamVR_Input_Sources.LeftHand);
        }
        else if (handType == 2)
        {
            return SteamVR_Input._default.inActions.GrabGrip.GetStateDown(SteamVR_Input_Sources.RightHand);
        }
        else
        {
            return false;
        }
    }

    public bool GetGrabUp(int handType)
    {
        if (handType == 0)
        {
            return SteamVR_Input._default.inActions.GrabPinch.GetStateUp(SteamVR_Input_Sources.Any);
        }
        else if (handType == 1)
        {
            return SteamVR_Input._default.inActions.GrabPinch.GetStateUp(SteamVR_Input_Sources.LeftHand);
        }
        else if (handType == 2)
        {
            return SteamVR_Input._default.inActions.GrabPinch.GetStateUp(SteamVR_Input_Sources.RightHand);
        }
        else
        {
            return false;
        }
    }

    public bool GetTeleportDown(int handType)
    {
        if (handType == 0)
        {
            return SteamVR_Input._default.inActions.Teleport.GetStateDown(SteamVR_Input_Sources.Any);
        }
        else if (handType == 1)
        {
            return SteamVR_Input._default.inActions.Teleport.GetStateDown(SteamVR_Input_Sources.LeftHand);
        }
        else if (handType == 2)
        {
            return SteamVR_Input._default.inActions.Teleport.GetStateDown(SteamVR_Input_Sources.RightHand);
        }
        else
        {
            return false;
        }
    }

    public bool GetTeleportUp(int handType)
    {
        if (handType == 0)
        {
            return SteamVR_Input._default.inActions.Teleport.GetStateUp(SteamVR_Input_Sources.Any);
        }
        else if (handType == 1)
        {
            return SteamVR_Input._default.inActions.Teleport.GetStateUp(SteamVR_Input_Sources.LeftHand);
        }
        else if (handType == 2)
        {
            return SteamVR_Input._default.inActions.Teleport.GetStateUp(SteamVR_Input_Sources.RightHand);
        }
        else
        {
            return false;
        }
    }

    public float FindDistance(Vector3 s, Vector3 d)
    {
        return Vector3.Distance(s, d);
    }

    public float FindDistanceIgnoreY(Vector3 s, Vector3 d)
    {
        Vector3 sNew = new Vector3(s.x, 0f, s.z);
        Vector3 dNew = new Vector3(d.x, 0f, d.z);
        return Vector3.Distance(sNew, dNew);
    }
    
    public void UpdateTimer()
    {
        time -= Time.deltaTime;
        time_Text.text = time.ToString("F2") + " s";
        if (time <= 0)
        {
            status = false;
        }
    }

    public void UpdateMode()
    {
        modeID = (modeID == 0) ? 1 : 0;
        mode_Text.text = modeType[modeID];
    }

}
