using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyAttack : Enemy_Attack {

    private Cinemachine.CinemachineImpulseSource MyInpulse;

    // Use this for initialization
    void Start () {
        MyInpulse = GetComponent<Cinemachine.CinemachineImpulseSource>();
    }

    // Update is called once per frame
    //void Update()
    //{
    //}

    public override void Damage()
    {
        MyInpulse.GenerateImpulse();

        if (attackTarget != null)
        {
            attackTarget.GetComponent<PlayerControl>().TakeDamage(attackDamage);
        }

    }
}
