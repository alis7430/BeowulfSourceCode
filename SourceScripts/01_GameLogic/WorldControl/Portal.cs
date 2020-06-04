using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public enum PortalValue
    {
        Scene,
        World
    }

    public int ID;
    public PortalValue value;

    protected virtual void ActivatePortal(Transform playerTransform) { }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                Transform player = other.transform;
                ActivatePortal(player);
            }
        }
    }
}
