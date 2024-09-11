using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MonopolyEntryPoint : MonoBehaviour
{
    [SerializeField] private PlayersCreator playerCreator;
    [SerializeField] private MonopolyMapCreator monopolyMapCreator;
    [SerializeField] private WayPointsCreator wayPointsCreator;
    [SerializeField] private NotificationSercive notifications;
    [SerializeField] private StandartBusinesesConfig standartBusinesesConfig;
    [SerializeField] private BusinessInfoPanel businessInfoPanel;
    [SerializeField] private List<BusinessVisual> visuals;
    [SerializeField] private DiceVisual dice;
    [SerializeField] private TurnsManager turns;
    [SerializeField] private PhotonDataUpdater dataUpdater;
    [SerializeField] private TradeMenu tradeMenu;

    private PlayerMovementVisual movementView;
    private TurnMaker turnMaker;
    private MonopolyMap map;

    private List<PlayerData> playerData;

    private BusinessCenter businessCenter = new BusinessCenter();

    private void Awake()
    {
        Application.targetFrameRate = 120;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        playerData = PhotonPlayerFinder.CreateMap();
    }
    private void Start()
    {
        map = monopolyMapCreator.GetMonopolyMap();
        dataUpdater.Init(map, businessCenter);

        movementView = new(wayPointsCreator);
        turnMaker = new(map, movementView, playerData);
        turns.SetService(notifications);

        playerCreator.Init(turns, wayPointsCreator);
        turns.Setup(playerData, turnMaker);
        businessInfoPanel.SetupBusinessCenter(dataUpdater, turns);
        playerCreator.RegisterPlayers(playerData, wayPointsCreator.GetMapPoint(0));

        turnMaker.OnDice += dice.OpenDice;
        turnMaker.OnUpdateTimer += turns.ResetTime;
        turnMaker.OnFinishTurn += turns.MakeTurn;
        turns.OnCircleComplated += businessCenter.OnCircleComplated;
        turns.OnPlayerTurn += businessInfoPanel.ChangePlayer;
        turns.OnChangePlayer += businessInfoPanel.HideWindow;
        turns.OnChangePlayer += HideDice;

        SetupVisual();
    }
    private void HideDice()
    {
        dice.gameObject.SetActive(false);
        tradeMenu.transform.GetChild(0).gameObject.SetActive(false);
    }
    private void SetupVisual()
    {
        var businesses = map.GetAllBusinesses();
        for (int i = 0; i < visuals.Count; i++)
        {
            visuals[i].Setup(businesses[i], businessInfoPanel);
            visuals[i].TryGetComponent(out Button button);

            int index = i;
            button.onClick.AddListener(() =>
            {
                tradeMenu.AddBusiness(businesses[index]);
                if (tradeMenu.transform.GetChild(0).gameObject.activeInHierarchy)
                {
                    businessInfoPanel.gameObject.SetActive(false);
                }
                else businessInfoPanel.gameObject.SetActive(true);
            });
        }
    }
}
