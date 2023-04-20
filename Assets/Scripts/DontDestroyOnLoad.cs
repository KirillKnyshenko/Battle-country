using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private void Start()
    {
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }
}
