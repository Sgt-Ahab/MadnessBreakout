//Accent Control for the Colors
using UnityEngine;

public class AccentLights : MonoBehaviour
{
    public SpriteRenderer[] lights;
    public float cycleSpeed = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            float hue = Mathf.Repeat((Time.time * cycleSpeed) + (i * 0.15f), 1f);
            lights[i].color = Color.HSVToRGB(hue, 1f, 1f);
        }
    }
}
