using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class GameManager : MonoBehaviour
{
	public static int playerCount = 4;
	public static Player currentPlayer;
	public static Player[] players;
	public static GamePiece[] gamePieces;
	private PlayerNameSetter playerNameSetter;

	public Text[] _playerNames = new Text[playerCount];
	public static Text[] playerNames;
	public static Button diceRollButton;

	public Text _winText;
	public static Text winText;

	public Button _returnToMenuButton;
	public static Button returnToMenuButton;

	//Runs at start
	private void Start()
	{
		playerNameSetter =
			GameObject.FindGameObjectWithTag ("NameInput").GetComponent<PlayerNameSetter> ();

		players = new Player[playerCount];
		playerNames = _playerNames;
		winText = _winText;
		returnToMenuButton = _returnToMenuButton;

		diceRollButton = GetComponent<Dice>().button;

		GetGamePieces ();
		CreatePlayers ();
		SelectPlayer(0);

		returnToMenuButton.gameObject.SetActive(false);
	}

	//Runs every frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			NextPlayerTurn();
		}
	}

	//Gets references to all game pieces in the scene
	private void GetGamePieces()
	{
		GameObject[] gamePiecesGameObjects = GameObject.FindGameObjectsWithTag("GamePiece");
		gamePieces = new GamePiece[gamePiecesGameObjects.Length];

		for (int i = 0; i < gamePiecesGameObjects.Length; i++)
		{
			gamePieces[i] = gamePiecesGameObjects[i].GetComponent<GamePiece> ();
		}
	}

	//Creates player classes
	private void CreatePlayers()
	{
		for (int i = 0; i < playerCount; i++)
		{
			players[i] = new Player((Player.PlayerColor) i, playerNameSetter.playerNameStrings[i]);
			Debug.Log(players[i] + " created with color: " + players[i].color + " and name: " + players[i].playerName);
		}
	}

	//Goes to the next player's turn
	public static void NextPlayerTurn()
	{
		int currentPlayerIndex = Array.IndexOf(players, currentPlayer);

		diceRollButton.interactable = true;

		if (currentPlayerIndex != playerCount - 1)
		{
			SelectPlayer(currentPlayerIndex + 1);
		}
		else
		{
			SelectPlayer (0);
		}

		SelectionManager.selectedGamePiece = null;
		DisableAllPlayerHighlighting();

		print (currentPlayer.color + "'s turn");
	}

	//Selects a player at the given index in the player array
	public static void SelectPlayer(int index)
	{
		currentPlayer = players[index];
		HighlightCurrentPlayerName();
	}
	
	//Disables all gamepiece highlighting
	public static void DisableAllPlayerHighlighting()
	{
		for (int i = 0; i < gamePieces.Length; i++)
		{
			gamePieces[i].SetSelectable(false);
		}
	}

	//Enables the highlight zone on all selectable gamepieces
	public static void EnableSelectableGamepieceHighlighting()
	{
		for (int i = 0; i < gamePieces.Length; i++)
		{
			if (gamePieces[i].gamePieceColor == currentPlayer.color && gamePieces[i].CanMoveWithRoll (Dice.number))
				gamePieces[i].SetSelectable(true);
		}
	}

	//Highlights the name of the current player and grays the others out
	public static void HighlightCurrentPlayerName()
	{
		for (int i = 0; i < playerCount; i++)
		{
			playerNames[i].color = Color.gray;
		}

		playerNames[Array.IndexOf(players, currentPlayer)].color = Color.white;
	}

	//Returns any gamepiece that's on a given tile
	public static GamePiece[] FindGamePiecesOnTile(Tile tile)
	{
		var list = new List<GamePiece>();

		for (int i = 0; i < gamePieces.Length; i++)
		{
			if (gamePieces[i].currentTile == tile)
			{
				list.Add(gamePieces[i]);
			}
		}

		return list.ToArray();
	}

	public static void CheckGameWinState()
	{
		int[] gamePiecesInEndZoneCount = new int[playerCount];

		for (int i = 0; i < gamePieces.Length; i++)
		{
			if (gamePieces[i].isInEndZone)
				gamePiecesInEndZoneCount[(int) gamePieces[i].gamePieceColor]++;
		}

		//TODO: ADD A UI ELEMENT POPUP HERE
		for (int i = 0; i < gamePiecesInEndZoneCount.Length; i++)
		{
			if (gamePiecesInEndZoneCount[i] == 4)
			{
				print(GameManager.players[i].playerName + "has won!");
				winText.enabled = true;
				winText.text = GameManager.players[i].playerName + " has won!";
				returnToMenuButton.gameObject.SetActive (true);
			}
		}
	}

	public void ReturnToMenu()
	{
		SceneManager.LoadScene(0);
	}
}
