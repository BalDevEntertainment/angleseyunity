using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStatus
{
    Idle, Walking, Fighting
}

public interface IPlayerStatusListener
{
    void OnStatusChanged(PlayerStatus playerStatus);
}
