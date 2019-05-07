using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatePattern;

public class LandStarBehaviour_Test : GenericStateContex<LandStarController>
{
    public class StateName
    {
        public static readonly string Neutral = "Neutral";
        public static readonly string Moving = "Moving";
        public static readonly string Stucked = "Stucked";
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}