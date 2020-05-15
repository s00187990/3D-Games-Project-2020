using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextWithShadow : MonoBehaviour
{
    public string Text;

    public TextMeshProUGUI shadowObject;
    public TextMeshProUGUI textObject;

    private void Start()
    {
        shadowObject.text = textObject.text = Text;
    }
}
