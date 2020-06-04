using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPosition : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        CharacterController cc = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        player = cc.gameObject;
        cc.enabled = false;
        player.transform.position = this.transform.position;
        cc.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
