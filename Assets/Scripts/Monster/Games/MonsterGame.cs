using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterGame : IPrintable {

	protected string gameName = "The game with no name";
	protected bool gameOver = false;
	protected NeuralNetwork player1;
	protected NeuralNetwork player2;

	public virtual string getGameName(){
		return gameName;
	}

	public virtual bool getGameOver(){
		return gameOver;
	}
	public abstract int getWinner();
	public virtual string print(){
		return gameName;
	}
}