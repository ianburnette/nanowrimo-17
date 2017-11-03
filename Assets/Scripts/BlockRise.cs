using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRise : MonoBehaviour {

    #region Private Variables
    [SerializeField] float raiseSpeed;
    #endregion  

#region Public Properties

#endregion

#region Unity Functions
	void Update () {
        transform.position += (Vector3)Vector2.up * raiseSpeed * Time.deltaTime * Time.deltaTime;
	}
#endregion

#region Custom Functions

#endregion
}
