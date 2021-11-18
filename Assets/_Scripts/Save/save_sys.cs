using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class save_sys : MonoBehaviour
{
    
    public static void SavePlayer(PlayerMovement player){
      BinaryFormatter formatter = new BinaryFormatter();
      string path = Application.persistentDataPath + "/player.player";
      FileStream stream = new FileStream(path,FileMode.Create);

      player_data data = new player_data(player);

      formatter.Serialize(stream,data);
      stream.Close();
    }

    public static player_data LoadPlayer(){
      string path = Application.persistentDataPath + "/player.player";
      if(File.Exists(path)){
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path,FileMode.Open);
        player_data data = formatter.Deserialize(stream) as player_data;
        stream.Close();
        return data;
      }else{
        Debug.LogError("Error"+path);
        return null;
      }
    }
    public static void SaveScore(score_data data){
      BinaryFormatter formatter = new BinaryFormatter();
      string path = Application.persistentDataPath + "/score.score";
      FileStream stream = new FileStream(path,FileMode.Create);
      formatter.Serialize(stream,data);
      stream.Close();
    }

    public static score_data LoadScore(){
      string path = Application.persistentDataPath + "/score.score";
      if(File.Exists(path)){
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path,FileMode.Open);
        score_data score = formatter.Deserialize(stream) as score_data;
        stream.Close();
        return score;
      }else{
        Debug.LogError("Error"+path);
        return null;
      }
    }
}
