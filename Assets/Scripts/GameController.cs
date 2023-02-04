using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController Instance { get; private set; }

    [Header("Happiness Levels")]
    [SerializeField] float maxHappiness;
    [SerializeField] float happinessHigh;
    [SerializeField] float happinessMed;
    [SerializeField] float happinessLow;
    [SerializeField] float startHappiness;

    [Header("Stage Intervals")]
    [SerializeField] float stageOneTime;
    [SerializeField] float stageTwoTime;
    [SerializeField] float stageThreeTime;
    [SerializeField] float stageFourTime;

    [Header("Sun Levels")]
    [SerializeField] float maxSun;
    [SerializeField] float lowSunLevel;
    [SerializeField] float highSunLevel;
    [SerializeField] float startSunLevel;
    [SerializeField] float sunStepInterval;

    [Header("Water Levels")]
    [SerializeField] float maxWater;
    [SerializeField] float lowWaterLevel;
    [SerializeField] float startWaterLevel;
    [SerializeField] float waterStepInterval;

    [Header("Nutrient Levels")]
    [SerializeField] float maxNutrient;
    [SerializeField] float lowNutrientLevel;
    [SerializeField] float startNutrientLevels;
    [SerializeField] float nutrientStepInterval;

    [Header("Temperture Levels")]    
    [SerializeField] float maxTemperture;
    [SerializeField] float lowTemperture;
    [SerializeField] float midTemperture;
    [SerializeField] float highTemperture;
    [SerializeField] float startTemperture;
    [SerializeField] float tempertureStepInterval;


    [Header("DEBUGGING. DO NOT TOUCH.")]
    [SerializeField] float happiness;
    [SerializeField] float sunLevel;
    [SerializeField] bool sunOn;
    [SerializeField] float waterLevel;
    [SerializeField] float nutrientLevel;
    [SerializeField] float tempertureLevel;
    [SerializeField] bool tempertureOn;
    

    private void Awake()
    {
        Singleton();
    }

    private void Singleton()
    {
        if(Instance != null)
        {
            Instance = this;
            //DontDestroyOnLoad(); MAY NOT NEED THIS
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        happiness = startHappiness;
        sunLevel = startSunLevel;
        waterLevel = startWaterLevel;
        nutrientLevel = startNutrientLevels;
        tempertureLevel = startTemperture;

        sunOn = true;
        tempertureOn = false;

    }

    private void Update()
    {
        AdjustSunLevels();
        AdjustTempertureLevels();
    }

    private void AdjustTempertureLevels()
    {
        if (tempertureOn)
        {
            if(sunOn)
            {
                tempertureLevel += tempertureStepInterval * Time.deltaTime;
            }
            else
            {
                tempertureLevel -= tempertureStepInterval * Time.deltaTime;
            }            
        }
    }

    private void AdjustSunLevels()
    {
        if (sunOn)
        {
            sunLevel += sunStepInterval * Time.deltaTime;
        }
        else
        {
            sunLevel -= sunStepInterval * Time.deltaTime;
        }
    }

    public void TurnOnSun()
    {
        sunOn = true;
    }

    public void TurnOffSun()
    {
        sunOn = false;
    }

    public void AddWater()
    {
        waterLevel += waterStepInterval;
    }

    public void AddNutrients()
    {
        nutrientLevel += nutrientStepInterval;
    }

    

}
