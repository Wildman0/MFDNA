using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour {

    public Text NumberText;
	public Button button;
    public static int number;

	//Rolls the dice and gives a random number between 1 and 6
    public void RollDice()
    {
        System.Random random = new System.Random();
        number = random.Next(1, 7);
        NumberText.text = "You rolled a " + number.ToString();
	    button.interactable = false;

		//Updates the selection highlighting based on the dice roll
	    GameManager.EnableSelectableGamepieceHighlighting();
    }
}
