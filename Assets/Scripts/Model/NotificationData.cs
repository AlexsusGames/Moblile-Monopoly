public struct NotificationData
{
    public string Title;
    public string Message;
    public string FirstAction;
    public string SecondAction;

    public NotificationData(bool dice)
    {
        Title = "��� ���!";
        Message = "<color=green>���������:</color> ����� ���������� �����, ������� �� ������ ������.";
        FirstAction = "�������";
        SecondAction = "";
    }
    
    public NotificationData(BusinessConfig config)
    {
        Title = "������?";
        Message = $"������ ������ ����������� '{config.BusinessName}' �� {config.BusinessPrice}k?";
        FirstAction = "������";
        SecondAction = "����������";
    }
    public NotificationData(PlayerData player, int sum)
    {
        Title = "��������� �����";
        Message = $"�� ������ ��������� ������ '{player.PlayerName}' <color=green>{sum}</color>$";
        FirstAction = "���������";
        SecondAction = "�������";
    }
    public NotificationData(int sum)
    {
        Title = "��������� �����";
        Message = $"�� ������ ��������� ����� ����� � ������� <color=green>{sum}</color>$";
        FirstAction = "���������";
        SecondAction = "�������";
    }
}
