using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashingImage : MonoBehaviour
{
	public Image flashingImage;
	private Color color1 = Color.red;
	private Color color2 = Color.yellow;

	public float lastTimeFlashed;
	public float flashGap = 0.5f;

	void Update()
	{
		if (Time.time > lastTimeFlashed + flashGap)
		{
			lastTimeFlashed = Time.time;
			ToggleColor();
		}
	}

	void ToggleColor()
	{
		flashingImage.color = flashingImage.color == color1 ? color2 : color1;
	}
}
