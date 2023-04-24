using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public Player[] Players = new Player[2];
	public GameObject HeroPrefab;

	private Hero[,] gameBoard = new Hero[6,5];

	private int currentPlayer = 1;
	private bool canInteract = false;
	private bool choosingInteraction = false;
	private bool changingColor = false;

	private bool useNetwork;
	private NetworkManager networkManager;
	//private TMPro.TextMeshProUGUI turnIndicator;
	void Start()
	{
		DontDestroyOnLoad(gameObject);
		networkManager = GameObject.Find("Network Manager").GetComponent<NetworkManager>();
		MessageQueue msgQueue = networkManager.GetComponent<MessageQueue>();
		msgQueue.AddCallback(Constants.SMSG_MOVE, OnResponseMove);
		msgQueue.AddCallback(Constants.SMSG_INTERACT, OnResponseInteract);
		msgQueue.AddCallback(Constants.SMSG_ColorChange, OnResponseColorChange);
		msgQueue.AddCallback(Constants.SMSG_Win, OnWin);
		msgQueue.AddCallback(Constants.SMSG_Test, Test);
		//turnIndicator = GameObject.Find("Winner").GetComponent<TMPro.TextMeshProUGUI>();
		//Debug.Log(turnIndicator.text);
	}

	public Player GetCurrentPlayer()
	{
		return Players[currentPlayer - 1];
	}

	public void Init(Player player1, Player player2)
	{
		Players[0] = player1;
		Players[1] = player2;
		currentPlayer = 1;
		useNetwork = (!player1.IsMouseControlled || !player2.IsMouseControlled);
	}

	public void CreateHeroes()
	{
		for (int i = 0; i < 5; i++)
		{
			GameObject heroObj1 = Instantiate(HeroPrefab, new Vector3(0, 0, (float)i), Quaternion.identity);
			heroObj1.GetComponentInChildren<Renderer>().material.color = Players[0].Color;
			Hero hero1 = heroObj1.GetComponent<Hero>();
			hero1.Index = i;
			Players[0].AddHero(hero1);
			gameBoard[0, i] = hero1;
			GameObject heroObj2 = Instantiate(HeroPrefab, new Vector3(5, 0, (float)i), Quaternion.identity);
			heroObj2.GetComponentInChildren<Renderer>().material.color = Players[1].Color;
			Hero hero2 = heroObj2.GetComponent<Hero>();
			hero2.Index = i;
			gameBoard[5, i] = hero2;
			Players[1].AddHero(hero2);
		}
	}

	public bool CanInteract()
	{
		return canInteract;
	}

	public void StartInteraction()
	{
		if (canInteract)
		{
			choosingInteraction = true;
		}
	}
	public void startChangeColor(){
		changingColor = true;
	}

	public void EndInteraction(Hero hero)
	{
		EndTurn();
	}

	public void EndInteractedWith(Hero hero)
	{
		// Do nothing
	}

	public void EndMove(Hero hero)
	{
		bool heroCanInteract = false;

		int[] deltaX = { 1, 0, -1, 0 };
		int[] deltaY = { 0, 1, 0, -1 };
		for (int i = 0; i < 4; ++i)
		{
			int x = hero.x + deltaX[i];
			int y = hero.y + deltaY[i];
			if (x >= 0 && x < 6 && y >= 0 && y < 5)
			{
				if (gameBoard[x, y] && gameBoard[x, y].Owner != hero.Owner)
				{
					heroCanInteract = true;
					break;
				}
			}
		}
		if (hero.Owner.IsMouseControlled)
		{
			canInteract = heroCanInteract;
		}

		if (!heroCanInteract)
		{
			EndTurn();
		}
	}

	public void EndTurn()
	{
		ObjectSelector.SetSelectedObject(null);
		canInteract = false;
		currentPlayer = 3 - currentPlayer;
	}

	public void ProcessClick(GameObject hitObject)
	{
		if (hitObject.tag == "Tile")
		{
			if (ObjectSelector.SelectedObject)
			{
				Hero hero = ObjectSelector.SelectedObject.GetComponentInParent<Hero>();
				if (hero)
				{
					int x = (int)hitObject.transform.position.x;
					int y = (int)hitObject.transform.position.z;
					if (gameBoard[x, y] == null)
					{
						if (hero.CanMoveTo(x, y))
						{
							if (useNetwork)
							{
								networkManager.SendMoveRequest(hero.Index, x, y);
							}
							gameBoard[hero.x, hero.y] = null;
							hero.Move(x, y);
							gameBoard[x, y] = hero;
						}
					}
				}
			}
		}
		else
		{
			Hero hero = hitObject.GetComponentInParent<Hero>();
			if (hero)
			{
				if (choosingInteraction)
				{
					Hero selectedHero = ObjectSelector.SelectedObject?.GetComponentInParent<Hero>();
					if (selectedHero)
					{
						if (AreNeighbors(hero, selectedHero) && hero.Owner != selectedHero.Owner)
						{
							if (useNetwork)
							{
								networkManager.SendInteractRequest(selectedHero.Index, hero.Index);
							}
							selectedHero.Interact(hero);
							choosingInteraction = false;
						}
					}
				}
				else if (changingColor)
				{
					Hero selectedHero = ObjectSelector.SelectedObject?.GetComponentInParent<Hero>();
					if (selectedHero)
					{
						if (AreNeighbors(hero, selectedHero) && hero.Owner != selectedHero.Owner)
						{
							if (useNetwork)
							{
								networkManager.SendColorChangeRequest(hero.Index);
							}
							
							if(currentPlayer ==  1){
								hero.GetComponent<ColorChange>().updateColor(1);
								hero.GetComponent<Highlight>().ColorChange(hero.GetComponent<ColorChange>().endColor1);
								if(didWin(Players[1]))
								{
									TMPro.TextMeshProUGUI winner = GameObject.Find("Winner").GetComponent<TMPro.TextMeshProUGUI>();
									if (useNetwork)
									{
										//Debug.Log("send win request");
									networkManager.SendTestRequest(1);
									}else{
									winner.text = GetCurrentPlayer().Name + " Wins";
								}
								}
							}
							if(currentPlayer ==  2){
								hero.GetComponent<ColorChange>().updateColor(2);
								hero.GetComponent<Highlight>().ColorChange(hero.GetComponent<ColorChange>().endColor2);
									if(didWin(Players[0]))
									{
									TMPro.TextMeshProUGUI winner = GameObject.Find("Winner").GetComponent<TMPro.TextMeshProUGUI>();
									if (useNetwork)
									{
									networkManager.SendTestRequest(1);
									}else{
									winner.text = GetCurrentPlayer().Name + " Wins";
								}
								}
							}
							
							changingColor = false;
						}
					}
				}
				else if (hero.gameObject == ObjectSelector.SelectedObject)
				{
					ObjectSelector.SetSelectedObject(null);
				}
				else if (hero.Owner.IsMouseControlled && hero.Owner == Players[currentPlayer - 1])
				{
					ObjectSelector.SetSelectedObject(hitObject);
				}
			}
		}
	}

	public bool HighlightEnabled(GameObject gameObject)
	{
		if (gameObject.tag == "Tile")
		{
			Hero hero = ObjectSelector.SelectedObject?.GetComponentInParent<Hero>();
			if (hero)
			{
				int x = (int)gameObject.transform.position.x;
				int y = (int)gameObject.transform.position.z;
				return (gameBoard[x, y] == null);
			}
		}
		else if (choosingInteraction)
		{
			Hero hero = gameObject.GetComponentInParent<Hero>();
			Hero selectedHero = ObjectSelector.SelectedObject?.GetComponentInParent<Hero>();
			if (hero && selectedHero)
			{
				return AreNeighbors(hero, selectedHero) && hero.Owner != selectedHero.Owner;
			}
			else
			{
				return false;
			}
		}
		else
		{
			Hero hero = gameObject.GetComponentInParent<Hero>();
			if (hero)
			{
				return (hero.Owner.IsMouseControlled && hero.Owner == Players[currentPlayer - 1]);
			}
		}
		return true;
	}

	private bool AreNeighbors(Hero hero1, Hero hero2)
	{
		return (Math.Abs(hero1.x - hero2.x) + Math.Abs(hero1.y - hero2.y) == 1);
	}

	public void OnResponseMove(ExtendedEventArgs eventArgs)
	{
		ResponseMoveEventArgs args = eventArgs as ResponseMoveEventArgs;
		if (args.user_id == Constants.OP_ID)
		{
			int pieceIndex = args.piece_idx;
			int x = args.x;
			int y = args.y;

			Hero hero = Players[args.user_id - 1].Heroes[pieceIndex];
			gameBoard[hero.x, hero.y] = null;
			hero.Move(x, y);
			gameBoard[x, y] = hero;
		}
		else if (args.user_id == Constants.USER_ID)
		{
			// Ignore
		}
		else
		{
			Debug.Log("ERROR: Invalid user_id in ResponseReady: " + args.user_id);
		}
	}

	public void OnResponseInteract(ExtendedEventArgs eventArgs)
	{
		ResponseInteractEventArgs args = eventArgs as ResponseInteractEventArgs;
		if (args.user_id == Constants.OP_ID)
		{
			int pieceIndex = args.piece_idx;
			int targetIndex = args.target_idx;
			Hero hero = Players[args.user_id - 1].Heroes[pieceIndex];
			Hero target = Players[Constants.USER_ID - 1].Heroes[targetIndex];
			hero.Interact(target);
		}
		else if (args.user_id == Constants.USER_ID)
		{
			// Ignore
		}
		else
		{
			Debug.Log("ERROR: Invalid user_id in ResponseReady: " + args.user_id);
		}
	}
	
	public void OnResponseColorChange(ExtendedEventArgs eventArgs)
	{
		Debug.Log("gotcolorChange request");
		ResponseColorChangeEventArgs args = eventArgs as ResponseColorChangeEventArgs;
		if (args.user_id == Constants.OP_ID)
		{
			Debug.Log(args.user_id);
			int pieceIndex = args.piece_idx;
			Hero hero = Players[3 - args.user_id - 1].Heroes[pieceIndex];
				if(Constants.OP_ID ==  1){
					hero.GetComponent<ColorChange>().updateColor(1);
					hero.GetComponent<Highlight>().ColorChange(hero.GetComponent<ColorChange>().endColor1);
					}
					if(Constants.OP_ID ==  2){
						hero.GetComponent<ColorChange>().updateColor(2);
						hero.GetComponent<Highlight>().ColorChange(hero.GetComponent<ColorChange>().endColor2);
					}
		}
		else if (args.user_id == Constants.USER_ID)
		{
			// Ignore
		}
		else
		{
			Debug.Log("ERROR: Invalid user_id in ResponseReady: " + args.user_id);
		}
	}
