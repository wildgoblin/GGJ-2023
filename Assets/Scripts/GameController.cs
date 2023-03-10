using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;


public class GameController : MonoBehaviour
{

    public static GameController Instance { get; private set; }

    [Header("Happiness Levels")]
    [SerializeField] float happinessDecayInterval;
    [SerializeField] List<Sprite> happyFaces = new List<Sprite>();
    [SerializeField] List<Sprite> sadFaces = new List<Sprite>();

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
    [SerializeField] AudioClip switchingSound;
    

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
    [SerializeField] AudioClip wateringSound;

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
    [SerializeField] AudioClip nutrientsSound;
    

    [Header("Temperture Levels")]    
    [SerializeField] float maxTemperture;
    [SerializeField] float minTemperture;
    [SerializeField] float lowTemperture;
    [SerializeField] float midTemperture;
    [SerializeField] float highTemperture;
    [SerializeField] float startTemperture;
    [SerializeField] float tempertureStepInterval;
    [SerializeField] Slider tempertureSlider;
    

    [Header("Slugs")]
    
    [SerializeField] float slugSpawnTimer;
    float slugTimer;
    [SerializeField] float slugMovementSpeed;
    [SerializeField] float slugStopDistance;
    [SerializeField] float slugDieTimer;
    [SerializeField] GameObject slugPrefab;
    [SerializeField] GameObject slugParentObject;
    [SerializeField] Transform[] slugSpawnerLocation;
    [SerializeField] GameObject sprayBottleObject;
    SpriteRenderer sprayBottleSpriteRenderer;
    [SerializeField] AudioClip sprayBottleSound;
    AudioSource slugParentAudioSource;
    [SerializeField] Button sprayBottleButton;



    [Header("DEBUGGING. DO NOT TOUCH.")]
    [SerializeField] float happiness;
    [SerializeField] float timeAsHappy;
    [SerializeField] bool happy;
    bool usingHappyFace;
    [SerializeField] float sunLevel;
    [SerializeField] bool sunLevelLowOrHigh;

    [SerializeField] bool sunOn;
    [SerializeField] float waterLevel;
    [SerializeField] bool waterLevelLow;
    [SerializeField] float nutrientLevel;
    [SerializeField] bool nutrientLevelLow;

    [SerializeField] bool spraySelected;
    [SerializeField] float tempertureLevel;
    [SerializeField] bool tempertureLowOrHigh;

    [SerializeField] bool tempertureOn;

    

    [Header("References")]
    [SerializeField] List<GameObject> stageForms = new List<GameObject>();
    [SerializeField] PlayableDirector timelineDirector;
    AudioSource audioSource;
    int currentStageIndex;


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
        tempertureSlider.gameObject.SetActive(false);
        tempertureLowOrHigh = false;
        sprayBottleButton.gameObject.SetActive(false);
        timeAsHappy = 0;

        //References
        audioSource = GetComponent<AudioSource>();
        sprayBottleSpriteRenderer = sprayBottleObject.GetComponent<SpriteRenderer>();
        slugParentAudioSource = slugParentObject.GetComponent<AudioSource>();

