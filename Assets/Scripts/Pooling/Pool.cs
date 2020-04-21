using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour, IPooling
{
    public List<GameObject> Items;
    [SerializeField] private GameObject m_PrefabToPool;
    [SerializeField] private int m_AmountToPool = 10;

    #region Pooling Initialization
    private void Start() {
        InitializeObjectsToPool();
    }

    public void InitializeObjectsToPool()
    {
        Items = new List<GameObject>();
        for (int i = 0; i < m_AmountToPool; i++)
        {
            GameObject item = (GameObject)Instantiate(m_PrefabToPool);
            item.transform.SetParent(this.transform);
            item.SetActive(false);
            Items.Add(item);
        }
    }
    #endregion

    public void DisableAllPoolingObjects()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            Items[i].SetActive(false);
        }
    }

    public GameObject GetPooledObject() {
        for (int i = 0; i < Items.Count; i++)
        {
            if (!Items[i].activeInHierarchy)
            {
                return Items[i];
            }
        }
        return null;
    }


}
