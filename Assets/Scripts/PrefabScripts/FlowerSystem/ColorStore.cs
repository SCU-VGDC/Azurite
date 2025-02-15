using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ColorStore : MonoBehaviour
{
    // Start is called before the first frame update
    public Color[] colors; //List of available colors. MAKE AS MANY AS THE MAIN LIGHT HAS COLORS.
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
        ColorRenderer.color = colors[state]; //Changes the color of the sprite based on the current lighting of the room.
    }
}
