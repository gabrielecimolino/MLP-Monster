using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITicTacToe : MonoBehaviour {

	[SerializeField] private GameObject terminal;
	[SerializeField] private GameObject terminalText;
	[SerializeField] private GameObject board;
	[SerializeField] private GameObject neuralNetworkView;
	[SerializeField] private GameObject[] buttons;
	[SerializeField] private bool[] buttonsClicked;
	[SerializeField] private GameObject[] pieces;
	[SerializeField] private Camera camera;
	[SerializeField] private Sprite X;
	[SerializeField] private Sprite O;
	private TicTacToe game;
	private char[] gameBoard;
	private Monster player1;
	private Monster player2;
	private char currentPlayer;

	public void Initialize(Monster player1, Monster player2){
		this.game = new TicTacToe(player1, player2);
		this.gameBoard = Functions.copy(game.getBoard());
		this.player1 = player1;
		this.player2 = player2;
		this.currentPlayer = 'X';
		this.neuralNetworkView = Instantiate(neuralNetworkView, Vector3.zero, Quaternion.identity);
		this.neuralNetworkView.GetComponent<NeuralNetworkView>().setNeuralNetwork(player1.getGameNetwork(game.getGameName()), "top-left");
	}

	public void click(int button){
		Debug.Log("Click: " + button.ToString());
		buttonsClicked[button] = true;
	}

	public bool buttonClicked(int button){
		bool clicked = buttonsClicked[button];
		buttonsClicked[button] = false;
		return clicked;
	}

	public void newGame(){
		if(currentPlayer != '-'){
			currentPlayer = 'X';
			resetBoard();
			updateTerminal("");
			game.newGame();
			gameBoard = Functions.copy(game.getBoard());
		}
	}

	void Start () {
		this.game = null;
		this.buttonsClicked = Functions.initArray(9, false);
		this.camera = GameObject.Find("Main Camera").GetComponent<Camera>();
		this.gameObject.GetComponent<Canvas>().worldCamera = camera;
		Initialize(new Monster1("player1"), new Monster1("player2"));

		Vector2 cameraDimensions = new Vector2(camera.orthographicSize * 2 * camera.aspect, camera.orthographicSize * 2);
		Debug.Log("Camera dimensions: " + cameraDimensions);
	}
	
	void Update () {
		if(game != null){
			if(currentPlayer == '-'){
				for(int i = 0; i < buttonsClicked.Length; i++){
					if(buttonsClicked[i]){
						game.train(Functions.map((x => x ? 1.0f : -1.0f), buttonsClicked));
						updateTerminal("");
						currentPlayer = 'O';
						i = buttonsClicked.Length;
					}
				}
			}

			else if(!game.getGameOver()){
				if(currentPlayer == 'X'){
					buttonsClicked = Functions.map((x => false), buttonsClicked);
					game.takeTurn(currentPlayer);
					updateBoard();
					currentPlayer = '-';
					updateTerminal("Choose correct move");
				}
				else if(currentPlayer == 'O'){
					game.takeTurn(currentPlayer);
					updateBoard();
					currentPlayer = TicTacToe.opponent(currentPlayer);
				}
			}
			else{
				int winner = game.getWinner();
				switch(winner){
					case 1: 
						updateTerminal("X won!");
						break;
					case 2: 
						updateTerminal("O won ;____;");
						break;
					case 3:
						updateTerminal("Nobody won...");
						break;
					default:
						updateTerminal("WTF");
						break;
				}
			}
		}
	}

	private void resetBoard(){
		for(int i = 0; i < pieces.Length; i++){
			pieces[i].GetComponent<Image>().sprite = null;
			pieces[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		}
	}

	private void updateBoard(){
		char[] newBoard = game.getBoard();

		for(int i = 0; i < gameBoard.Length; i++){
			if(gameBoard[i] != newBoard[i]){
				gameBoard[i] = newBoard[i];

				if(gameBoard[i] == 'X'){
					pieces[i].GetComponent<Image>().sprite = X;
				}
				else{
					pieces[i].GetComponent<Image>().sprite = O;
				}
				pieces[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			}
		}
	}

	private void updateTerminal(string text){
		terminalText.GetComponent<Text>().text = text;
	}
}
