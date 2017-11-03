using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour {

    #region Private Variables
    [SerializeField] Vector3 mousePosition, touchPosition;
    [SerializeField] GameObject currentlySelectedBlock;
    enum InputType { mouse, touch, none };
    InputType currentInputType;
    Touch currentTouch;
#endregion

#region Public Properties

#endregion

#region Unity Functions
	void Start () {
		
	}
	
	void Update () {
        if (Input.GetMouseButtonDown(0))
            ClickedWithMouse();
        if (Input.touches.Length > 0)
            if (Input.GetTouch(0).phase == TouchPhase.Began)
                TouchedWithFinger();
        if (currentlySelectedBlock != null)
        {
            HandleBlockMovement();
            ListenForInputEnd();
        }
	}
    #endregion

    #region Custom Functions
    Vector3 GetMouseRay()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    Vector3 GetTouchRay()
    {
        return Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
    }
    void ClickedWithMouse()
    {
        mousePosition = GetMouseRay();
        currentInputType = InputType.mouse;
        InputAtPosition(mousePosition);
    }
    void TouchedWithFinger()
    {
        touchPosition = GetTouchRay();
        InputAtPosition(touchPosition);
    }
    void InputAtPosition(Vector3 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        if (hit != false && hit.collider != null)
        {
            currentlySelectedBlock = hit.transform.gameObject;
            print("I'm hitting " + hit.collider.name);
        }
    }
    void HandleBlockMovement()
    {

    }
    void ListenForInputEnd()
    {
       
        if (currentInputType == InputType.mouse)
            if (Input.GetMouseButtonUp(0))
                ReleaseBlock();
        if (currentInputType == InputType.touch)
        {
            currentTouch = Input.GetTouch(0);
            if (currentTouch.phase == TouchPhase.Ended)
                ReleaseBlock();
        }
    }
    void ReleaseBlock() { 

         //drop block code goes here
         //send info to block here
        currentlySelectedBlock = null;
        currentInputType = InputType.none;
       
    }
    #endregion
}
