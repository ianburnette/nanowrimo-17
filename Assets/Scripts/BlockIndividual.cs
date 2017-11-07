using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockIndividual : MonoBehaviour {

    #region Private Variables
    [SerializeField] BlockType myType;
    [SerializeField] GridCoordinates myGridCoords;
    [SerializeField] SpriteRenderer[] sprites;
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
    #endregion

    #region Unity Functions
    void OnEnable()
    {
        StartCoroutine(Setup());
    }
	
	void Update () {
		
	}
#endregion

#region Custom Functions
    IEnumerator Setup()
    {
        yield return new WaitForEndOfFrame();
        myType = GlobalMembers.SelectRandomType();
        sprites[1].sprite = GlobalBlockBehavior.publicGlobalBlockBehavior.GetSprite(myType);
        yield return null;
    }
    public void Fade(bool state)
    {
        Color currentTargetColor = state ? GlobalBlockBehavior.publicGlobalBlockBehavior.FadeColor : GlobalBlockBehavior.publicGlobalBlockBehavior.NormalColor;
        foreach (SpriteRenderer sprite in sprites)
            sprite.color = currentTargetColor;
    }
#endregion
}
