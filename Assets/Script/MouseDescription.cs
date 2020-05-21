using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseDescription : MonoBehaviour
{
    private Text text;
    public int mousePosX = 250;
    public int mousePosY = 0;

    private void FixedUpdate()
    {
        Vector3 pos = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x + mousePosX, Input.mousePosition.y+ mousePosY, Input.mousePosition.z-1));
           
        if (pos.x < 0f) pos.x = 0f;
         
        if (pos.x > 0.9f) pos.x = 0.9f;
         
        if (pos.y < 0f) pos.y = 0f;
         
        if (pos.y > 1f) pos.y = 1f;

        transform.position = Camera.main.ViewportToScreenPoint(pos);
    }

    public void LookDiscription(bool isLook, string disc)
    {
        if (text == null)
        {
            text = GetComponentInChildren<Text>();
        }

       gameObject.SetActive(isLook);
       text.text = disc;
    }
}
