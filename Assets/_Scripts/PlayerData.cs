using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
  public int playerScore = 0;
  private int startScore = 0;

  public PlayerData(PlayerStats player){
    playerScore = player.playerScore;
    startScore = player.startScore;
    
  }
}
