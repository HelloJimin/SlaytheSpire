using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostUI : MonoBehaviour
{
    Image[] images;

    void Start()
    {
        images = GetComponentsInChildren<Image>();
        StartCoroutine(Bingle());
    }


    IEnumerator Bingle()
    {
        images[2].rectTransform.Rotate(Vector3.forward * Time.deltaTime * 150f);
        images[3].rectTransform.Rotate(Vector3.forward * Time.deltaTime * -150f);
        images[4].rectTransform.Rotate(Vector3.forward * Time.deltaTime * 130f);

        yield return new WaitForSeconds(0.05f);
        StartCoroutine(Bingle());
    }
}
