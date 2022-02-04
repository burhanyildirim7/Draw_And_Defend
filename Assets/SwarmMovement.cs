using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmMovement : MonoBehaviour
{
    public Transform swarmTarget;
	private float swarmSpeed = .05f;
	bool isMovementEnable = true;
	private void Update()
	{
		if(isMovementEnable)
		transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z-swarmSpeed);
	}

	Vector3 CalculateSwarmTargetPosition()
	{
		return new Vector3(transform.position.x,transform.position.y,swarmTarget.position.z);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("area")) isMovementEnable = false;
	}
}
