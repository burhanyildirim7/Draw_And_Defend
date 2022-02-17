using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraContoller : MonoBehaviour
{
    public static CameraContoller instance;
	public GameObject cmVcam;

	private void Awake()
	{
		if (instance == null) instance = this;
		else Destroy(this);
	}

	public void DeactivateCinemachine()
	{
		cmVcam.SetActive(false);
	}

	public void ActivateCinemachine()
	{
		cmVcam.SetActive(true);
	}

}
