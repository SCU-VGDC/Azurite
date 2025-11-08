using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class persistenceDebug : MonoBehaviour
{
    // Start is called before the first frame update
    SpriteRenderer m_SpriteRenderer;
    //The Color to be assigned to the Renderer’s Material
    Color m_NewColor;
    //These are the values that the Color Sliders return
    float m_Red, m_Blue, m_Green;
    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        PersistentDataManager.Instance.Set("worldState", 2);
    }

    // Update is called once per frame
    void Update()
    {
        if ((int)PersistentDataManager.Instance.Get("worldState") == 2)
        {
            m_SpriteRenderer.color = Color.green;
        }
        else
        {
            m_SpriteRenderer.color = Color.red;
        }

    }
}
