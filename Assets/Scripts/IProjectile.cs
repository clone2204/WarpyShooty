using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile 
{
    void Init(GamePlayer owner, Vector3 direction, float velocity, int damage);
}
