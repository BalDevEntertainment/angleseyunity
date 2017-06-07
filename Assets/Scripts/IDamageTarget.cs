using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageTarget {

    void OnStartReceivingDamage(int damage);
    void OnStopReceivingDamage();
}
