using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
  public void Shake()
    {
        Vector3 original = transform.position;

        transform.DOMove(new Vector3(transform.position.x-0.1f, transform.position.y+0.1f, transform.position.z), 0.05f)
        .OnComplete(() =>
        {
            transform.DOMove(original, 0.05f);
        });
    }
}
