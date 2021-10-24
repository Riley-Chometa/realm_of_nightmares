using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
  public int score;
  public int bodyParts;
  public float[] position;
  public int highestscore;
  public int health;

  public PlayerData(Snake player){
    score = player.score;
    health = player.health;
    highestscore = player.highestscore;
    bodyParts  = player.segments.Count;
    position = new float[3];
    position[0] = player.transform.position.x;
    position[1] = player.transform.position.y;
    position[2] = player.transform.position.z;
  }
}
