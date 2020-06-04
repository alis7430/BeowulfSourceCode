using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkillColor : MonoBehaviour
{
    public Color changeColor;

    GameObject effectObj;
    Renderer[] ObjRenderers;

    public bool isChangeColor;

    private void Start()
    {
        effectObj = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        if (isChangeColor && effectObj != null)
        {
            ObjRenderers = effectObj.GetComponentsInChildren<Renderer>(true);

            foreach (Renderer rend in ObjRenderers)
            {
                for (int i = 0; i < rend.materials.Length; i++)
                {
                    rend.materials[i].SetColor("_TintColor", changeColor * 1.75f);
                    rend.materials[i].SetColor("_Color", changeColor * 1.75f);
                    rend.materials[i].SetColor("_RimColor", changeColor * 1.75f);
                }
            }
        }
    }
}
