using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Colors
{
    public Vector3 Body;
    public Vector3 Eyes;
    public Vector3 Hair;
    public Vector3 Beard;
    public Vector3 Shirt;
    public Vector3 Pants;
}

[Serializable]
public class SelectedSprites
{
    public int Shirt;
    public int Pants;
    public int Hair;
    public int Beard;
}

[Serializable]
public class SaveObject
{
    public Colors Colors;
    public int Score;
    public int Coins;
    public SelectedSprites SelectedSprites;
}