using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip bombExplosion, paddleHit, tableHit;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        bombExplosion = Resources.Load<AudioClip>("bomb_explosion");
        paddleHit = Resources.Load<AudioClip>("paddle_hit");
        tableHit = Resources.Load<AudioClip>("table_hit");

        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(string clip)
    {
        Debug.Log("Playing some sound");
        switch (clip)
        {            
            case "bomb_explosion":
                audioSrc.PlayOneShot(bombExplosion);
                break;
            case "paddle_hit":
                audioSrc.PlayOneShot(paddleHit);
                break;
            case "table_hit":
                audioSrc.PlayOneShot(tableHit);
                break;
        }

    }
}