public void Test(ExtendedEventArgs eventArgs)
	{
		ResponseTestEventArgs args = eventArgs as ResponseTestEventArgs;
		TMPro.TextMeshProUGUI winner = GameObject.Find("Winner").GetComponent<TMPro.TextMeshProUGUI>();
		if (args.user_id == Constants.OP_ID)
		{
			Debug.Log("Test1 ");
			winner.text = "You Lose";
		}
		else if (args.user_id == Constants.USER_ID)
		{
			Debug.Log("Test2 ");
			winner.text = "You Win";
		}
		else
		{
			Debug.Log("ERROR: Invalid user_id in ResponseReady: " + args.user_id);
		}
	}

	public void OnWin(ExtendedEventArgs eventArgs)
	{
		Debug.Log("gotWin request");
		ResponseWinEventArgs args = eventArgs as ResponseWinEventArgs ;
		if (args.user_id == Constants.OP_ID)
		{
			Debug.Log("You Win");
		}
		else if (args.user_id == Constants.USER_ID)
		{
			Debug.Log("You lose");
		}
		else
		{
			Debug.Log("ERROR: Invalid user_id in ResponseWin: " + args.user_id);
		}
	}

	private bool didWin(Player player){
		Hero[] heros = player.Heroes;
		for(int i = 0; i<5; i++){
			if (heros[i].isChanged == false)
			{
				Debug.Log(i);
				return false;
			}
		}
		return true;
	}

}
