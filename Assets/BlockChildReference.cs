using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockChildReference : MonoBehaviour {

    #region Private Variables
    public static BlockChildReference publicBlockReference;
    [SerializeField] Color fadeColor, normalColor;
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
    void OnEnable () {
        publicBlockReference = this;
	}
	
	void Update () {
		
	}
#endregion

#region Custom Functions

#endregion
}
