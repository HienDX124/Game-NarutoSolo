using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitpointBar : MonoBehaviour {

    public Player player;
    public Image fillImage;
    private Slider slider;
    protected float currentHP;
    protected float maxHP;

    void Start()
    {
        slider = GetComponent<Slider>();

    }

    void Update()
    {
        maxHP = player.MaxHitPoint;
        currentHP = player.currentHP;

        if (slider.value <= slider.minValue) {
            fillImage.enabled = false;
        }

        if (slider.value > slider.minValue && fillImage.enabled) {
            fillImage.enabled = true;
        }

        float fillValue = currentHP / maxHP;

        if (fillValue <= slider.maxValue / 3) {
            fillImage.color = Color.red;
        }
        else if (fillValue > slider.maxValue /3 ) {
            fillImage.color = new Color(125,0,0);
        }
        slider.value = fillValue;
    }
}
