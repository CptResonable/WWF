using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour {
    [SerializeField] private float baseWindSpeed;
    [SerializeField] private float windSpeedChangeSpeed;

    
    [Header("FBM noise properies")]
    [SerializeField] private float frequency = 1;
    [SerializeField] private float amplitude = 1f;
    [SerializeField] private int octaves = 3;
    [SerializeField] private float lacunarity = 2;
    [SerializeField] private float gain = 0.5f;
    
    [Header("Wind output")]
    public Vector3 windDirection;
    public float windSpeed;

    private void Update() {
        CalculateWind();

        Shader.SetGlobalVector("_WindDirection", new Vector4(windDirection.x, windDirection.y, windDirection.z, 0));
        Shader.SetGlobalFloat("_WindSpeed", windSpeed);
    }

    private void CalculateWind() {
        windDirection = new Vector3(Perlin.CustomFbm(Time.time * windSpeedChangeSpeed, frequency, amplitude, octaves, lacunarity, gain), 0, Perlin.CustomFbm(Time.time * windSpeedChangeSpeed + 9323, frequency, amplitude, octaves, lacunarity, gain));
        windSpeed = windDirection.magnitude * baseWindSpeed;
        windDirection.Normalize();
    }
}
