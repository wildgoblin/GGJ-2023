using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class Slug : MonoBehaviour
{
    GameObject player;
    GameController gc;
    [SerializeField] Sprite faceSad;
    AudioSource audioSource;

    float offset;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gc = GameController.Instance;
        audioSource = GetComponent<AudioSource>();
        offset = gc.GetSlugStopDistance();
    }
    private void Update()
    {
        MoveSlug();
        if(Input.GetMouseButtonDown(0) )
        {
            Vector2 cubeRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D cubeHit = Physics2D.Raycast(cubeRay, Vector2.zero);

            if (cubeHit)
            {
                //check spray
                if (gc.GetSpraySelected())
                {
                    //Hurt me!
                    cubeHit.collider.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = faceSad;
                    audioSource.Play();
                    StartCoroutine(WaitThenDie(cubeHit.collider));
                    gc.SetSpraySelectedFalse();
                }
            }

        }
    }

    IEnumerator WaitThenDie(Collider2D collider2D)
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(collider2D.gameObject);
    }

    private void MoveSlug()
    {
        if (transform.position.x > player.transform.position.x)
        {
            offset = -offset;
        }

        var playerPos = new Vector3(player.transform.position.x - offset, player.transform.position.y + 0.2f, player.transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, playerPos, gc.GetSlugSpeed() * Time.deltaTime);
    }


}
