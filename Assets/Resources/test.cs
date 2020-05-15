using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject stt;
    public GameObject end;
    public LineRenderer t;
    // Start is called before the first frame update
    void Start()
    {
        t.SetPosition(0, stt.transform.position);
        t.SetPosition(1, end.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        t.SetPosition(0, stt.transform.position);
        t.SetPosition(1, end.transform.position);
    }
}
