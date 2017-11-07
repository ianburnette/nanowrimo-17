using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{

    #region Private Variables
    [Header("References")]
    [SerializeField] PlayerInput playerInput;
    [SerializeField] ColumnManagement columnManagement;

    [Header("Dynamic Variables")]
    [SerializeField] GameObject currentlySelectedBlock;
    [SerializeField] BlockIndividual currentIndividualClass;
    [SerializeField] bool captureTouchPos, captureMousePos;


    [Header("Behavior Variables")]
    [SerializeField] float columnChangeMargin;

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
    private void Start()
    {
       // columnChangeMargin = GridManagement.publicGrid.ColumnWidth;
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
            EvaluateBlockMovement();
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
      //  print("checking position " + pos);
        RaycastHit2D hit = Physics2D.Raycast(new Vector3(pos.x, pos.y, -.5f), Vector3.forward);
        if (hit != false && hit.collider != null)
        {
            SelectBlock(hit.transform.gameObject);
            ShowSelectionReference(hit.point, (Vector2)hit.transform.position);
            return true;
        }
        else
        {
          //  Debug.DrawRay(new Vector3(pos.x, pos.y, -.5f), Vector3.forward, Color.red);
          //  EditorApplication.isPaused = true;
            return false;
        }
            
    }
    #endregion

    #region Movement Behavior   

    void SelectBlock(GameObject block)                   //THE PLAYER HAS SELECTED A BLOCK - TELL THE BLOCK THAT IT'S BEEN SELECTED AND GET A REFERENCE TO IT
    {
        currentlySelectedBlock = block.transform.gameObject;
        currentIndividualClass = currentlySelectedBlock.GetComponent<BlockIndividual>();
        currentIndividualClass.Fade(true);
    }
    void EvaluateBlockMovement()                           //THE PLAYER IS DRAGGING THE BLOCK - HANDLE IT'S SNAPPING TO COLUMNS AND DISPLACING OTHER BLOCKS
    {
        Vector2 positionToSet = new Vector2(playerInput.InputPositionWorld.x, currentlySelectedBlock.transform.position.y);
        Vector2 curBlockPos = currentlySelectedBlock.transform.position;
        float differenceInPosition = positionToSet.x - curBlockPos.x;
        if (Mathf.Abs(differenceInPosition) > columnChangeMargin)
        {
            if (positionToSet.x > curBlockPos.x && positionToSet.x < GridManagement.publicGrid.FinalColumnXPosition)          //we're moving to the right
                positionToSet.x = curBlockPos.x + GridManagement.publicGrid.ColumnWidth;
            else if (positionToSet.x < curBlockPos.x && positionToSet.x > GridManagement.publicGrid.FirstColumnXPosition)
            {     //we're moving to the left
           //     print("minimum is " + GridManagement.publicGrid.FirstColumnXPosition + " and this blocks x position is " + positionToSet.x);
                positionToSet.x = curBlockPos.x - GridManagement.publicGrid.ColumnWidth;
            }
        }
        else
            positionToSet.x = curBlockPos.x;

        if (positionToSet.x != curBlockPos.x)//we're going to move the block in some direction
        {
            BlockIndividual currentBlockScript = currentlySelectedBlock.GetComponent<BlockIndividual>();
            //the block is placed partially in the cell at this point - it displaces other blocks but doesn't yet match with others

            int newBlockColumn = positionToSet.x > curBlockPos.x ? (int)currentBlockScript.MyGridIndex.x + 1 : (int)currentBlockScript.MyGridIndex.x - 1;
            columnManagement.CategorizeBlock((int)currentBlockScript.MyGridIndex.y, newBlockColumn, currentlySelectedBlock.GetComponent<BlockIndividual>());
            PlaceBlockInNewPosition(positionToSet);     
        }
    }

    public void DisplaceBlock(Block blockToMove, Vector2 positionToMoveTo)
    {
        BlockIndividual displacedBlockScript = blockToMove.myGameObject.GetComponent<BlockIndividual>();
        Vector2 positionToSet = blockToMove.myGameObject.transform.position;
        if (positionToMoveTo.x > displacedBlockScript.MyGridIndex.x)
            positionToSet.x = positionToSet.x + GridManagement.publicGrid.ColumnWidth;
        else
            positionToSet.x = positionToSet.x - GridManagement.publicGrid.ColumnWidth;
        
        BlockPlaced(currentIndividualClass);
    }

    void PlaceBlockInNewPosition(Vector2 positionToSet)
    {
        currentlySelectedBlock.transform.position = positionToSet;
    }
    void ReleaseBlock()                                     //THE PLAYER HAS RELEASED THE SELECTED BLOCK - PLACE THE BLOCK IN THE GRID
    {
        BlockPlaced(currentIndividualClass);
        currentIndividualClass.Fade(false);
        currentlySelectedBlock = null;
        currentIndividualClass = null;
       
        HideSelectionReference();
    }
    void BlockPlaced(BlockIndividual blockThatHasBeenPlaced)
    {
        //this block is officially in the grid now - check if it has any nearby matches
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
