using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmMovement : MonoBehaviour
{
    public Transform swarmTarget;
	private float swarmSpeed = .05f;
	private void Update()
	{
		transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z-swarmSpeed);
	}

	Vector3 CalculateSwarmTargetPosition()
	{
		return new Vector3(transform.position.x,transform.position.y,swarmTarget.position.z);
	}
}
