using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventPageTrigger
{
    private TriggerType _triggerType = TriggerType.ActionButton;
    private MoveStatusHandler moveStatusHandler = GameObject.FindWithTag("Player").GetComponent<MoveStatusHandler>();
    private bool _pushKey = false;

    public EventPageTrigger(TriggerType triggerType) =>
        _triggerType = triggerType;

    public bool IsActionButtonValid()
    {
        if (_triggerType == TriggerType.ActionButton)
        {
            if (Input.GetKey(KeyCode.UpArrow) && !_pushKey)
            {
                _pushKey = true;
                if (!moveStatusHandler.CanJump)
                    return false;
                return true;
            }
            else if (!Input.GetKey(KeyCode.UpArrow) && _pushKey)
                _pushKey = false;
            return false;
        }
        return false;
    }

    public bool IsPlayerTouch()
    {
        if (_triggerType == TriggerType.PlayerTouch)
            return true;
        return false;
    }

    public bool IsAutorunValid()
    {
        if (_triggerType == TriggerType.AutoRun)
            return true;
        return false;
    }

    public TriggerType TriggerType => _triggerType;
}