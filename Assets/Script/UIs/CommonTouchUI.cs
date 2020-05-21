using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class CommonTouchUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MouseDescription mouseDescription;
    public string description;
    public int mPosY;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (mouseDescription == null)
        {
            mouseDescription = UIManager.instance.mouseDescription;
        }
        mouseDescription.mousePosY = mPosY;
        mouseDescription.LookDiscription(true, description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (mouseDescription == null)
        {
            mouseDescription = UIManager.instance.mouseDescription;
        }
        mouseDescription.LookDiscription(false, description);
    }

    void Start()
    {
        mouseDescription = UIManager.instance.mouseDescription;
    }
}
