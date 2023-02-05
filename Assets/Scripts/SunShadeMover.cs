using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunShadeMover : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject spriteObject;
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite sunSprite;
    [SerializeField] Sprite shadeSprite;
    Vector3 originalLocation;
    [SerializeField] Transform targetLocation;
    [SerializeField] float speed;
    [SerializeField] float waitTimeForSwitch;


    private void Start()
    {
        spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
        originalLocation = spriteObject.transform.position;
    }

    public void StartSunUp()
    {
        StartCoroutine(SunUp());
    }

    public void StartShadeUp()
    {
        StartCoroutine(ShadeUp());
    }

    public IEnumerator SunUp()
    {
        float elapsedTime = 0;
        while (elapsedTime < waitTimeForSwitch)
        {
            spriteObject.transform.position = Vector3.MoveTowards(spriteObject.transform.position, targetLocation.position, (elapsedTime / waitTimeForSwitch));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(ShadeDown());
    }

    public IEnumerator ShadeDown()
    {        
        spriteRenderer.sprite = shadeSprite;
        float elapsedTime = 0;
        Debug.Log(originalLocation);
        while (elapsedTime < waitTimeForSwitch)
        {
            
            spriteObject.transform.position = Vector3.MoveTowards(spriteObject.transform.position, originalLocation, (elapsedTime / waitTimeForSwitch));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator ShadeUp()
    {
        float elapsedTime = 0;
        while (elapsedTime < waitTimeForSwitch)
        {
            spriteObject.transform.position = Vector3.MoveTowards(spriteObject.transform.position, targetLocation.position, (elapsedTime / waitTimeForSwitch));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(SunDown());
    }

    public IEnumerator SunDown()
    {
        spriteRenderer.sprite = sunSprite;
        float elapsedTime = 0;
        while (elapsedTime < waitTimeForSwitch)
        {
            spriteObject.transform.position = Vector3.MoveTowards(spriteObject.transform.position, originalLocation, (elapsedTime / waitTimeForSwitch));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }



}
