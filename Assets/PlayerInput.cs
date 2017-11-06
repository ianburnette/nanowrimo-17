﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    #region Private Variables
    #region Events
    public delegate void StartedInput();
    public static event StartedInput OnInputStarted;

    //public delegate void InputPosition(Vector2 pos);
    //public static event InputPosition CurrentInput;

    public delegate void EndedInput();
    public static event EndedInput OnInputEnded;
    #endregion
    [SerializeField] Vector2 mousePosition;
    [SerializeField] Vector2 touchPosition;
    [SerializeField] Vector2 inputPositionScreen;
    [SerializeField] Vector2 inputPositionWorld;
    [SerializeField] bool captureMousePos, captureTouchPos;
    public InputType currentInputType;
    Touch currentTouch;
    public enum InputType { mouse, touch, none };
    #endregion

    #region Public Properties
    public Vector2 InputPositionWorld
    {
        get
        {
            return inputPositionWorld;
        }

        set
        {
            inputPositionWorld = value;
        }
    }

    public Vector2 InputPositionScreen
    {
        get
        {
            return inputPositionScreen;
        }

        set
        {
            inputPositionScreen = value;
        }
    }
    #endregion

    #region Unity Functions
    void Start () {
		
	}
	
	void Update () {
        //LISTEN FOR ANY INPUT
        if (Input.GetMouseButtonDown(0))
            ClickedWithMouse();
        if (Input.touches.Length > 0)
            if (Input.GetTouch(0).phase == TouchPhase.Began)
                TouchedWithFinger();
        if (captureMousePos)
            GetMousePos();
        else if (captureTouchPos)
            GetTouchPos();
        if (captureMousePos || captureTouchPos)
            StillReceivingInput();
    }
    #endregion

    #region Custom Functions
    void ClickedWithMouse()                              //THE USER HAS CLICKED - FIGURE OUT WHERE IT WAS AND START TRACKING IT
    {
        GetMousePos();
        currentInputType = InputType.mouse;
        captureMousePos = true;
        /*if (HitAtPosition(mousePosition))
            captureMousePos = true;
        else
            captureMousePos = false;*/
        OnInputStarted();
    }
    void TouchedWithFinger()                             //A TOUCH HAS BEGUN - FIGURE OUT WHERE IT IS AND START TRACKING IT
    {
        GetTouchPos();
        captureTouchPos = true;
        /*if (HitAtPosition(touchPosition))
            captureTouchPos = true;
        else
            captureTouchPos = false;*/
        OnInputStarted();
    }
    void GetMousePos()                                //GET THE LOCATION AT WHICH TO CAST A RAY FROM THE RECEIVED CLICK
    {
        Vector3 mousePos = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePos);
        inputPositionScreen = mousePosition;
        //CurrentInput(mousePosition);
    }
    void GetTouchPos()                                //GET THE LOCATION AT WHICH TO CAST A RAY FROM THE RECEIVED TOUCH
    {
        Vector3 touchPos = Input.GetTouch(0).position;
        touchPosition = Camera.main.ScreenToWorldPoint(touchPos);
        inputPositionScreen = touchPosition;
        //CurrentInput(touchPosition);
    }
    public void UpdatePosition(InputType currentInputType)      //KEEP TRACK OF THE RAW LOCATION OF THE CURRENT INPUT, TRANSLATE TO WORLD SPACE, AND BROADCAST
    {
        if (currentInputType == InputType.mouse)
            inputPositionScreen = Input.mousePosition;
        else
            inputPositionScreen = Input.GetTouch(0).position;
        inputPositionWorld = Camera.main.ScreenToWorldPoint(inputPositionScreen);
        //CurrentInput(inputPositionWorld);
    }
    public bool StillReceivingInput()                             //THE PLAYER IS TOUCHING/HOLDING THE MOUSE DOWN - WAIT FOR THEM TO STOP
    {
        if (currentInputType == InputType.mouse)
            if (Input.GetMouseButtonUp(0))
            {
                //ReleaseBlock();
                captureMousePos = false;
                currentInputType = InputType.none;
                return false;
            }

        if (currentInputType == InputType.touch)
        {
            currentTouch = Input.GetTouch(0);
            if (currentTouch.phase == TouchPhase.Ended)
            {
                currentInputType = InputType.none;
                return false;
            }
              //  ReleaseBlock();
            captureTouchPos = false;
        }
        return true;
    }
    #endregion
}
