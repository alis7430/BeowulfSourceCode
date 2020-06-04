using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : MonoBehaviour
{
    public BaseCharacter boss;

    public Slider Healthbar;

    public TMP_Text currentHealth;
    public TMP_Text needHealth;

    // Start is called before the first frame update
    void Start()
    {
        Healthbar = this.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        Healthbar.value = (float)boss.HEALTH / (float)boss.MAXHEALTH;
        currentHealth.text = boss.HEALTH.ToString();
        needHealth.text = boss.MAXHEALTH.ToString();
    }
}
