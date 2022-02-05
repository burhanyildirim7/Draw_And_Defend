using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DestroyCubes : MonoBehaviour
{
	public bool isEnable = true;
	private void Start()
	{
		DOTween.Init();
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("mesh") && isEnable)
		{
			isEnable = false;
			other.transform.parent.GetComponent<Rigidbody>().useGravity = false;
			other.transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
			MeshCrashGround(other.transform.parent.gameObject);
			Debug.Log("destroy çalýþtý...");
		}
		// GEREKÝRSE AÇILACAK.... DÜÞÜNÜLECEK..
		//if (other.CompareTag("mesh"))
		//{
		//	other.GetComponent<Collider>().enabled = false;
		//}

	    void MeshCrashGround(GameObject obj)
		{
			obj.transform.DOMove(new Vector3(obj.transform.position.x, -3.70f,obj.transform.position.z), .3f)
			.OnComplete(() =>
			{
				obj.transform.DOMove(new Vector3(obj.transform.position.x, -4.48f, obj.transform.position.z), .6f).SetEase(Ease.OutBounce)
				.OnComplete(()=> 
				{
					StartCoroutine(DestroyMesh(obj.gameObject));
				});
			});
		}

		IEnumerator DestroyMesh(GameObject obj)
		{
			yield return new WaitForSeconds(.5f);
			Destroy(obj);
			isEnable = true;
		}
	}
}
