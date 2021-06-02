using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class igniteBrazero : MonoBehaviour
{
	public GameObject brazero_light;
	public GameObject brazero_particles;
	public GameObject torch_light;
	public GameObject torch_particles;
	private void OnTriggerStay(Collider other)
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			if (torch_light.activeSelf)
			{
				if (!brazero_light.activeSelf)
				{
					//lancer animation
					brazero_light.SetActive(true);
					brazero_particles.SetActive(true);
				}
			}
			else
			{
				if (brazero_light.activeSelf)
				{
					//lancer animation
					torch_light.SetActive(true);
					torch_particles.SetActive(true);
				}
			}
		}
		//else allumer torche
	}
}
