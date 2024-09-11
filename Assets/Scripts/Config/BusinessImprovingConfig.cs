using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/BusinessImproving", fileName = "BusinessImproving")]
public class BusinessImprovingConfig : BusinessConfig
{
    public int BusinessId;
    public int ImprovingCost;
    public int[] Outcomes;

    public int FillialOutcome => Outcomes[0] * 2;
}
