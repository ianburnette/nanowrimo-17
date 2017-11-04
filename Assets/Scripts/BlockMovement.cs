using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour {

    #region Private Variables
    [Header("Input")]
    [SerializeField] Vector3 mousePosition;
    [SerializeField] Vector3 touchPosition;
    [SerializeField] Vector2 inputPosition;
    enum InputType { mouse, touch, none };

    [Header("Dynamic Variables")]
    InputType currentInputType;
    Touch currentTouch;
    [SerializeField] GameObject currentlySelectedBlock;
    [SerializeField] BlockIndividual currentIndividualClass;
    [SerializeField] bool captureTouchPos, captureMousePos;

    [Header("UI Variables")]
    [SerializeField] Transform selectionBlockReference;
    [SerializeField] Vector2 offsetFromBlockCenter;
    [SerializeField] SpriteRenderer selectionReferenceSprite;



    #endregion

    #region Public Properties

    #endregion

    #region Unity Functions
    void Start() {

    }

    void Update() {
        if (Input.GetMouseButtonDown(0))
            ClickedWithMouse();
        if (Input.touches.Length > 0)
            if (Input.GetTouch(0).phase == TouchPhase.Began)
                TouchedWithFinger();
        if (currentlySelectedBlock != null)
        {
            if (captureMousePos)
                UpdatePosition(InputType.mouse);
            else if (captureTouchPos)
                UpdatePosition(InputType.touch);
            UpdateSelectionReference();
            HandleBlockMovement();
            ListenForInputEnd();
        }
    }
    #endregion

    #region Custom Functions
    #region Input Functions
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
        captureMousePos = true;
    }
    void TouchedWithFinger()
    {
        touchPosition = GetTouchRay();
        InputAtPosition(touchPosition);
        captureTouchPos = true;
    }
    void InputAtPosition(Vector3 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        if (hit != false && hit.collider != null)
        {
            SelectBlock(hit.transform.gameObject);
            ShowSelectionReference((Vector2)pos, (Vector2)hit.transform.position);
            print("I'm hitting " + hit.collider.name);
        }
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
    #endregion
    #region Movement Behavior


    void SelectBlock(GameObject block)
    {
        currentlySelectedBlock = block.transform.gameObject;
        currentIndividualClass = currentlySelectedBlock.GetComponent<BlockIndividual>();
        currentIndividualClass.Fade(true);


    }
    void HandleBlockMovement()
    {

    }
    void ReleaseBlock() {

        //drop block code goes here
        //send info to block here
        currentIndividualClass.Fade(false);
        currentlySelectedBlock = null;
        currentIndividualClass = null;
        currentInputType = InputType.none;
        HideSelectionReference();
    }
    #endregion
    #region Selection
    void ShowSelectionReference(Vector2 inputPosition, Vector2 selectedObjectPosition)
    {
        selectionBlockReference.position = new Vector3(selectedObjectPosition.x, selectedObjectPosition.y, 0);
        selectionReferenceSprite.enabled = true;
        offsetFromBlockCenter = inputPosition - selectedObjectPosition;
    }
    void UpdateSelectionReference()
    {
        Vector3 unadjustedPos = Camera.main.ScreenToWorldPoint(inputPosition + offsetFromBlockCenter);
        selectionBlockReference.position = new Vector3(unadjustedPos.x, unadjustedPos.y, 0f);
    }
    void HideSelectionReference()
    {
        selectionReferenceSprite.enabled = false;
    }
    void UpdatePosition(InputType currentInputType)
    {
        if (currentInputType == InputType.mouse)
            inputPosition = Input.mousePosition;
        else
            inputPosition = Input.GetTouch(0).position;
    }
    #endregion      
    #endregion
}
