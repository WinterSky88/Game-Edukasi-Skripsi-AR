using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoShared;

public class ARLocationGoMapLocationManager : LocationManager
{
    public void ChangeLocation(Coordinates coords)
    {
	onLocationChanged?.Invoke(coords);
    }
}
