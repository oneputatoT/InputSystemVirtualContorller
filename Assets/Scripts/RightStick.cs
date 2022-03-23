using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RightStick : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] Transform leftButton;
    [SerializeField] CancleButton cancle;
    bool enableAtk;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out m_PointerDownPos);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out var position);
        var delta = position - m_PointerDownPos;

        enableAtk = IsEnableAtk(position);

        delta = Vector2.ClampMagnitude(delta, movementRange);
        ((RectTransform)leftButton).anchoredPosition = m_StartPos + (Vector3)delta;

        var newPos = new Vector2(delta.x / movementRange, delta.y / movementRange);
        SendValueToControl(newPos);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ((RectTransform)leftButton).anchoredPosition = m_StartPos;

        if(enableAtk) cancle.PointerClick();

        SendValueToControl(Vector2.zero);
    }

    public bool IsEnableAtk(Vector3 poiontPositon)
    {
        float currentDistance = (poiontPositon - cancle.transform.localPosition).magnitude;

        if (currentDistance > 20f)
        {
            cancle.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            return true;
        }
        else
        {
            cancle.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            return false;
        }
    }

    private void Start()
    {
        m_StartPos = ((RectTransform)leftButton).anchoredPosition;
    }

    public float movementRange
    {
        get => m_MovementRange;
        set => m_MovementRange = value;
    }

    [FormerlySerializedAs("movementRange")]
    [SerializeField]
    private float m_MovementRange = 50;

    [InputControl(layout = "Vector2")]
    [SerializeField]
    private string m_ControlPath;

    private Vector3 m_StartPos;
    private Vector2 m_PointerDownPos;

    protected override string controlPathInternal
    {
        get => m_ControlPath;
        set => m_ControlPath = value;
    }
}
