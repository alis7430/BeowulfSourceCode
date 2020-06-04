using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public GameObject[] Skills;
    
    public void ActiveSkill(int skillNum)
    {
        GameObject SkillObj = GameObject.Instantiate(Skills[skillNum - 1], 
           this.transform.GetChild(0));

        switch (skillNum)
        {
            case 1:
                SkillObj.transform.position += new Vector3(0.0f, 1.0f, 0.0f);
                SoundManager.instance.PlaySFX("Swing_s01");
                Destroy(SkillObj, 0.2f);
                Invoke("createSkill1", 0.4f);
                break;
            case 2:
                SkillObj.transform.parent = null;
                Destroy(SkillObj, 2.0f);
                break;
            case 3:
                SkillObj.transform.position += new Vector3(0.0f, 1.0f, 0.0f);
                Destroy(SkillObj, 0.5f);
                break;
            case 4:
                SkillObj.transform.position += new Vector3(0.0f, 1.0f, 0.0f);
                Destroy(SkillObj, 4.2f);
                break;
            default:
                break;
        }
    }
    private void createSkill1()
    {
        GameObject SkillObj = GameObject.Instantiate(Skills[0], this.transform.GetChild(0));
        SkillObj.transform.position += new Vector3(0.0f, 1.0f, 0.0f);

        SoundManager.instance.PlaySFX("Swing_s01");

        Destroy(SkillObj, 0.2f);
    }
}
