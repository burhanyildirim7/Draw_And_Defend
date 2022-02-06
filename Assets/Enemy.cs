using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("mesh"))
		{
			Debug.Log("meshe captýkk");
			Destroy(gameObject);
		}else if (other.CompareTag("kale"))
		{
			// restart game için tutulamlý..
			GetComponent<Collider>().enabled = false;
			GameController.instance.DeactivatedObjects.Add(other.gameObject);
			transform.parent = null;
			// saldýrý yapýyor...
			GameController.instance.DecreaseCastleHealth();
			
		}
	}

}
