using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevTools : MonoBehaviour
{
    
    public Text fpsCounterText;    
	private float fpsCount;

	IEnumerator Start()
	{
		while (true)
		{
			if (Time.timeScale == 1)
			{
				yield return new WaitForSeconds(0.1f);
				fpsCount = Mathf.RoundToInt(1 / Time.deltaTime);
			}
			fpsCounterText.text = "FPS: " + fpsCount;
			yield return new WaitForSeconds(0.5f);
		}
	}
}
