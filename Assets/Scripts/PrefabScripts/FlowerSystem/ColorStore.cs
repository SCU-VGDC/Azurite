using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorStore : MonoBehaviour
{
    // Start is called before the first frame update
    public Color[] colors;
    SpriteRenderer ColorRenderer;
    void Start()
    {
        ColorRenderer = GetComponent<SpriteRenderer>();
        if(ColorRenderer == null)
        {
            Debug.Log("Color Renderer Could not be Found");
        }

    }
    public void ColorChange(int state)
    {
        ColorRenderer.color = colors[state];

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
