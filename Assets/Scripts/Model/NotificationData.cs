public struct NotificationData
{
    public string Title;
    public string Message;
    public string FirstAction;
    public string SecondAction;

    public NotificationData(bool dice)
    {
        Title = "Ваш ход!";
        Message = "<color=green>Подсказка:</color> чтобы предложить обмен, нажмите на иконку игрока.";
        FirstAction = "Бросить";
        SecondAction = "";
    }
    
    public NotificationData(BusinessConfig config)
    {
        Title = "Купить?";
        Message = $"Хотите купить предприятие '{config.BusinessName}' за {config.BusinessPrice}k?";
        FirstAction = "Купить";
        SecondAction = "Отказаться";
    }
    public NotificationData(PlayerData player, int sum)
    {
        Title = "Заплатить ренту";
        Message = $"Вы должны заплатить игроку '{player.PlayerName}' <color=green>{sum}</color>$";
        FirstAction = "Заплатить";
        SecondAction = "Сдаться";
    }
    public NotificationData(int sum)
    {
        Title = "Заплатить штраф";
        Message = $"Вы должны заплатить банку штраф в размере <color=green>{sum}</color>$";
        FirstAction = "Заплатить";
        SecondAction = "Сдаться";
    }
}
