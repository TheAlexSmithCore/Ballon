using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCostPooling : Pool
{
    #region Singleton
    public static ScoreCostPooling instance;
    #endregion

    private void Awake()
    {
        instance = this;
    }
}
