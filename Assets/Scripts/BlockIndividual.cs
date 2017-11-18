using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BlockIndividual : MonoBehaviour {

    #region Private Variables
    [SerializeField] BlockType myType;
    [SerializeField] GridCoordinates myGridCoords;
    [SerializeField] SpriteRenderer[] sprites;
    [SerializeField] bool inMatch = false;
    [SerializeField] LayerMask blockMatchingLayer, blockPhysicsLayer;
    [SerializeField] bool showRays;
    [SerializeField] Ray2D[] rays;
    [Tooltip("up=0, down=1; left=2; right=3")]
    [SerializeField] bool[] matchesInDirections;
    [SerializeField] float rayOffset;
    [SerializeField] float rayLength;
    
    #endregion

    #region Public Properties
    public BlockType MyType
    {
        get
        {
            return myType;
        }

        set
        {
            myType = value;
            ChangeLayerMask(myType);
        }
    }
        public GridCoordinates MyGridCoords
    {
        get
        {
            return myGridCoords;
        }

        set
        {
            myGridCoords = value;
        }
    }
        public bool InMatch
    {
        get
        {
            return inMatch;
        }

        set
        {
            inMatch = value;
        }
    }
    #endregion

    #region Unity Functions
    void OnEnable()
    {
        StartCoroutine(Setup());
    }
    private void Start()
    {
        rays = new Ray2D[4];
        rays[0] = new Ray2D((Vector2)transform.position + new Vector2(0, rayOffset), Vector2.up * rayLength);
        rays[1] = new Ray2D((Vector2)transform.position + new Vector2(0, -rayOffset), Vector2.down * rayLength);
        rays[2] = new Ray2D((Vector2)transform.position + new Vector2(-rayOffset, 0), Vector2.left * rayLength);
        rays[3] = new Ray2D((Vector2)transform.position + new Vector2(rayOffset, 0), Vector2.right * rayLength);
    }
    void Update () {
        CheckForGravity();
        CheckForMatches();
	}
    private void OnDisable()
    {
        inMatch = false;
    }
    #endregion

    #region Custom Functions
    IEnumerator Setup()
    {
        yield return new WaitForEndOfFrame();
        MyType = GlobalMembers.SelectRandomType();
        sprites[1].sprite = GlobalBlockBehavior.publicGlobalBlockBehavior.GetSprite(myType);
        for (int i = 0; i < matchesInDirections.Length; i++)
            matchesInDirections[i] = false;
        yield return null;
    }
    public void Fade(bool state)
    {
        Color currentTargetColor = state ? GlobalBlockBehavior.publicGlobalBlockBehavior.FadeColor : GlobalBlockBehavior.publicGlobalBlockBehavior.NormalColor;
        foreach (SpriteRenderer sprite in sprites)
            sprite.color = currentTargetColor;
    }
    void CheckForGravity()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(rays[1].origin, rays[1].direction, rayLength, blockPhysicsLayer);
        /*
         * we can maybe use 2D physics here, but we have to be sure that we're also registering the changes with the grid manager, otherwise the grid will get all screwed up
         * */
    }
    public void CheckForMatches()
    {
        rays[0] = new Ray2D((Vector2)transform.position + new Vector2(0, rayOffset), new Vector2(0, rayLength));
        rays[1] = new Ray2D((Vector2)transform.position + new Vector2(0, -rayOffset), new Vector2(0, -rayLength));
        rays[2] = new Ray2D((Vector2)transform.position + new Vector2(-rayOffset, 0), new Vector2(-rayLength, 0));
        rays[3] = new Ray2D((Vector2)transform.position + new Vector2(rayOffset, 0), new Vector2(rayLength, 0));
        if (showRays)
        {
            for (int i = 0; i < rays.Length; i++)
            {
                Debug.DrawRay(rays[i].origin, rays[i].direction * rayLength, Color.red);
            }
        }
        if (!inMatch)
        {
            for (int i = 0; i < rays.Length; i++)
            {
                RaycastHit2D hit;
                hit = Physics2D.Raycast(rays[i].origin, rays[i].direction, rayLength, blockMatchingLayer);
                if (hit.transform != null)
                {
                    inMatch = true;
                    GridDirection dir = GetDirectionOfHit(i);
                    SetMatchInDirection(i);
                    HitBlockInDirection(dir, i);
                }
            }
        }
    }
    public void SetMatchInDirection(int dir)
    {
        matchesInDirections[dir] = true;
    }
    void HitBlockInDirection(GridDirection dir, int dirRaw)
    {
        GridManagement.publicGrid.GridMovementQuery(myGridCoords, dir).blockInCell.SetMatchInDirection(GetOppositeDirection(dirRaw));
    }
    GridDirection GetDirectionOfHit(int inputDir)
    {
        switch (inputDir)
        {
            case 0:
                return GridDirection.up;
            case 1:
                return GridDirection.down;
            case 2:
                return GridDirection.left;
            case 3:
                return GridDirection.right;
            default:
                return GridDirection.none;
        }
    }
    int GetOppositeDirection(int incomingDir)
    {
        switch (incomingDir)
        {
            case 0: return 1;
            case 1: return 0;
            case 2: return 3;
            case 3: return 2;
            default: return 0;
        }
    }
    void ChangeLayerMask(BlockType newType)
    {
        switch (newType)
        {
            case BlockType.none:
                blockMatchingLayer = GlobalBlockBehavior.publicGlobalBlockBehavior.NoneLayer;
                break;
            case BlockType.fire:
                blockMatchingLayer = GlobalBlockBehavior.publicGlobalBlockBehavior.FireLayer;
                break;
            case BlockType.ice:
                blockMatchingLayer = GlobalBlockBehavior.publicGlobalBlockBehavior.IceLayer;
                break;
            case BlockType.ghost:
                blockMatchingLayer = GlobalBlockBehavior.publicGlobalBlockBehavior.GhostLayer;
                break;
            case BlockType.crate:
                blockMatchingLayer = GlobalBlockBehavior.publicGlobalBlockBehavior.CrateLayer;
                break;
            case BlockType.spirit:
                blockMatchingLayer = GlobalBlockBehavior.publicGlobalBlockBehavior.SpiritLayer;
                break;
            case BlockType.water:
                blockMatchingLayer = GlobalBlockBehavior.publicGlobalBlockBehavior.WaterLayer;
                break;
            case BlockType.wood:
                blockMatchingLayer = GlobalBlockBehavior.publicGlobalBlockBehavior.WoodLayer;
                break;
        }
    }
#endregion
}
