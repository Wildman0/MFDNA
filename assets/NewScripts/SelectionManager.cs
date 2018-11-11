using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionManager : MonoBehaviour {

    public static GamePiece selectedGamePiece;

	//Runs every frame
	public void Update()
	{
		MouseController();
	}

	//Controls the mouse and it's selection
	private void MouseController()
	{
		//lmb gamepiece selection
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hitInfo = new RaycastHit ();

			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitInfo))
			{
				if (hitInfo.transform.gameObject.GetComponent<GamePiece> () != null)
				{
					GamePiece gamePiece = hitInfo.transform.gameObject.GetComponent<GamePiece> ();

					if (gamePiece.gamePieceColor == GameManager.currentPlayer.color && gamePiece.isSelectable)
					{
						SelectGamepiece(hitInfo.transform.gameObject.GetComponent<GamePiece>());
					}
					else
					{
						print("Gamepiece is not selectable");
					}
				}
			}
		}
	}

	//Selects the given gamepiece
	private void SelectGamepiece(GamePiece gamePiece)
	{
		selectedGamePiece = gamePiece;
		gamePiece.Move(gamePiece.CalculateTileWouldLandOnWithRoll(Dice.number));

		Debug.Log("Selected gamepiece " + gamePiece.gameObject.name);
	}
}
