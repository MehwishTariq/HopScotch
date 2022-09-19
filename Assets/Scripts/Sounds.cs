using UnityEngine;

[System.Serializable]
public class Sounds
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)] public float volum = 0.5f;

    [Range(-3f, 3f)] public float pitch;

    public bool loop = false;
}