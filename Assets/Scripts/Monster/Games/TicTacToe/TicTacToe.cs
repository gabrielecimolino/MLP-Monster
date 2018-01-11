using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToe : MonsterGame {

	private char[] board = "---------".ToCharArray();
	private int winner = 0;

	public TicTacToe(Monster player1, Monster player2){
		this.gameName = "Tic Tac Toe";
		this.gameOver = false;

		if(!player1.hasGameNetwork(gameName)) player1.addGameNetwork(gameName, 9, new int[]{9}, 9);
		this.player1 = player1.getGameNetwork(gameName);
		
		if(!player2.hasGameNetwork(gameName)) player2.addGameNetwork(gameName, 9, new int[]{9}, 9);
		this.player2 = player2.getGameNetwork(gameName);
	}

	public override int getWinner(){
		return winner;
	}

	public char[] getBoard(){
		return board;
	}

	public static char opponent(char player){
		if(player == 'X'){
			return 'O';
		}
		return 'X';
	}

	public void newGame(){
		gameOver = false;
		board = "---------".ToCharArray();
		winner = 0;
		player2.randomizeBiases(0.1f);
		player2.randomizeWeights(0.1f);
	}

	public bool takeTurn(char player){
		if(gameOver) return false;
		else{
			makeBestMove(player);
			updateWinner();
			return true;
		}
	}

	public void train(float[] desiredOutputs){
		player1.train(desiredOutputs);
	}

	private void makeBestMove(char player){
		float[] outputs;
		if(player == 'X'){
			player1.feedForward(Functions.map((x => (x == player) ? 1.0f : (x == '-') ? 0.0f : -1.0f), board));
			outputs = player1.getOutputs();
		}
		else if(player == 'O'){
			player2.feedForward(Functions.map((x => (x == player) ? 1.0f : (x == '-') ? 0.0f : -1.0f), board));
			outputs = player2.getOutputs();
		}
		else throw new System.ArgumentException("TicTacToe::makeBestMove ~ invalid player char: " + player);

		int favoredNeuron = Functions.maxIndex(outputs);

		while(!makeMove(player, favoredNeuron)){
			outputs[favoredNeuron] = -1.0f;
			favoredNeuron = Functions.maxIndex(outputs);
			if(Functions.all((x => x == -1.0f), outputs)) throw new System.Exception("TicTacToe::makeBestMove ~ no valid moves");
		}
	}

	private bool makeMove(char playerToken, int move){
		if(board[move] == '-'){
			board[move] = playerToken;
			return true;
		}
		return false;
	}

	private void updateWinner(){
		if(!gameOver){
			checkBoard();

			if(gameOver){
				winner = 3;
			}

			if(won('X')){
				winner = 1;
				gameOver = true;
			} 
			else if(won('O')){
				winner = 2;
				gameOver = true;
			}
		}
	}

	private bool won(char move){
		return (
			// Check rows
			(board[0] == move && board[1] == move && board[2] == move)
			|| (board[3] == move && board[4] == move && board[5] == move)
			|| (board[6] == move && board[7] == move && board[8] == move)
			// Check columns
			|| (board[0] == move && board[3] == move && board[6] == move)
			|| (board[1] == move && board[4] == move && board[7] == move)
			|| (board[2] == move && board[5] == move && board[8] == move)
			// Check diagonals
			|| (board[0] == move && board[4] == move && board[8] == move)
			|| (board[2] == move && board[4] == move && board[6] == move)
		);
	}

	private void checkBoard(){
		gameOver = gameOver || Functions.foldl<char, bool>(((x, y) => x && (y == 'X' || y == 'O')), true, board);
	}

	private float moveHash(char move){
		if(move == 'X') return 1.0f;
		if(move == 'O') return -1.0f;
		else return 0.0f;
	}
}
