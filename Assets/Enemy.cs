using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("mesh"))
		{
			Debug.Log("meshe capt�kk");
			Destroy(gameObject);
		}
	}

}
