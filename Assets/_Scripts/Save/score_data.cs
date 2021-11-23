using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class score_data
{
  //player coins
    public int coinPurse;

    //player score

    public int playerScore;

    //player keys
    public int numKeys;
    public int numBombs;

   public score_data(int coinPurse, int playerScore, int numKeys, int numBombs){
     this.coinPurse = coinPurse;
     this.playerScore = playerScore;
     this.numKeys = numKeys;
     this.numBombs = numBombs;
   }
}
