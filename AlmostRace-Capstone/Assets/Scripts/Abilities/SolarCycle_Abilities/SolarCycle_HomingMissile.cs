﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    /*
    Author: Jake Velicer
    Purpose: AMoming missile for the VoidWasp. This script takes in variables from the corresponding VoidWasp Attack Script.
    The missile then moves forward and toward its given target.
    */

public class SolarCycle_HomingMissile : Projectile
{
    public ProjectileNormalizer normalizerScript;
    public string poolTag = "SCHomingMissile";
    public string explodeVFXTag = "SCExplodeVFX";
    private GameObject _target;
    private Quaternion _missileTargetRotation;
    private CarHealthBehavior _carHealthStatus;
    private Interactable _interactableStatus;
    private float _turnRate;
    private float _hangTime;
    private bool _canTrack;
    private ObjectPooler _objectPooler;

    private CarHealthBehavior carHit;

    public void SetAdditionalInfo(GameObject giventTarget, float givenTurnRate, float givenHangTime, float maxLifeTime)
    {
        _objectPooler = ObjectPooler.instance;
        _target = giventTarget;
        _turnRate = givenTurnRate;
        _hangTime = givenHangTime;
        OnMissileActivation();
        StartCoroutine(_objectPooler.DeactivateAfterTime(poolTag, gameObject, maxLifeTime));
    }

    private void OnMissileActivation()
    {
        GiveSpeed();
        if (_target != null)
        {
            normalizerScript.enabled = false;
            StartCoroutine(hangTimeSequence());
            _carHealthStatus = _target.GetComponent<CarHealthBehavior>();
            _interactableStatus = _target.GetComponent<Interactable>();
        }
        else
        {
            normalizerScript.enabled = true;
        }
    }

    public void AttackTriggered(GameObject givenCollision)
    {
        if(givenCollision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Explode();
        }
        if(givenCollision.gameObject.GetComponent<CarHealthBehavior>() != null)
        {
            if(givenCollision.gameObject != _immunePlayer)
            {
                carHit = givenCollision.gameObject.GetComponent<CarHealthBehavior>();
                carHit.DamageCar(_projectileDamage, _immunePlayerScript.playerID);
                Explode();
            }
        }
        if(givenCollision.gameObject.GetComponent<Interactable>() != null)
        {
            givenCollision.gameObject.GetComponent<Interactable>().DamageInteractable(_projectileDamage);
            Explode();
        }
    }

    private IEnumerator hangTimeSequence()
    {
        yield return new WaitForSeconds(_hangTime);
        _canTrack = true;
    }

    private void FixedUpdate()
    {
        _rigidBody.velocity = transform.forward * _projectileSpeed;
        if(_canTrack)
        {
            if(_target != null)
            {
                _missileTargetRotation = Quaternion.LookRotation(_target.transform.position - transform.position);
                _rigidBody.MoveRotation(Quaternion.RotateTowards(transform.rotation, _missileTargetRotation, _turnRate));
            }
            else
            {
                _canTrack = false;
            }
        }
    }

    private void Update()
    {
        if(_target != null)
        {
            if(_carHealthStatus != null && _carHealthStatus.healthCurrent <= 0 && _interactableStatus == null)
            {
                _canTrack = false;
                normalizerScript.enabled = true;
            }
            else if(_interactableStatus != null && _interactableStatus.interactableHealth <= 0 && _carHealthStatus == null)
            {
                _canTrack = false;
                normalizerScript.enabled = true;
            }
        }        
    }

    public void TrackDuringRuntime(GameObject givenTarget)
    {
        _target = givenTarget;
        _carHealthStatus = _target.GetComponent<CarHealthBehavior>();
        _interactableStatus = _target.GetComponent<Interactable>();
        _canTrack = true;
        normalizerScript.enabled = false;
    }

    private void Explode()
    {
        GameObject explodeVFXObject = _objectPooler.SpawnFromPool(explodeVFXTag, transform.position, transform.rotation);
        _objectPooler.StartCoroutine(_objectPooler.DeactivateAfterTime(explodeVFXTag, explodeVFXObject, 1));
        AudioManager.instance.Play("VoidWasp Shot Hit", gameObject.transform);
        normalizerScript.enabled = true;
        _objectPooler.Deactivate(poolTag, gameObject);
    }

    public GameObject GetImmunePlayer()
    {
        return _immunePlayer;
    }

    public GameObject GetTarget()
    {
        return _target;
    }

    public bool GetTracking()
    {
        return _canTrack;
    }

}
