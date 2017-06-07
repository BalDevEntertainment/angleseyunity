using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour {

    public GameObject player;
    private DamageTarget damageTarget;
    private Slider lifeBarSlider;

    void Start () {
        damageTarget = player.GetComponent<DamageTarget>();
        lifeBarSlider = GetComponent<Slider>();
	}
	
	void Update () {
        lifeBarSlider.value = damageTarget.health;
	}
}
