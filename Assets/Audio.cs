using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public static Audio instance;

    public AudioClip[] clip;
    List<AudioSource> Suara = new List<AudioSource> ();

    private void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Suara.Clear();
        for (int i = 0; i < clip.Length; i++)
        {
            Suara.Add(gameObject.AddComponent<AudioSource>());
            Suara[i].clip = clip[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PanggilSuara(int i)
    {
        Suara[i].Play();
    }

    public void PutarBGM()
    {
        Suara[2].loop = true;
        Suara[2].Play();
    }
}
