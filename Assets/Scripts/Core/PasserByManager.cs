using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasserByManager : MonoBehaviour
{
    
    public void MakePassbyDog()
    {
        Factory.Instance.GetCorgi();
    }

}
