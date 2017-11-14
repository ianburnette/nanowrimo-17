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
