using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
	public Player.PlayerColor gamePieceColor;
	public bool isAtStartTile = true;
	public bool isInEndZone;
	public Tile currentTile;
	public Tile targetTile;

	private Actions animationActions;

	public Tile endZoneEntranceTile;
	public Tile firstEndZoneTile;
	public Tile startTile;

	public bool isSelectable;
	public bool isMoving;
	public GameObject selectionMarker;

	private float moveSpeed = 18f;

	//Runs on initialization
	private void Awake()
	{
		selectionMarker = transform.Find("SelectionMarker").gameObject;
		animationActions = GetComponent<Actions>();

		if (selectionMarker == null)
			Debug.LogError("Was unable to find selection marker for " + gameObject);
		else
			selectionMarker.GetComponent<Renderer>().enabled = false;
	}

	void Start()
	{
		Respawn ();
	}

	public void Update()
	{
		if (isMoving)
		{
			MovePlayerTowardsTile(targetTile);

			if (IsEnemyGamePieceOnTile(targetTile))
			{
				if (Vector3.Distance(transform.position, targetTile.transform.position) < 1.25f)
				{
					isMoving = false;

					for (int i = 0; i < GameManager.FindGamePiecesOnTile (targetTile).Length; i++)
					{
						if (GameManager.FindGamePiecesOnTile(targetTile)[i].gamePieceColor != gamePieceColor)
						{
							animationActions.Attack();
							GameManager.FindGamePiecesOnTile (targetTile)[i].Kill(this);
						}	
					}
				}
			}
			else
			{
				if (transform.position == targetTile.transform.position)
				{
					isMoving = false;
					animationActions.Stay ();
				}
			}
		}
	}

	public void Kill(GamePiece killer)
	{
		StartCoroutine (KillEnumerator (killer));
	}

	private IEnumerator KillEnumerator(GamePiece killer)
	{
		animationActions.Death();
		yield return new WaitForSeconds(2.5f);
		animationActions.animator.SetTrigger("Respawn");
		Respawn();

		killer.transform.position = killer.targetTile.transform.position;
		killer.animationActions.Stay();
	}

	//Moves the game piece to the tile
	public void Move(Tile tile)
	{
		//Move towards the correct tile at a given speed
		//Play the running animation while you do this

		if (GameManager.FindGamePiecesOnTile(tile) != null)
		{
			//GameManager.FindGamePiecesOnTile (tile).Respawn();
		}

		animationActions.Run();

		transform.LookAt(tile.transform.position);
		targetTile = tile;
		isMoving = true;

		isAtStartTile = false;
		currentTile = tile;

		if (tile.isInEndZone)
			isInEndZone = true;

		GameManager.CheckGameWinState();

		GameManager.DisableAllPlayerHighlighting();
	}

	private void MovePlayerTowardsTile(Tile tile)
	{
		transform.position = Vector3.MoveTowards (transform.position, tile.gameObject.transform.position, moveSpeed * Time.deltaTime);
	}

	//Checks if the piece is able to move with a given dice roll (can't if the tile is occupied by a friendly gamepiece)
	public bool CanMoveWithRoll(int roll)
	{
		Tile proposedTile = CalculateTileWouldLandOnWithRoll(roll);

		//The calculated tile is null for various reasons, not allowed to move
		if (proposedTile == null)
			return false;

		//Player hasn't rolled a 6 and is in the start area, not allowed to move
		if (roll != 6 && isAtStartTile)
			return false;

		//There's another friendly gamepiece on this tile, not allowed to move
		for (int i = 0; i < GameManager.gamePieces.Length; i++)
		{
			if (GameManager.gamePieces[i].gamePieceColor == gamePieceColor)
			{
				if (GameManager.gamePieces[i].currentTile == proposedTile)
				{
					return false;
				}
			}
		}

		return true;
	}

	//Works out which tile the player would land on with a given roll. Returns null if the dice roll goes too far
	public Tile CalculateTileWouldLandOnWithRoll(int roll)
	{
		Tile tile = (isAtStartTile) ? null : currentTile;

		for (int i = 0; i < roll; i++)
		{
			//if the gamepiece isn't currently on the board, move it onto it and remove 1 from roll
			if (tile == null)
			{
				tile = startTile;
				roll -= 1;
			}

			if (tile == endZoneEntranceTile)
			{
				tile = firstEndZoneTile;
			}
			else
			{
				if (tile.nextTile != null)
				{
					tile = tile.nextTile;
				}
				else
				{
					return null;
				}
			}
		}
		
		return tile;
	}

	//Respawns the gamepiece at it's home tile
	public void Respawn()
	{
		isAtStartTile = true;
		isInEndZone = false;
		currentTile = null;
		transform.position = GameObject.Find(gamePieceColor + "Start").transform.Find(gameObject.name).transform.position;

		animationActions.animator.SetTrigger("Respawn");
	}

	//Sets the isSelectable value and enables/disables highlighting
	public void SetSelectable(bool b)
	{
		isSelectable = b;
		selectionMarker.GetComponent<Renderer>().enabled = b;
	}

	//Returns whether or not an enemy gamepiece occupies a tile
	public bool IsEnemyGamePieceOnTile(Tile tile)
	{
		for (int i = 0; i < GameManager.gamePieces.Length; i++)
		{
			if (GameManager.gamePieces[i].currentTile == tile &&
			    GameManager.gamePieces[i] != this)
				return true;
		}

		return false;
	}
}
