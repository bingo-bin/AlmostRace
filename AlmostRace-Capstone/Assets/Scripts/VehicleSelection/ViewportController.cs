﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewportController : MonoBehaviour
{
    public int playerID;
    public int selectedCarID;
     public GameObject playerStatus;
    public GameObject activeStatus;
    public SelectionManager selectionManager;
    public RawImage[] InfoPanels = new RawImage[4];
    public GameObject noPlayerPanel;
    public GameObject vehicleRotationHolder;
    private RotateSelection _rotateSelection;
    private TextMeshProUGUI _text;
    private PlayerInput _playerInput;
    private bool _ready;
    private bool _joined;
    private int _vehicleCount;
    private int _selectedInfoPanel;

    private void Start()
    {
        _text = playerStatus.GetComponent<TextMeshProUGUI>();
        _vehicleCount = selectionManager.amountOfSelections;
        _rotateSelection = vehicleRotationHolder.GetComponent<RotateSelection>();
        vehicleRotationHolder.SetActive(false);
        noPlayerPanel.SetActive(true);
    }

    private void Update()
    {
        if (_joined)
        {
            if (!_ready)
            {
                VehicleScroll();
                InfoScroll();

                if (Input.GetButtonDown(_playerInput.selectButton))
                {
                    VehicleSelect(true);
                }
                else if (Input.GetButtonDown(_playerInput.backButton))
                {
                    PlayerJoin(false, null);
                }
            }
            else
            {
                if (Input.GetButtonDown(_playerInput.backButton))
                {
                    VehicleSelect(false);
                }
            }
        }
    }

    private void VehicleScroll()
    {
        if (Input.GetAxis(_playerInput.horizontal) > 0.3f)
        {
            if(!_rotateSelection.GetSwitching())
            {
                _rotateSelection.SetRightOrLeft(true);
                _rotateSelection.SetSwitching(true);
                if (selectedCarID >= _vehicleCount - 1)
                {
                    selectedCarID = 0;
                }
                else
                {
                    selectedCarID = selectedCarID + 1;
                }
            }
        }
        else if (Input.GetAxis(_playerInput.horizontal) < -0.3f)
        {
            if(!_rotateSelection.GetSwitching())
            {
                _rotateSelection.SetRightOrLeft(false);
                _rotateSelection.SetSwitching(true);
                if (selectedCarID <= 0)
                {
                    selectedCarID = _vehicleCount - 1;
                }
                else
                {
                    selectedCarID = selectedCarID - 1;
                }
            }
        }
    }

    private void InfoScroll()
    {
        if(Input.GetButtonDown(_playerInput.bumperRight))
        {
            if (_selectedInfoPanel >= InfoPanels.Length - 1)
            {
                _selectedInfoPanel = 0;
            }
            else
            {
                _selectedInfoPanel = _selectedInfoPanel + 1;
            }
            for (int i = 0; i < InfoPanels.Length; i++)
            {
                if (i == _selectedInfoPanel)
                {
                    InfoPanels[i].enabled = true;
                }
                else
                {
                    InfoPanels[i].enabled = false;
                }
            }
        }
        else if (Input.GetButtonDown(_playerInput.bumperLeft))
        {
            if (_selectedInfoPanel <= 0)
            {
                _selectedInfoPanel = InfoPanels.Length - 1;
            }
            else
            {
                _selectedInfoPanel = _selectedInfoPanel - 1;
            }
            for (int i = 0; i < InfoPanels.Length; i++)
            {
                if (i == _selectedInfoPanel)
                {
                    InfoPanels[i].enabled = true;
                }
                else
                {
                    InfoPanels[i].enabled = false;
                }
            }
        }
    }
    
    public void PlayerJoin(bool status, PlayerInput controllerNumber)
    {
        if (status == true)
        {
            _playerInput = controllerNumber;
            _joined = true;
            _text.text = "PLAYER " + playerID;
            activeStatus.gameObject.SetActive(false);
            vehicleRotationHolder.SetActive(true);
            noPlayerPanel.SetActive(false);
            activeStatus.gameObject.GetComponent<TextMeshProUGUI>().text = "SELECT A VEHICLE";
        }
        else if (status == false)
        {
            _joined = false;
            _ready = false;
            _text.text = "NO PLAYER";
            activeStatus.gameObject.SetActive(true);
            selectionManager.UpdateData(playerID, _ready, selectedCarID, 0);
            vehicleRotationHolder.SetActive(false);
            noPlayerPanel.SetActive(true);
            activeStatus.gameObject.GetComponent<TextMeshProUGUI>().text = "PRESS Y TO JOIN";
        }
    }

    private void VehicleSelect(bool status)
    {
        if (status == true)
        {
            _ready = true;
            activeStatus.gameObject.SetActive(true);
            activeStatus.gameObject.GetComponent<TextMeshProUGUI>().text = "READY";
            selectionManager.UpdateData(playerID, _ready, selectedCarID, _playerInput.GetPlayerNum());
        }
        else
        {
            _ready = false;
            selectionManager.UpdateData(playerID, _ready, selectedCarID, _playerInput.GetPlayerNum());
            activeStatus.gameObject.SetActive(true);
            activeStatus.gameObject.GetComponent<TextMeshProUGUI>().text = "SELECT A VEHICLE";
        }
    }

    public bool GetReady()
    {
        return _ready;
    }

    public bool GetJoined()
    {
        return _joined;
    }

    public PlayerInput GetInput()
    {
        return _playerInput;
    }

}
