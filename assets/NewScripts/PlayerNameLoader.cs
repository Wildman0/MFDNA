using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameLoader : MonoBehaviour
{

	public Text[] playerNamesOnUI;
	private PlayerNameSetter playerNameSetter;

	private void Start ()
	{
		playerNameSetter =
			GameObject.FindGameObjectWithTag ("NameInput").GetComponent<PlayerNameSetter> ();

		SetUINames();
	}

	private void SetUINames ()
	{
		for (int i = 0; i < playerNamesOnUI.Length; i++)
		{
			playerNamesOnUI[i].text = playerNameSetter.playerNameStrings[i];
		}
	}
}
