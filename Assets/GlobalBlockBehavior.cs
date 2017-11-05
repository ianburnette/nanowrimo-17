using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalBlockBehavior : MonoBehaviour {

    #region Private Variables
    public static GlobalBlockBehavior publicGlobalBlockBehavior;

    [Header("Selection Variables")]
    [SerializeField] Color fadeColor;
    [SerializeField] Color normalColor;
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

#endregion
}
