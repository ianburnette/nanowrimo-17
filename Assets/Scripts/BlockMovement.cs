using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{

    #region Private Variables
    [SerializeField] PlayerInput playerInput;

    [Header("Dynamic Variables")]
    [SerializeField] GameObject currentlySelectedBlock;
    [SerializeField] BlockIndividual currentIndividualClass;
    [SerializeField] bool captureTouchPos, captureMousePos;

    [Header("UI Variables")]
    [SerializeField]
    Transform selectionBlockReference;
    [SerializeField] Vector2 offsetFromBlockCenter;
    [SerializeField] SpriteRenderer selectionReferenceSprite;
    #endregion

    #region Public Properties

    #endregion

    #region Unity Functions
    private void OnEnable()         //SUBSCRIBE TO RELEVANT INPUT EVENTS
    {
        PlayerInput.OnInputStarted += InputStart;
        PlayerInput.OnInputEnded += InputEnd;
    }
    private void OnDisable()        //UNSUBSCRIBE FROM INPUT EVENTS
    {
        PlayerInput.OnInputStarted -= InputStart;
        PlayerInput.OnInputEnded += InputEnd;
    }
    void Update()
    {
       
        //THE PLAYER IS DRAGGING A BLOCK AROUND - KEEP TRACK OF WHERE THAT IS AND HANDLE THE MOVEMENT
        if (currentlySelectedBlock != null)
        {
            if (captureMousePos)
                playerInput.UpdatePosition(PlayerInput.InputType.mouse);
            else if (captureTouchPos)
                playerInput.UpdatePosition(PlayerInput.InputType.touch);
            UpdateSelectionReference();
            HandleBlockMovement();
            if (!playerInput.StillReceivingInput())
                ReleaseBlock();
        }
    }
    #endregion

    #region Custom Functions

    #region Input Functions
    void InputStart()
    {
        HitAtPosition(playerInput.InputPositionWorld);
    }
    void InputEnd()
    {
        ReleaseBlock();
    }
    bool HitAtPosition(Vector2 pos)                      //THE USER TOUCHED/CLICKED - ARE THEY HITTING A BLOCK AT THAT LOCATION?
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector3.forward);
        if (hit != false && hit.collider != null)
        {
            SelectBlock(hit.transform.gameObject);
            ShowSelectionReference(hit.point, (Vector2)hit.transform.position);
            return true;
        }
        else
            return false;
    }
    #endregion

    #region Movement Behavior   

    void SelectBlock(GameObject block)                   //THE PLAYER HAS SELECTED A BLOCK - TELL THE BLOCK THAT IT'S BEEN SELECTED AND GET A REFERENCE TO IT
    {
        currentlySelectedBlock = block.transform.gameObject;
        currentIndividualClass = currentlySelectedBlock.GetComponent<BlockIndividual>();
        currentIndividualClass.Fade(true);
    }
    void HandleBlockMovement()                           //THE PLAYER IS DRAGGING THE BLOCK - HANDLE IT'S SNAPPING TO COLUMNS AND DISPLACING OTHER BLOCKS
    {
        Vector2 positionToSet = new Vector2(playerInput.InputPositionWorld.x, currentlySelectedBlock.transform.position.y);
        Vector2 curBlockPos = currentlySelectedBlock.transform.position;
        float differenceInPosition = positionToSet.x - curBlockPos.x;
        if (Mathf.Abs(differenceInPosition) > GridManagement.publicGrid.ColumnWidth)
        {
            if (positionToSet.x > curBlockPos.x) //we're moving to the right
                positionToSet.x = curBlockPos.x + GridManagement.publicGrid.ColumnWidth;
            else                                 //we're moving to the left
                positionToSet.x = curBlockPos.x - GridManagement.publicGrid.ColumnWidth;
        }
        else
            positionToSet.x = curBlockPos.x;
        currentlySelectedBlock.transform.position = positionToSet;
    }
    void ReleaseBlock()
    {                   //THE PLAYER HAS RELEASED THE SELECTED BLOCK - PLACE THE BLOCK IN THE GRID
                        //drop block code goes here
                        //send info to block here
        currentIndividualClass.Fade(false);
        currentlySelectedBlock = null;
        currentIndividualClass = null;
       
        HideSelectionReference();
    }
    #endregion
    #region Selection
    void ShowSelectionReference(Vector2 inputPosition, Vector2 selectedObjectPosition)  //THE PLAYER IS SELECTING A BLOCK - SHOW A UI INDICATOR TO SHOW WE'RE RECEIVING INPUT
    {
        selectionBlockReference.position = new Vector3(selectedObjectPosition.x, selectedObjectPosition.y, 0);
        selectionReferenceSprite.enabled = true;
        offsetFromBlockCenter = inputPosition - selectedObjectPosition;
    }
    void UpdateSelectionReference()                      //KEEP THE UI INDICATOR OF WHERE INPUT IS BEING RECEIVED MOVING AROUND WITH THE MOUSE/FINGER
    {
        Vector3 unadjustedPos = Camera.main.ScreenToWorldPoint(playerInput.InputPositionScreen + offsetFromBlockCenter);
        selectionBlockReference.position = new Vector3(unadjustedPos.x, unadjustedPos.y, 0f);
    }
    void HideSelectionReference()                        //HIDE THE UI INDICATOR OF INPUT RECEIPT ONCE INPUT ISN'T BEING RECEIVED
    {
        selectionReferenceSprite.enabled = false;
    }
    #endregion
    #endregion
}
