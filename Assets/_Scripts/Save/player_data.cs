using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class player_data
{
  public int currentStamina;
  public int attackDamage;
  public float[] position;

   public player_data(PlayerMovement player){
      currentStamina = 4;
      attackDamage =2 ;
      position = new float[3];
      position[0] = player.transform.position.x;
      position[1] = player.transform.position.y;
      position[2] = player.transform.position.z;
   }
}
