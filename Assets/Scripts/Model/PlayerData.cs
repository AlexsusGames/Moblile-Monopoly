using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData 
{
    public Color PlayerColor;
    public string PlayerName;
    public Sprite PlayerAvatar;
    public Wallet PlayerWallet = new();
    public List<Business> Businesses = new();
    public PlayerMovement PlayerMovement;
    public PlayerVisual PlayerVisual;

    public int restTurns;

    public PlayerData(string name)
    {
        PlayerName = name;
        restTurns = 0;
    }
    public PlayerData()
    {

    }

}
