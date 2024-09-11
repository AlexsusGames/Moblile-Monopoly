using System;
using System.Threading.Tasks;
using Photon.Pun;
using Unity.VisualScripting;

public class PlayerMovementVisual 
{
    private WayPointsCreator wayPoints;

    public PlayerMovementVisual(WayPointsCreator wayPoints)
    { 
        this.wayPoints = wayPoints;
    }
    public async void Move(int currentIndex, int transformIndex, Action callBack)
    {
        var actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        await Move(currentIndex, transformIndex, actorNumber);

        callBack();
    }

    private async Task Move(int currentIndex, int transformIndex, int playerId)
    {
        int moveIndex = currentIndex;
        int aimTransform = currentIndex + transformIndex;

        var movablePlayer = PhotonPlayerFinder.GetPlayerData(playerId).PlayerMovement;

        while (moveIndex != aimTransform)
        {

            if (moveIndex == wayPoints.CountOfWays - 1)
            {
                moveIndex -= wayPoints.CountOfWays;
                aimTransform -= wayPoints.CountOfWays;
            }

            if (moveIndex < aimTransform) moveIndex++;
            else moveIndex--;
            movablePlayer.MoveToParent(moveIndex, false);

            if (moveIndex == aimTransform)
            {
                SetPlayerPosition(aimTransform);
            }

            await Task.Delay(165);
        }
    }
    private void SetPlayerPosition(int index)
    {
        var wayPoint = wayPoints.GetMapPoint(index);
        if(wayPoint.childCount > 1)
        {
            for (int i = 0; i < wayPoint.childCount; i++)
            {
                var child = wayPoint.GetChild(i);
                child.TryGetComponent(out PlayerMovement movement);
                movement.MoveToParent(index, true);
            }
        }
    }
}
