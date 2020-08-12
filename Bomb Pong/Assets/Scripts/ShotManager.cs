using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Shot
{
    public float upForce;
    public float hitForce;

}

public class ShotManager : MonoBehaviour
{
    public Shot rightHit; //ball will go to the left
    public Shot leftHit; //ball will go to the right
}