        waterBtnTimer = disableWaterBtnTime; //Ensure water can be used immediatly
        nutrientBtnTimer = disableNutrientBtnTime; //Ensure Nutrient can be used immediatly
        TurnOffAllStageForms();
        TurnOnStage(0); // Start at stage 1
        TurnOnSun();
        tempertureOn = false;



    }

    private void TurnOnStage(int stageIndex)
    {
        currentStageIndex = stageIndex;
        TurnOffAllStageForms();
        stageForms[stageIndex].SetActive(true);

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
        UpdateTempertureBool();
        UpdateTempertureSlider();
        UpdateHappinessBool();
        UpdateShroomFaces();
        UpdateAndSpawnSlugs();
        UpdateSlugSFX();
        


        UpdateSprayBottle();

        UpdateStage();

        
        waterBtnTimer += Time.deltaTime;
        nutrientBtnTimer += Time.deltaTime;
        if(happy) { timeAsHappy += Time.deltaTime; }
       

    }



    private void UpdateShroomFaces()
    {
        if(happy && !usingHappyFace)
        {
            int randomHappyFace = UnityEngine.Random.Range(0, happyFaces.Count);
            stageForms[currentStageIndex].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = happyFaces[randomHappyFace];
            usingHappyFace = true;
        }
        else if (!happy && usingHappyFace)
        {
            int randomSadFace = UnityEngine.Random.Range(0, sadFaces.Count);
            stageForms[currentStageIndex].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sadFaces[randomSadFace];
            usingHappyFace = false;
        }

    }

    private void UpdateSprayBottle()
    {

        //Turn on spray bottle
        if (spraySelected && sprayBottleSpriteRenderer.enabled == false)
        {
            sprayBottleSpriteRenderer.enabled = true;
        }

        else if (spraySelected && sprayBottleSpriteRenderer.enabled == true)
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var mouseXPos = mousePosition.x;
            var mouseYPos = mousePosition.y;

            sprayBottleObject.transform.position = new Vector3(mouseXPos, mouseYPos, 0);
        }
        else if (!spraySelected && sprayBottleSpriteRenderer.enabled == true)
        {
            sprayBottleSpriteRenderer.enabled = false;
        }

    }

    private void UpdateAndSpawnSlugs()
    {
        if(currentStageIndex >= 2 && currentStageIndex != 4) //Stage 3 +
        {
            slugTimer += Time.deltaTime;
            //Spawn slug after timer
            if(slugTimer >= slugSpawnTimer)
            {
                //spawn Slug
                GameObject slug = Instantiate(slugPrefab, slugParentObject.transform);
                int randomSpawnPosition = UnityEngine.Random.Range(0, slugSpawnerLocation.Length);
                slug.transform.position = slugSpawnerLocation[randomSpawnPosition].transform.position;
                if(slug.transform.position.x > 0)
                {
                    slug.GetComponent<SpriteRenderer>().flipX = true;
                    foreach (Transform child in slug.transform)
                    {
                        child.gameObject.GetComponent<SpriteRenderer>().flipX = true;
                        var childPosition = child.gameObject.transform.localPosition;
                        child.gameObject.transform.localPosition = new Vector3(childPosition.x * -1f, childPosition.y, childPosition.z);
                    }
                        
                }
                slugTimer = 0;
            }
        }
        if (currentStageIndex == 4 && slugParentObject.transform.childCount > 0) // stage 5
        {
            foreach(Transform slug in slugParentObject.transform)
            {
                Destroy(slug.gameObject);
            }
        }
    }



    private void UpdateSlugSFX()
    {
        if (slugParentObject.transform.childCount > 0 && !slugParentAudioSource.isPlaying)
        {
            Debug.Log("Playing Slug Muddy Sound!");
            slugParentAudioSource.Play();
        }
        else if (slugParentObject.transform.childCount == 0 && slugParentAudioSource.isPlaying)
        { 
            slugParentAudioSource.Stop();           
        }
    }

    private void UpdateStage()
    {

        if (timeAsHappy >= stageFourTime && currentStageIndex < 4)
        {
            //Activate stage 5
            TurnOnStage(4);
            Debug.Log("Activate Stage Five");
            //Play timeline
            MusicPlayer.Instance.StopMusic();
            timelineDirector.enabled = true;
            timelineDirector.Play();
        }
        else if (timeAsHappy < stageFourTime && timeAsHappy >= stageThreeTime && currentStageIndex < 3)
        {
            //Activate stage 4
            TurnOnStage(3);
            Debug.Log("Activate Stage Four");
        }
        else if (timeAsHappy < stageThreeTime && timeAsHappy >= stageTwoTime && currentStageIndex < 2)
        {
            //Activate Stage 3
            TurnOnStage(2);
            sprayBottleButton.gameObject.SetActive(true);
            Debug.Log("Activate Stage Three");
        }
        else if (timeAsHappy >= stageOneTime && currentStageIndex < 1)
        {
            //Activate Stage two
            TurnOnStage(1);
            tempertureSlider.gameObject.SetActive(true);
            tempertureOn = true;
            Debug.Log("Activate Stage two");
        }
            
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
    private void UpdateTempertureBool()
    {
        if (tempertureOn)
        {
            //Decrease Happiness if too low or too high
            if (tempertureLevel < lowTemperture || tempertureLevel > highTemperture)
            {
                tempertureLowOrHigh = true;
            }
            else
            {
                tempertureLowOrHigh = false;
            }
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

        if (nutrientLevelLow == true || waterLevelLow == true || sunLevelLowOrHigh == true || tempertureLowOrHigh)
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
        //Set limits
        if(nutrientLevel > maxNutrient) { nutrientLevel = maxNutrient; }
        if (nutrientLevel < 0) { nutrientLevel = 0; }
        //Adjust levels
        nutrientLevel -= nutrientDescreaseInterval * Time.deltaTime;

    }

    private void AdjustWaterLevels()
    {
        //Set limits
        if (waterLevel > maxWater) { waterLevel = maxWater; }
        if (waterLevel < 0) { waterLevel = 0; }
        //Adjust levels
        waterLevel -= waterDecreaseInterval * Time.deltaTime;
    }

    private void AdjustTempertureLevels()
    {
        //Set limits
        if (tempertureLevel > maxTemperture) { tempertureLevel = maxTemperture; }
        if (tempertureLevel < minTemperture) { tempertureLevel = minTemperture; }
        //Adjust levels
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

    private void UpdateTempertureSlider()
    {
        if (tempertureOn)
        {
            if (sunOn)
            {
                tempertureSlider.value += tempertureStepInterval * Time.deltaTime;
                
            }
            else
            {
                tempertureSlider.value -= tempertureStepInterval * Time.deltaTime;
            }
        }

        
        tempertureLevel = tempertureSlider.value;
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
        if(!sunOn)
        {
            if(sunLevel < lowSunLevel)
            {
                sunLevel = lowSunLevel;
            }
            shadeButton.color = sunBtnBGColor;
            sunButton.color = Color.grey;

            sunOn = true;

            
            PlaySFX(switchingSound);
        }
        spraySelected = false;


    }

    public void TurnOffSun()
    {
        if(sunOn)
        {
            if (sunLevel > highSunLevel)
            {
                sunLevel = highSunLevel;
            }
            shadeButton.color = Color.grey;
            sunButton.color = sunBtnBGColor;
            sunOn = false;

            
            PlaySFX(switchingSound);
        }
        spraySelected = false;

    }

    public void AddWater()
    {
        if(waterLevel < highWaterLevel && waterBtnTimer >= disableWaterBtnTime )
        {
            Instantiate(rainParticle);
            waterLevel += waterIncreaseStepInterval;
            waterBtnTimer = 0;
            PlaySFX(wateringSound);
        }

        spraySelected = false;
        
        
    }

    public void AddNutrients()
    {
        if(nutrientLevel < highNutrientLevel && nutrientBtnTimer >= disableNutrientBtnTime)
        {
            Instantiate(nutrientParticle);
            nutrientLevel += nutrientIncreaseStepInterval;
            nutrientBtnTimer = 0;
            PlaySFX(nutrientsSound);
        }

        spraySelected = false;
        
    }

    private void PlaySFX(AudioClip clip)
    {
        if(clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        
    }

    public void SelectSpray()
    {
        if(spraySelected == false)
        {
            spraySelected = true;
            PlaySFX(sprayBottleSound);
        }
        

    }

    public float GetSlugSpeed()
    {
        return slugMovementSpeed;
    }


    public float GetSlugStopDistance()
    {
        return slugStopDistance;
    }
    
    public bool GetSpraySelected()
    {
        return spraySelected;
    }

    public void SetSpraySelectedFalse()
    {
        spraySelected = false;
    }

    public float GetSlugDieTimer()
    {
        return slugDieTimer;
    }

    public void ToggleTime()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        
    }





}
