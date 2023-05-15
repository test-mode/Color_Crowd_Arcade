using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventManager
{
    public static event Action LevelStarted;
    public static event Action OnSell;
    public static event Action OnUpgrade;
    public static event Action OnUpgradeExit;
    public static event Action FriendCount;

    public static void LevelStartedEvent()
    {
        LevelStarted?.Invoke();
    }

    public static void OnSellEvent ()
    {
        OnSell?.Invoke();
    }

    public static void OnUpgradeEvent()
    {
        OnUpgrade?.Invoke();
    }
    
    public static void OnUpgradeExitEvent()
    {
        OnUpgradeExit?.Invoke();
    }
    public static void OnFriendCountEvent()
    {
        OnUpgradeExit?.Invoke();
    }
}
