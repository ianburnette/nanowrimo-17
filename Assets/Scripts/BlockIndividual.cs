using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockIndividual : MonoBehaviour {

    #region Private Variables
    
    [SerializeField] SpriteRenderer[] sprites;
#endregion

#region Public Properties

#endregion

#region Unity Functions
	void Start () {
		
	}
	
	void Update () {
		
	}
#endregion

#region Custom Functions
    public void Fade(bool state)
    {
        Color currentTargetColor = state ? BlockChildReference.publicBlockReference.FadeColor : BlockChildReference.publicBlockReference.NormalColor;
        foreach (SpriteRenderer sprite in sprites)
            sprite.color = currentTargetColor;
    }
#endregion
}
