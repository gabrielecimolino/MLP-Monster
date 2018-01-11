using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour, IPrintable {

	protected List<GameNetwork> gameNetworks;
	protected string monsterName = "Anon";

	public virtual bool hasGameNetwork(string gameName){
		return Functions.foldl(((x, y) => x || y.getGameName() == gameName) , false, gameNetworks.ToArray());
	}
	public virtual NeuralNetwork getGameNetwork(string gameName){
		foreach(GameNetwork network in gameNetworks){
			if(network.getGameName() == gameName) return network.network;
		}
		throw new System.Exception("Monster::getGameNetwork(virtual) ~ Attempted to retrieve network of unknown game\nGame name: " + gameName);
	}

	public virtual void addGameNetwork(string gameName, NeuralNetwork network){
		gameNetworks.Add(new GameNetwork(gameName, network));
	}

	public virtual void addGameNetwork(string gameName, int inputs, int outputs, int retention = 0){
		gameNetworks.Add(new GameNetwork(gameName, inputs, outputs, retention));
	}

	public virtual void addGameNetwork(string gameName, int inputs, int[] hiddenLayerSizes, int outputs, int retention = 0){
		gameNetworks.Add(new GameNetwork(gameName, inputs, hiddenLayerSizes, outputs, retention));
	}

	public virtual string print(){
		return monsterName + " Games played: " + Functions.print(Functions.map((x => x.getGameName()), gameNetworks.ToArray()));
	}
}

public class Monster1 : Monster {

	public Monster1(string monsterName){
		this.monsterName = monsterName;
		this.gameNetworks = new List<GameNetwork>();
	}
}

public class GameNetwork : IPrintable {
	public NeuralNetwork network;
	private string gameName = "The game with no name";

	public GameNetwork(string gameName, NeuralNetwork network){
		this.gameName = gameName;
		this.network = network;
	}
	public GameNetwork(string gameName, int inputs, int outputs, int retention = 0){
		this.gameName = gameName;
		this.network = new NeuralNetwork(inputs, outputs, retention);
		this.network.randomizeWeights(0.1f);
		this.network.randomizeBiases(0.1f);
	}

	public GameNetwork(string gameName, int inputs, int[] hiddenLayerSizes, int outputs, int retention = 0){
		this.gameName = gameName;
		this.network = new NeuralNetwork(inputs, hiddenLayerSizes, outputs, retention);
		this.network.randomizeWeights(0.1f);
		this.network.randomizeBiases(0.1f);
	}

	public string print(){
		return gameName + ": " + network.print();
	}

	public string getGameName(){
		return gameName;
	}
}
