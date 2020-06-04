using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDamage : MonoBehaviour
{
    private PlayerController pc;

    public float increaseRatio = 1.0f;
    public float Damage = 0;

    // Start is called before the first frame update
    void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Damage = pc.DAMAGE * increaseRatio;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {

            int damage = (int)Random.Range(Damage - Damage / 4, Damage + Damage / 4);
            other.gameObject.SendMessage("OnAttacked", damage);
        }
    }
}
