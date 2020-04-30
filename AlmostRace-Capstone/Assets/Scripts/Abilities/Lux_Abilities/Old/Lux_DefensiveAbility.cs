﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Eddie B

    The defensive ability for the Lux.
    Spawns a number of shield panels around the Lux that protect it for a set number of damage.  
     */

public class Lux_DefensiveAbility : CooldownAbility
{
    public float shieldHealth;
    public List<GameObject> shields;
    private CarHealthBehavior _carHealthScript;
    public List<GameObject> vfxToActivate = new List<GameObject>();
    public Animator hoodAnimator;
    public bool shouldAnimate = true;

    void Start()
    {
        _carHealthScript = gameObject.GetComponent<CarHealthBehavior>();
        foreach (GameObject shield in shields)
        {
            shield.GetComponent<ParticleSystem>().Stop();
        }
    }

    public override void ActivateAbility()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play("Shield Activated", transform);
        }
        if (_carHealthScript != null)
        {
            _carHealthScript.AddPersonalShields(shieldHealth);
        }
        foreach (GameObject shield in shields)
        {
            shield.GetComponent<ParticleSystem>().Play();
        }
    }

    public override void DeactivateAbility()
    {
        if (_carHealthScript != null)
        {
            _carHealthScript.SetPersonalShieldAmount(0);
        }
        foreach (GameObject shield in shields)
        {
            shield.GetComponent<ParticleSystem>().Stop();
            shield.GetComponent<ParticleSystem>().Clear();
        }
    }

    public override void AbilityOnCooldown()
    {
        if (shouldAnimate)
        {
            //raise hood, play vfx
            hoodAnimator.SetTrigger("HoodUp");

            foreach (GameObject vfx in vfxToActivate)
            {
                vfx.SetActive(true);
            }
        }


    }

    public override void AbilityOffOfCooldown()
    {
        if (shouldAnimate)
        {
            //lower hood, stop vfx
            hoodAnimator.SetTrigger("HoodDown");
            foreach (GameObject vfx in vfxToActivate)
            {
                vfx.SetActive(false);
            }
        }
    }

    public override void AbilityInUse()
    {

    }
}
