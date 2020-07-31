using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TvFlicker : MonoBehaviour
{
    public AnimationCurve flickerCurve;
    private float _baseLight;
    [Range(1f,100f)]
    public float intensityScalar;
    [Range(0.1f, 2f)]
    public float timeScale;

    private Light light;
    // Start is called before the first frame update
    void Start()
    {
       light = GetComponent<Light>();
       _baseLight = light.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        light.intensity = _baseLight + (flickerCurve.Evaluate(Time.time * timeScale) * intensityScalar);
    }
}
