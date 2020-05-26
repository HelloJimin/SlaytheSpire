using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LobbyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Vector3 ogScale;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ogScale = transform.localScale;

        transform.localScale = ogScale+ new Vector3(1f, 1f, 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = ogScale;
    }
}
