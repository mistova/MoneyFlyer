using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour
{
    ParticleSystem particleWind;

    [SerializeField] float startWindEmissionRate = 50;
    [SerializeField] float startWindSpeed = 0.35f;
    [SerializeField] float startWindWidth = 0.15f;


    [SerializeField] float incWindEmissionRate = 50;
    [SerializeField] float incWindSpeed = 0.005f;
    [SerializeField] float incWindWidth = 0.075f;

    float emissionVal = 50;

    //Tapping ile artacak
    float moreEmission = 0;
    private void Awake()
    {
        particleWind = GetComponent<ParticleSystem>();
    }

    public void SetWindProperties(int level,int propellerLevelIndex)
    {
        //print("Wind Level: " + level);
        //print("Propeller Level Index: " + propellerLevelIndex);

        SetWindEmission(level);
        SetWindSpeed(level);
        SetWindWidth(propellerLevelIndex);
    }

    public void SetWindSpeed(int level)
    {
        ParticleSystem.MainModule mainModule = particleWind.main;
        mainModule.simulationSpeed = (startWindSpeed + moreEmission + level * incWindSpeed);
    }

    public void SetWindWidth(int propellerLevelIndex)
    {
        //Vector3 scale = transform.localScale;
        //scale.y = startWindWidth + propellerLevelIndex * incWindWidth;
        //transform.localScale = scale;

        ParticleSystem.ShapeModule shape = particleWind.shape;
        Vector3 shapeScale = shape.scale;
        shapeScale.x = startWindWidth + propellerLevelIndex * incWindWidth;
        shape.scale = shapeScale;

    }

    //public void SetWindLifeTime(int level)
    //{
    //    ParticleSystem.MainModule mainModule = particleWind.main;
    //    mainModule.startLifetime = level * 0.025f;
    //}

    private void SetWindEmission(int level)
    {
        ParticleSystem.EmissionModule curve = particleWind.emission;
        emissionVal = curve.rateOverTime.constant;
        emissionVal = startWindEmissionRate  + incWindEmissionRate * level;
        curve.rateOverTime = emissionVal;
    }


    public void AddMoreWindSpeed(float speed)
    {
        moreEmission = speed;
        SetWindSpeed(PlayerPrefs.GetInt("LvlPropellerPower"));
    }


}
