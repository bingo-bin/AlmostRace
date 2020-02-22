﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivationEvent : UnityEvent<GameObject>
{

}

public class ActivateAbilities : MonoBehaviour
{
    public ActivationEvent _ability;
    private void Start()
    {
        if(_ability == null)
        {
            _ability = new ActivationEvent();
        }

        _ability.AddListener(AbilitiesOn);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<VehicleAbilityBehavior>())
        {
            _ability.Invoke(other.gameObject);
        }
    }


    void AbilitiesOn(GameObject car)
    {
        car.GetComponent<VehicleAbilityBehavior>().Activation();
    }
}
