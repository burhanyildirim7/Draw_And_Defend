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
		if (other.CompareTag("parentCube") && isEnable)
		{
			isEnable = false;
			other.GetComponent<Collider>().enabled = false;
			other.GetComponent<Rigidbody>().useGravity = false;
			other.GetComponent<Rigidbody>().velocity = Vector3.zero;
			other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
			MeshCrashGround(other.gameObject);
		}
		// GEREKÝRSE AÇILACAK.... DÜÞÜNÜLECEK..
		//if (other.CompareTag("mesh"))
		//{
		//	other.GetComponent<Collider>().enabled = false;
		//}

		void MeshCrashGround(GameObject obj)
		{
			obj.transform.DOMove(new Vector3(obj.transform.position.x, transform.position.y+1.5f,obj.transform.position.z), .3f)
			.OnComplete(() =>
			{			
				obj.transform.DOMove(new Vector3(obj.transform.position.x, transform.position.y-.2f, obj.transform.position.z), .6f).SetEase(Ease.OutBounce)
				.OnComplete(()=> 
				{
					StartCoroutine(DestroyMesh(obj));
				});
			});
		}

		IEnumerator DestroyMesh(GameObject obj)
		{
			yield return new WaitForSeconds(.2f);
			Destroy(obj);
			isEnable = true;
			DrawMeshSbi.instance.ActivateDrawing();
		}
	}
}
