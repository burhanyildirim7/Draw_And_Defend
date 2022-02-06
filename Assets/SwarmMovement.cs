using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmMovement : MonoBehaviour
{
	private float swarmSpeed = .02f;
	public Transform hareketNoktasi1, hareketNoktasi2 , hareketNoktasi3;
	private bool isRotationTo1 = true;
	private bool isRotationTo2, isRotationTo3;

	private void Start()
	{
		RotationToHareketNoktasi1();
	}

	private void Update()
	{
		if (GameController.instance.isContinue && isRotationTo1)
			transform.position = Vector3.MoveTowards(
				transform.position, new Vector3(hareketNoktasi1.position.x, transform.position.y, hareketNoktasi1.position.z), swarmSpeed);

		else if (GameController.instance.isContinue && isRotationTo2)
			transform.position = Vector3.MoveTowards(
				transform.position, new Vector3(hareketNoktasi2.position.x, transform.position.y, hareketNoktasi2.position.z), swarmSpeed);

		else if(GameController.instance.isContinue && isRotationTo3)
			transform.position = Vector3.MoveTowards(
				transform.position, new Vector3(hareketNoktasi3.position.x, transform.position.y, hareketNoktasi3.position.z), swarmSpeed);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("hareket1"))
		{
			isRotationTo1 = false;
			isRotationTo2 = true;
			StartCoroutine(RotationToHareketNoktasi2());
		}else if (other.CompareTag("hareket2"))
		{
			isRotationTo2 = false;
			isRotationTo3 = true;
		}else if (other.CompareTag("kale"))
		{
			if(transform.childCount == 0)
			{
				GameController.instance.ActivateMeshCam();
				Destroy(gameObject);
			}
		}

	
	}


	void RotationToHareketNoktasi1()
	{
		transform.LookAt(hareketNoktasi1);
	}

	IEnumerator RotationToHareketNoktasi2()
	{
		transform.LookAt(hareketNoktasi2);
		yield return new WaitForSeconds(1);
	}
}
