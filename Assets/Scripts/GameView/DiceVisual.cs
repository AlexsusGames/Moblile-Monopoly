using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceVisual : MonoBehaviour
{
    [SerializeField] private Image diceFirst;
    [SerializeField] private Image diceSecond;
    [SerializeField] private List<Sprite> sprites = new();

    public void OpenDice(int first, int second, PlayerData player)
    {
        gameObject.SetActive(true);
        diceFirst.sprite = sprites[first - 1];
        diceSecond.sprite = sprites[second - 1];
        ChatLog.instance.AddMessage(player, $"бросил кубики. ({first} и {second})");
    }
}
