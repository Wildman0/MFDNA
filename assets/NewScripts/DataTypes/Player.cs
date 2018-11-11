using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
	public Player.PlayerColor color;
	public string playerName;

	public enum PlayerColor
	{
		Blue,
		Green,
		Red,
		Yellow
	};

	//Constructor
	public Player(Player.PlayerColor _color, string _playerName)
	{
		color = _color;
		playerName = _playerName;
	}
}
