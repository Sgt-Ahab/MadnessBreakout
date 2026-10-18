//Name: David Sargent
//Date: 10-16-2026
//Desc: Cycles through Paddle Nested lights and makes them colorful
//Attach: MainLevel.Paddle
//Accent Control for the Colors, Attached to Paddle
using UnityEngine;

public class AccentLights : MonoBehaviour
{
    public SpriteRenderer[] lights;
    public float cycleSpeed = 0.5f;
    // Update is called once per frame
    void Update()
    {
        //Iterate each light
        for (int i = 0; i < lights.Length; i++)
        {
            //Make the hue for HSL cycle for each color to differ
            float hue = Mathf.Repeat((Time.time * cycleSpeed) + (i * 0.15f), 1f);
            lights[i].color = Color.HSVToRGB(hue, 1f, 1f);
        }
    }
}
