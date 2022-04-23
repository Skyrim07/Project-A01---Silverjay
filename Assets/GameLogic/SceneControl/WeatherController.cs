using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;
public sealed class WeatherController : MonoSingleton<WeatherController>
{
    public SceneWeather weather;
    public ParticleSystem rainEffect, subRainEffect, rainFogEffect;

    private float timer;
    private ParticleSystem.MainModule rainModule;
    private ParticleSystem.MainModule subRainModule;
    private ParticleSystem.MainModule fogRainModule;

    private void Start()
    {
        rainModule = rainEffect.main;
        subRainModule = subRainEffect.main;
        fogRainModule = rainFogEffect.main;
    }
    private void Update()
    {
        if(weather.type == WeatherType.None)
        {
            return;
        }
        
        timer += Time.deltaTime;
        if(timer > weather.interval)
        {
            if(weather.type == WeatherType.Rain)
                ToggleRain(Random.value < weather.probability);
            timer = 0;
        }
    }

    public void OnLoadWeatherAsset(SceneWeather weather)
    {
        this.weather = weather;

        //Disable all weather
        ToggleRain(false);

        //Enable specific weather
        if(weather.type == WeatherType.Rain && weather.activeOnStart)
            ToggleRain(true);
    }
    public void ToggleRain(bool isOn)
    {
        if (RuntimeData.weather_IsRaining == isOn)
            return;

        RuntimeData.weather_IsRaining = isOn;

        ParticleSystem.MinMaxGradient g = rainModule.startColor;
        float oAlpha = 1;
        CommonUtils.StartProcedure(SKCurve.LinearIn, 0.5f, (f) =>
        {
            g.color = new Color(g.color.r, g.color.g, g.color.b, isOn?(f*oAlpha) : (1 - f) * oAlpha);
            rainModule.startColor = g;
            fogRainModule.startColor = new Color(0.01f,0.01f,0.01f,g.color.a);
        }, (f) =>
        {
            CommonUtils.InvokeAction(1f, () =>
            {
                CommonUtils.StartProcedure(SKCurve.LinearIn, 0.2f, (ff) =>
                {
                    g.color = new Color(g.color.r, g.color.g, g.color.b, isOn ? (ff * oAlpha * 0.7f) : (1 - ff) * oAlpha * 0.7f);
                    subRainModule.startColor = g;
                });
            });
        });
        if (isOn)
            SKAudioManager.instance.PlayIdentifiableSound("Rain", "Rain", true);
        else
            SKAudioManager.instance.StopIdentifiableSound("Rain", 3f);
    }
}
