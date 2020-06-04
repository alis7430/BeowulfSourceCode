using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSkill : MonoBehaviour
{
    [Range(0.001f, 1.0f)]
    public float HealRatio;

    private int value;
    [SerializeField]
    private float elapsedTime;

    private PlayerController pc;

    // Start is called before the first frame update
    void Awake()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        value = (int)(pc.MAXHEALTH * HealRatio / 5.0f);
        elapsedTime = 0.0f;

        Heal();

    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= 1.0f)
        {
            elapsedTime = 0.0f;
            Heal();
        }
    }

    private void Heal()
    {
        if (pc.HEALTH > 0)
            pc.HEALTH += value;

        if (pc.HEALTH >= pc.MAXHEALTH)
            pc.HEALTH = pc.MAXHEALTH;
        EventManager.Instance.PostNotification(EVENT_TYPE.PLAYER_BASEINFO_UPDATE, this);
    }
}
