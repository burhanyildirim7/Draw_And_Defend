using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmMovement : MonoBehaviour
{
	private float swarmSpeed = .04f;
	private void Update()
	{
		if(GameController.instance.isContinue)
		transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z-swarmSpeed);
	}

}
