using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class save_sys : MonoBehaviour
{
  int coinPurse = 0;
  int playerScore = 0;
  int numKeys = 0;
  int numBombs = 0;
  private PlayerStatsComponent playerstatsmodifier;
  public GameObject playerStats;
  private void Start() {
        playerstatsmodifier = playerStats.GetComponent<PlayerStatsComponent>();
    }

    public void SavePlayer(){
      BinaryFormatter formatter = new BinaryFormatter();
      string path = Application.persistentDataPath + "/player.player";
      FileStream stream = new FileStream(path,FileMode.Create);
      PlayerMovement player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
      player_data data = new player_data(player);

      formatter.Serialize(stream,data);
      stream.Close();
    }

    public void LoadPlayer(){
      string path = Application.persistentDataPath + "/player.player";
      if(File.Exists(path)){
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path,FileMode.Open);
        player_data data = formatter.Deserialize(stream) as player_data;
        stream.Close();
        GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().LoadPlayer(data);
      }else{
        Debug.LogError("Error"+path);
      }
    }
    public void SaveScore(){
      coinPurse = playerstatsmodifier.getCoins();
      playerScore = playerstatsmodifier.getScore();
      numKeys = playerstatsmodifier.getNumKeys();
      numBombs = playerstatsmodifier.getNumBomb();
      score_data data = new score_data(coinPurse,playerScore,numKeys, numBombs);
      BinaryFormatter formatter = new BinaryFormatter();
      string path = Application.persistentDataPath + "/score.score";
      FileStream stream = new FileStream(path,FileMode.Create);
      formatter.Serialize(stream,data);
      stream.Close();
    }

    public void LoadScore(){
      string path = Application.persistentDataPath + "/score.score";
      if(File.Exists(path)){
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path,FileMode.Open);
        score_data score = formatter.Deserialize(stream) as score_data;
        stream.Close();
        playerstatsmodifier.modifyCoins(score.coinPurse);
        playerstatsmodifier.modifyScore(score.playerScore);
        playerstatsmodifier.modifyKeys(score.numKeys);
        playerstatsmodifier.modifyBombs(score.numBombs);
      }else{
        Debug.LogError("Error"+path);
      }
    }
}
