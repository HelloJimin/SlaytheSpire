using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NobGoblin : Monster
{
    private void OnEnable()
    {
        Init(30, 10);
        spain.AnimationName = "Animation";
    }
}
