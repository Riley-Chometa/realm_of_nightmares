using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    
    private bool AustralianMode = false;

    public void SetAustralianMode(bool mode)
    {
        this.AustralianMode = mode;
    }
    public bool IsAustralianMode()
    {
        return this.AustralianMode;
    }
}
