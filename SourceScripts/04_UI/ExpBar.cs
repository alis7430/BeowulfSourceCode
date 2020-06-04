using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpBar : MonoBehaviour
{
    public Slider exp;

    public TMP_Text currentExp;
    public TMP_Text needExp;

    // Start is called before the first frame update
    void Start()
    {
        exp = this.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        exp.value = (float) LevelManager.instance.EXPERIENCE / (float) LevelManager.instance.GetNeedExperience();
        currentExp.text = LevelManager.instance.EXPERIENCE.ToString();
        needExp.text = LevelManager.instance.GetNeedExperience().ToString();
    }
}
