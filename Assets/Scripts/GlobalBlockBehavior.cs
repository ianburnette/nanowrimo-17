using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalBlockBehavior : MonoBehaviour {

    #region Private Variables
    public static GlobalBlockBehavior publicGlobalBlockBehavior;

    [Header("Selection Variables")]
    [SerializeField] Color fadeColor;
    [SerializeField] Color normalColor;

    [Header("Sprites")]
    [SerializeField] Sprite fireSprite;
    [SerializeField] Sprite iceSprite, ghostSprite, crateSprite, spiritSprite, waterSprite, woodSprite;

    #region Layers
    [Header("Layers")]
    [SerializeField] private LayerMask noneLayer;
    [SerializeField] private LayerMask fireLayer;
    [SerializeField] private LayerMask iceLayer;
    [SerializeField] private LayerMask ghostLayer;
    [SerializeField] private LayerMask crateLayer;
    [SerializeField] private LayerMask spiritLayer;
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] private LayerMask woodLayer;
    #endregion

    #endregion

    #region Public Properties
    public Color FadeColor
    {
        get
        {
            return fadeColor;
        }

        set
        {
            fadeColor = value;
        }
    }
    public Color NormalColor
    {
        get
        {
            return normalColor;
        }

        set
        {
            normalColor = value;
        }
    }
    public LayerMask NoneLayer
    {
        get
        {
            return noneLayer;
        }

        set
        {
            noneLayer = value;
        }
    }
    public LayerMask FireLayer
    {
        get
        {
            return fireLayer;
        }

        set
        {
            fireLayer = value;
        }
    }
    public LayerMask IceLayer
    {
        get
        {
            return iceLayer;
        }

        set
        {
            iceLayer = value;
        }
    }
    public LayerMask GhostLayer
    {
        get
        {
            return ghostLayer;
        }

        set
        {
            ghostLayer = value;
        }
    }
    public LayerMask CrateLayer
    {
        get
        {
            return crateLayer;
        }

        set
        {
            crateLayer = value;
        }
    }
    public LayerMask SpiritLayer
    {
        get
        {
            return spiritLayer;
        }

        set
        {
            spiritLayer = value;
        }
    }
    public LayerMask WaterLayer
    {
        get
        {
            return waterLayer;
        }

        set
        {
            waterLayer = value;
        }
    }
    public LayerMask WoodLayer
    {
        get
        {
            return woodLayer;
        }

        set
        {
            woodLayer = value;
        }
    }
    #endregion

    #region Unity Functions
    void Start () {
        publicGlobalBlockBehavior = this;
	}
	
	void Update () {
		
	}
#endregion

#region Custom Functions
    public Sprite GetSprite(BlockType type)
    {
        switch (type)
        {
            case BlockType.fire:
                return fireSprite;
                
            case BlockType.ice:
                return iceSprite;
                
            case BlockType.ghost:
                return ghostSprite;
                
            case BlockType.crate:
                return crateSprite;
                
            case BlockType.spirit:
                return spiritSprite;
                
            case BlockType.water:
                return waterSprite;
                
            case BlockType.wood:
                return woodSprite;
                
        }
        return fireSprite;
    }
#endregion
}
