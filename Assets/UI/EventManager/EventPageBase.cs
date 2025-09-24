using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventPageBase : MonoBehaviour
{
    private IEnumerator _eventCoroutine;

    protected EventPageTrigger _eventPageTrigger;
    protected EventPageCondition _eventPageCondition;

    protected virtual void Update()
    {
        if (_eventPageTrigger.IsAutorunValid())
            if (_eventPageCondition.IsAllValid())
                StartContents();
    }

    protected void StartContents()
    {
        if (_eventCoroutine != null) { StopCoroutine(_eventCoroutine); }
        _eventCoroutine = Contents();
        StartCoroutine(_eventCoroutine);
    }

    protected abstract IEnumerator Contents();

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_eventPageTrigger.IsActionButtonValid())
            {
                if (_eventPageCondition.IsAllValid())
                    StartContents();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            if (_eventPageTrigger.IsPlayerTouch())
                if (_eventPageCondition.IsAllValid())
                    StartContents();
    }

    // Message
    protected void EnterActor(string name, DirectionType direction, string gesture) =>
        EnterActorCommand.Instance.Execute(name, direction, gesture);

    protected void ExitActor(string name, DirectionType direction) =>
        ExitActorCommand.Instance.Execute(name, direction);

    protected void UpdateActor(string name, string gesture) =>
        UpdateActorCommand.Instance.Execute(name, gesture);

    protected IEnumerator ShowText(string name, DirectionType direction, string text) =>
        ShowTextCommand.Instance.ExecuteCoroutine(name, direction, text);

    protected void HideText() =>
        HideTextCommand.Instance.Execute();

    // Game Progression
    protected void controlSwitch(Dictionary<string, bool> switchOperation) =>
        ControlSwitchesCommand.Instance.Execute(switchOperation);
}