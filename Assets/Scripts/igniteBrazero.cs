using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class igniteBrazero : MonoBehaviour
{
	public GameObject light;
	public GameObject particles;
	private void OnTriggerStay(Collider other)
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
		//if torche allumé
			if (!light.activeSelf)
			{
				//lancer animation
				light.SetActive(true);
				particles.SetActive(true);
			}
		}
		//else allumer torche
	}
}
