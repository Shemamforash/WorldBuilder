using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSubstance
{
	//Inherit from this to implement different tile substances

	/*Flow is how quickly the substance moves to higher or lower ground.
	A negative flow value should make the substance move uphill (useful for clouds).
	Flow should have a value between -1 and 1, where higher magnitude values cause substances to flow to new tiles at a faster rate.

	Abrasiveness affects how quickly the substance erodes other substances.
	When a substance is eroded it should not be removed, rather it should flow according to the sign of it's flow value.
	Abrasiveness should have a value from 0 to 1, where 1 causes instant erosion, and 0 causes no erosion.

	Permanance is the opposite of abrasiveness, it indicates the resistance to abrasion.
	Permanance should have a value from 0 to 1, where 1 resists all abrasion.
	*/
    private float _flow, _abrasiveness, _permanance;
    private Material mat;

    public void Flow() { }
}
