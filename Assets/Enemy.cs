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
		}else if (other.CompareTag("kale"))
		{
			// restart game i�in tutulaml�..
			GetComponent<Collider>().enabled = false;
			GameController.instance.DeactivatedObjects.Add(other.gameObject);
			transform.parent = null;
			// sald�r� yap�yor...
			GameController.instance.DecreaseCastleHealth();
			
		}
	}

}
