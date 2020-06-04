using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public int LEVEL
    {
        get
        {
            return level;
        }
        set { level = value; }
    }
    public int EXPERIENCE
    {
        get
        {
            return currentExperience;
        }
        set { currentExperience = value; }
    }

    [Range(1, MAXLEVEL)]
    private int level;
    private int currentExperience;

    public GameObject LevelUpVFX;
    public int[] needExperience;

    public const int MAXLEVEL = 30;


    private void Start()
    {
        LEVEL = 1;
        needExperience = new int[MAXLEVEL];
        needExperience[0] = 500;

        for (int i = 1; i < MAXLEVEL; i++)
        {
            needExperience[i] = needExperience[i - 1] + i * 75;
        }
        instance = this;
    }
    
    public void AddExperience(int value)
    {
        EXPERIENCE += value;
        int curlevel = LEVEL;

        if(EXPERIENCE >= needExperience[curlevel - 1])
        {
            EXPERIENCE -= needExperience[curlevel - 1];
            LevelUp();
        }
        
    }
    public int GetNeedExperience()
    {
        return needExperience[LEVEL - 1];
    }

    public void LevelUp()
    {
        LEVEL += 1;

        //능력치 상승
        //UpgradePlayerStatus

        PlayerController pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        pc.MAXHEALTH += 25;
        pc.HEALTH = pc.MAXHEALTH;
        pc.STAMINA = pc.MAXSTAMINA;

        pc.STRENGTH += (int)Random.Range(1, 3);
        pc.DEFENSE += (int)Random.Range(1, 3);
        pc.DEXTERITY += (int)Random.Range(1, 3);
        pc.INTELLIGENCE += (int)Random.Range(1, 3);

        GameObject obj = GameObject.Instantiate(LevelUpVFX, this.transform);
        Destroy(obj, 5.0f);

        UIManager.Instance.OnUpdateUI();
        SoundManager.instance.PlaySFX("LevelUpSound");
    }
}
