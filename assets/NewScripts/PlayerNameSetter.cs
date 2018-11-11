using UnityEngine;
using UnityEngine.UI;

public class PlayerNameSetter : MonoBehaviour {
	
	public InputField[] playerNameInputFields;
	public string[] playerNameStrings = new string[4];

	private void Start()
	{
		DontDestroyOnLoad(this);
	}

	private void Update()
	{
		if (playerNameInputFields[0] != null)
		{
			SetPlayerNames();
		}
		else
		{
			//Is not in the main menu
		}
	}

	//Sets the player name strings based on the input fields
    private void SetPlayerNames()
    {
	    for (int i = 0; i < playerNameInputFields.Length; i++)
	    {
			playerNameStrings[i] = playerNameInputFields[i].text;
	    }
    }
}
