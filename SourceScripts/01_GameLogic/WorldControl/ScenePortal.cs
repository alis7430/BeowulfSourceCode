using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePortal : Portal
{
    public string nextScene;

    // Start is called before the first frame update
    void Start()
    {
        value = PortalValue.Scene;
    }

    protected override void ActivatePortal(Transform playerTransform)
    {
        EventManager.Instance.PostNotification(EVENT_TYPE.ACTIVE_PORTAL, this, ID);
        LoadingSceneManager.LoadScene(nextScene);
    }
}
