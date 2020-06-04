using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOnOff : MonoBehaviour
{

    public void OnActive()
    {
        if(this.gameObject.activeSelf == false)
            this.gameObject.SetActive(true);
        else
            this.gameObject.SetActive(false);
    }
}
