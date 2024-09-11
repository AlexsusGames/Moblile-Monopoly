using UnityEngine;

public class BusinessConfig : ScriptableObject
{
    public string BusinessName;
    public Sprite businessLogo;
    public int BusinessPrice;

    public int SoldPrise => BusinessPrice / 2;
    public int BuyoutPrice => (BusinessPrice / 2) + (BusinessPrice / 10);
}
