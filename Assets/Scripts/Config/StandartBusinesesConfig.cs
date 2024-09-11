using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/StandartBusineses", fileName = "StandartBusineses")]
public class StandartBusinesesConfig : ScriptableObject
{
    public List<BusinessConfig> config;
}
