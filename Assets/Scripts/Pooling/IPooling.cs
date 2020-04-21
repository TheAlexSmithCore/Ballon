using System.Collections.Generic;
using UnityEngine;

public interface IPooling
{
    void InitializeObjectsToPool();
    void DisableAllPoolingObjects();
    GameObject GetPooledObject();
}
