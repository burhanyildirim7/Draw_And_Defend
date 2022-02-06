using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HareketNoktasi : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		if (transform.CompareTag("hareket1")) Gizmos.color = Color.green;
		else if(transform.CompareTag("hareket2")) Gizmos.color = Color.blue;
		else if(transform.CompareTag("hareket3")) Gizmos.color = Color.red;

		Gizmos.DrawSphere(transform.position, 1);

	}
}
