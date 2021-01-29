using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public enum Usage
    {
        Primary, Secondary, Both, Either
    }

    public Usage usage;
}
