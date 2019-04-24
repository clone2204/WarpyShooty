using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile 
{
    void Init(RealPlayer owner, Vector3 direction, float velocity, int damage);
}
