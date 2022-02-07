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
			MeshCrashGround(other.gameObject);
		}
		// GEREKÝRSE AÇILACAK.... DÜÞÜNÜLECEK..
		if (other.CompareTag("mesh"))
		{
			other.GetComponent<Collider>().enabled = false;
		}

		void MeshCrashGround(GameObject obj)
		{
			obj.transform.DOMove(new Vector3(obj.transform.position.x, transform.position.y+1.5f,obj.transform.position.z), .3f)
			.OnComplete(() =>
			{
				//StartCoroutine(GameController.instance.DelayAndActivateMeshCam());			
				obj.transform.DOMove(new Vector3(obj.transform.position.x, transform.position.y+.6f, obj.transform.position.z), .6f).SetEase(Ease.OutBounce)
				.OnComplete(()=> 
				{
					StartCoroutine(DestroyMesh(obj));
					//GameController.instance.ActivateMeshCam();
				});
			});
		}

		IEnumerator DestroyMesh(GameObject obj)
		{
			GameController.instance.ActivateMeshCam();
			yield return new WaitForSeconds(.2f);
			Destroy(obj);
			isEnable = true;
			//if (!GameController.instance.inputListener) GameController.instance.inputListener.SetActive(true);
		}
	}
}
