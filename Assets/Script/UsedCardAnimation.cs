using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UsedCardAnimation : MonoBehaviour
{
    public Transform endPosition;
    Vector3 original;
    private void OnEnable()
    {
        original = transform.position;

        transform.DOMove(endPosition.position, 0.7f)
                .OnComplete(() =>
                 {
                     transform.position = original;
                     transform.gameObject.SetActive(false);
                 });
    }

}
