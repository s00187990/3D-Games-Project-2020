using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{

    public Camera newMainCamera;
    public GameObject[] otherText;
    public float speed;
    public bool scroll = true;
    public float currentY = -800;

    private void Start()
    {
        Time.timeScale = 1;
    }

    private void Update()
    {

        if (!scroll) return;

        Vector3 currentPos = transform.localPosition;
        currentPos.y += speed * Time.deltaTime;
        currentPos.y = Mathf.Clamp(currentPos.y, -800, 800);
        transform.localPosition = currentPos;

        scroll = transform.localPosition.y <= 799;

        //print((transform.localPosition.y >= 800) + " - " + transform.localPosition.y);
        if (!scroll)
            foreach (var item in otherText)
                item.SetActive(false);


    }

}
