using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{

    public static GameController Instance { get; private set; }

    [Header("Happiness Levels")]
    [SerializeField] float happinessDecayInterval;    

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
    [SerializeField] Image sunButton;
    [SerializeField] Image shadeButton;
    private Color sunBtnBGColor;
    

    [Header("Water Levels")]
    [SerializeField] float maxWater;
    [SerializeField] float lowWaterLevel;
    [SerializeField] float highWaterLevel;
    [SerializeField] float startWaterLevel;
    [SerializeField] float waterDecreaseInterval;
    [SerializeField] float waterIncreaseStepInterval;
    [SerializeField] ParticleSystem rainParticle;
    [SerializeField] float disableWaterBtnTime;
    float waterBtnTimer;

    [Header("Nutrient Levels")]
    [SerializeField] float maxNutrient;
    [SerializeField] float lowNutrientLevel;
    [SerializeField] float highNutrientLevel;
    [SerializeField] float startNutrientLevels;
    [SerializeField] float nutrientDescreaseInterval;
    [SerializeField] float nutrientIncreaseStepInterval;
    [SerializeField] ParticleSystem nutrientParticle;
    [SerializeField] float disableNutrientBtnTime;
    float nutrientBtnTimer;

    [Header("Temperture Levels")]    
    [SerializeField] float maxTemperture;
    [SerializeField] float minTemperture;
    [SerializeField] float lowTemperture;
    [SerializeField] float midTemperture;
    [SerializeField] float highTemperture;
    [SerializeField] float startTemperture;
    [SerializeField] float tempertureStepInterval;


    [Header("DEBUGGING. DO NOT TOUCH.")]
    [SerializeField] float happiness;
    [SerializeField] bool happy;
    [SerializeField] float sunLevel;
    [SerializeField] bool sunLevelLowOrHigh;

    [SerializeField] bool sunOn;
    [SerializeField] float waterLevel;
    [SerializeField] bool waterLevelLow;
    [SerializeField] float nutrientLevel;
    [SerializeField] bool nutrientLevelLow;
    
    [SerializeField] float tempertureLevel;

    [SerializeField] bool tempertureOn;

    

    [Header("References")]
    [SerializeField] List<GameObject> stageForms = new List<GameObject>();



    private void Awake()
    {
        Singleton();
    }

    private void Singleton()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            //DontDestroyOnLoad(); MAY NOT NEED THIS
        }
        else
        {
            Instance = this;
            
        }
    }

    private void Start()
    {
        sunBtnBGColor = sunButton.color;
        sunLevel = startSunLevel;
        waterLevel = startWaterLevel;
        nutrientLevel = startNutrientLevels;
        tempertureLevel = startTemperture;

        waterBtnTimer = disableWaterBtnTime; //Ensure water can be used immediatly
        nutrientBtnTimer = disableNutrientBtnTime; //Ensure Nutrient can be used immediatly
        TurnOffAllStageForms();
        TurnOnStage1();
        TurnOnSun();
        tempertureOn = false;



    }

    private void TurnOnStage1()
    {
        stageForms[0].SetActive(true) ;
    }

    private void TurnOffAllStageForms()
    {
        foreach(var form in stageForms)
        {
            form.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        AdjustSunLevels();
        AdjustWaterLevels();
        AdjustNutrientLevels();
        AdjustTempertureLevels();

        UpdateNutrientBool();
        UpdateWaterBool();
        UpdateSunBool();
        UpdateHappinessBool();

        waterBtnTimer += Time.deltaTime;
        nutrientBtnTimer += Time.deltaTime;

    }

    private void UpdateNutrientBool()
    {
        if(nutrientLevel < lowNutrientLevel)
        {
            nutrientLevelLow = true;
        }
        else
        {
            nutrientLevelLow = false;
        }


    }

    private void UpdateWaterBool()
    {
        if(waterLevel < lowWaterLevel)
        {
            waterLevelLow = true;
        }
        else
        {
            waterLevelLow = false;
        }
    }

    private void UpdateSunBool()
    {
        //Decrease Happiness if too low or too high
        if (sunLevel < lowSunLevel || sunLevel > highSunLevel)
        {
            sunLevelLowOrHigh = true;
        }
        else
        {
            sunLevelLowOrHigh = false;
        }

    }

    private void UpdateHappinessBool()
    {

        if (nutrientLevelLow == true || waterLevelLow == true || sunLevelLowOrHigh == true)
        {
            happy = false;
        }
        else
        {
            happy = true;
        }

    }

    private void AdjustNutrientLevels()
    {
        nutrientLevel -= nutrientDescreaseInterval * Time.deltaTime;
    }

    private void AdjustWaterLevels()
    {
        waterLevel -= waterDecreaseInterval * Time.deltaTime;
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
        shadeButton.color = sunBtnBGColor;
        sunButton.color = Color.grey;

        sunOn = true;
    }

    public void TurnOffSun()
    {
        shadeButton.color = Color.grey;
        sunButton.color = sunBtnBGColor;
        sunOn = false;
    }

    public void AddWater()
    {
        if(waterLevel < highWaterLevel && waterBtnTimer >= disableWaterBtnTime )
        {
            Instantiate(rainParticle);
            waterLevel += waterIncreaseStepInterval;
            waterBtnTimer = 0;
        }

    }

    public void AddNutrients()
    {
        if(nutrientLevel < highNutrientLevel && nutrientBtnTimer >= disableNutrientBtnTime)
        {
            Instantiate(nutrientParticle);
            nutrientLevel += nutrientIncreaseStepInterval;
            nutrientBtnTimer = 0;
        }
        
    }

    

}
