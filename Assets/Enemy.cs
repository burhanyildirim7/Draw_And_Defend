using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("mesh"))
		{
			GetComponent<Collider>().enabled = false;
			UIController.instance.EnemyScoreFunc(transform.position, transform.parent.gameObject);
			Destroy(gameObject);
		}else if (other.CompareTag("kale"))
		{
			// restart game için tutulamlý..
			transform.parent = null;
			StartCoroutine(RandomPosition());
			GetComponent<Collider>().enabled = false;
			GameController.instance.EnemiesOnCastle.Add(gameObject);
			
			// saldýrý yapýyor...
			GameController.instance.DecreaseCastleHealth(); 
			
		}
	}


	IEnumerator RandomPosition()
	{
		int sayac = 0;
		float xValue = Random.Range(-.02f, .02f);
		float zValue = Random.Range(-.01f, .01f);
		while(sayac < 100)
		{
			transform.position +=new Vector3(xValue,0,zValue);
			sayac++;
			yield return new WaitForSeconds(.02f);
		}
	}

}
