using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill : MonoBehaviour
{
    public GameObject[] Skills;
    
    public void ActiveSkill(int skillNum)
    {
        GameObject SkillObj = GameObject.Instantiate(Skills[skillNum - 1],
           this.transform.GetChild(0));

        switch (skillNum)
        {
            case 1:
                SkillObj.transform.parent = null;
                Destroy(SkillObj, 2.0f);
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                break;
        }
    }
}
