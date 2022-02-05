using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCubes : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("parentCube"))
		{
			StartCoroutine(MeshCrashEvents(other.gameObject));
			Debug.Log("destroy çalýþtý...");
		}

		IEnumerator MeshCrashEvents(GameObject obj)
		{
			Debug.Log("ÇALIÞTI ASLINLDAAA");
			//obj.transform.parent.GetComponent<Rigidbody>().velocity = new Vector3(0,24,0);
			obj.transform.parent.GetComponent<Rigidbody>().AddForce(new Vector3(0, 200, 0));
			yield return new WaitForSeconds(1f);
			obj.transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
			obj.transform.parent.GetComponent<Rigidbody>().useGravity = false;
			yield return new WaitForSeconds(2f);
			Destroy(obj);
		}
	}
}
