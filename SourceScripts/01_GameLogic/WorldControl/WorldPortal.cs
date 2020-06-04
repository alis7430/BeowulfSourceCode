using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPortal : Portal
{
    public Transform PortalDestination;

    // Start is called before the first frame update
    void Start()
    {
        value = PortalValue.World;
    }

    protected override void ActivatePortal(Transform playerTransform)
    {
        playerTransform.position = PortalDestination.position;
    }
}
