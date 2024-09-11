using System;

public interface ISkipable
{
    public event Action<bool,PlayerData> OnSkip;
}
