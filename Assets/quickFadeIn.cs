using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class quickFadeIn : MonoBehaviour {
    public Image col;

    void Start()
    {
        col.color = Color.clear;
    }

    private void Update()
    {
        col.color += new Color(1f, 1f, 1f, .01f);
    }
}
