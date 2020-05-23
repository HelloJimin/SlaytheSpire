using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ShopHand : MonoBehaviour
{
    public Vector3 OrignalPosition;

    private void OnEnable()
    {
        OrignalPosition = new Vector3(170, 700, 0);
        gameObject.transform.localPosition = OrignalPosition;
    }

    public void GoToMousePosition(Vector3 pos)
    {
        gameObject.transform.DOMove(pos, 0.3f); 
    }
    public void GoToOrignalPosition()
    {
        gameObject.transform.DOLocalMove(OrignalPosition, 0.3f);
    }
}
