using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour
{
	private GameManager gameManager;
	private Button interactButton;
	private Button changeColor;
	private TMPro.TextMeshProUGUI turnIndicator;
	//private TMPro.TextMeshProUGUI Winner;

	// Start is called before the first frame update
	void Start()
    {
		gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
		interactButton = GameObject.Find("Interact Button").GetComponent<Button>();
		changeColor = GameObject.Find("changeColor").GetComponent<Button>();
		turnIndicator = GameObject.Find("Turn Indicator").GetComponent<TMPro.TextMeshProUGUI>();
		//Winner = GameObject.Find("Winner").GetComponent<TMPro.TextMeshProUGUI>();
		
	}

	public void OnInteractClick()
	{
		gameManager.StartInteraction();
	}

	void Update()
	{
		interactButton.interactable = gameManager.CanInteract();
		turnIndicator.text = gameManager.GetCurrentPlayer().Name + "'s turn";
		changeColor.interactable = gameManager.CanInteract();
	}
	public void OnColorChangeClick(){
		gameManager.startChangeColor();
	}
	public void winner(string winner){
		//Winner.text = winner + "Wins";
	}
}
