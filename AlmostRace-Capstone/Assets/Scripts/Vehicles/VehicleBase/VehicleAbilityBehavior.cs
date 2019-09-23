﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleAbilityBehavior : MonoBehaviour
{
    [Header ("Basic Ability")]
    [Tooltip("Basic Ability Script Slot")] public BasicAbility basicAbility;
    [Tooltip("The Button for using a Basic Ability")] public string basicAbilityInput;
    [Tooltip("Determines if the basic ability input can be held down")] public bool canHoldBasic;
    private bool _canUseBasic = true;

    [Header ("Signature Ability")]
    [Tooltip("Signature Ability Script Slot")] public Ability signatureAbility;
    [Tooltip("Length of ability cooldown in seconds.")] public float abilityRecharge = 5f;
    [Tooltip("The Button for using a Signature Ability")] public string signatureAbilityInput;
    [Tooltip("Determines if the signature ability input can be held down")] public bool canHoldSignature;
    private bool _canUseSignature = true;
    
    [Header ("Pickup Ability")]
    [Tooltip("Pickup Ability Script Slot")] public Ability pickup;
    [Tooltip("The Button for using Pickups")] public string pickupInput;


    // Update is called once per frame
    void Update()
    {
        // Basic Ability Call
        checkFireAbility(basicAbility, basicAbilityInput, _canUseBasic, canHoldBasic);
        
        // Signature Ability Call
        if (checkFireAbility(signatureAbility, signatureAbilityInput, _canUseSignature, canHoldSignature))
        {
            _canUseSignature = false;
            StartCoroutine(AbilityCooldown());
        }

        // Pickup Ability Call
        if (Input.GetButtonDown(pickupInput) && pickup != null)
        {
            pickup.ActivateAbility();
        }
    }

    // Handles the ability call, on what input it is, if it can be used, and if it can be held down
    private bool checkFireAbility(Ability ability, string abilityInput, bool canFire, bool canHoldInput)
    {
        if (canHoldInput)
        {
            if (Input.GetButton(abilityInput) && canFire && ability != null)
            {
                ability.ActivateAbility();
                return true;
            }
            else if (Input.GetButtonUp(abilityInput) && ability != null)
            {
                ability.DeactivateAbility();
                return false;
            }
        }
        else
        {
            if (Input.GetButtonDown(abilityInput) && canFire && ability != null)
            {
                ability.ActivateAbility();
                return true;
            }
        }
        return false;
    }

    // Countdown timer until reuse allowed for abilites that need a cooldown
    private IEnumerator AbilityCooldown()
    {
        float tempTime = abilityRecharge;

        while (tempTime > 0)
        {
            tempTime -= Time.deltaTime;
            Mathf.Lerp(0, 1, tempTime);
            yield return null;
        }
        _canUseSignature = true;
    }

    // Returns if the pickup slot is vacant or occupied
    public bool hasPickup()
    {
        if (pickup != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Assigns the pickup slot with a given pickup
    public void assignPickup(GameObject givenPickup)
    {
        pickup = givenPickup.GetComponent<Ability>();
    }
}
