using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(EventTrigger))]
public class UIButtonInteractor : MonoBehaviour
{
    private Vector3 pressedScale = new Vector3(0.9f, 0.9f, 0.9f);

    void Start()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entryDown = new EventTrigger.Entry();
        entryDown.eventID = EventTriggerType.PointerDown;
        entryDown.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
        trigger.triggers.Add(entryDown);

        EventTrigger.Entry entryUp = new EventTrigger.Entry();

        entryUp.eventID = EventTriggerType.PointerUp;
        entryUp.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
        trigger.triggers.Add(entryUp);
    }

    public void OnPointerDown(PointerEventData data)
    {
        Debug.Log("OnPointer Down");
        transform.localScale = pressedScale;
    }

    public void OnPointerUp(PointerEventData data)
    {
        Debug.Log("OnPointer up");
        transform.localScale = Vector3.one;
    }
}
