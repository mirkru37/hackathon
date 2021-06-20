using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public float max = 10;
    public float cur;
    public Gradient gradient;
    public Image fill;
    private void Start()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    public void setMax(float m)
    {
        max = m;
        slider.maxValue = m;
        fill.color = gradient.Evaluate(1f);
    }

    public void remove()
    {
        Destroy(gameObject);
    }
    
    public void setCurrent(float c)
    {
        cur = c;
        slider.value = c;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        if (c <= 0)
        {
            remove();
        }
    }
}
