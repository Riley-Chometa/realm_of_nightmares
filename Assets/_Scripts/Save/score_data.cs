using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class score_data
{
  //player coins
    public int coinPurse = 0;

    //player score

    public int playerScore = 0;

    //player keys
    public int numKeys = 0;

   public score_data(int coinPurse, int playerScore, int numKeys){
     this.coinPurse = coinPurse;
     this.playerScore = playerScore;
     this.numKeys = numKeys;
   }
}
