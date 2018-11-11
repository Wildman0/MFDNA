using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	public Tile nextTile;
	int[] tilesToEnd = new int[4];	//The number of tiles to the end for each player
	public bool isInEndZone;
	public Player.PlayerColor playerEndZoneColor;

}
