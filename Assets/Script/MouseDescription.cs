using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseDescription : MonoBehaviour
{
    public Text text;
    public bool isLook;
    void Start()
    {
        text = GetComponentInChildren<Text>();
    }

    void Update()
    {
        Vector3 pos = new Vector3(Input.mousePosition.x + 250, Input.mousePosition.y, Input.mousePosition.z);
        transform.position = pos;
    }

    public void SetDiscription(string disc)
    {
        text.text = disc;
    }
    public void LookDiscription(bool isLook, string disc)
    {
        gameObject.SetActive(isLook);
        text.text = disc;
    }
}
