using UnityEngine;
using System.Collections;

public class DrawWaypointGizmos_Script : MonoBehaviour {
	void  OnDrawGizmos (){
		// make a new array of waypoints, then set it to all of the transforms in the current object
		Transform[] waypoints= gameObject.GetComponentsInChildren< Transform >();
		
		// now loop through all of them and draw gizmos for each of them
		foreach( Transform waypoint in waypoints ) {
			Gizmos.DrawSphere( waypoint.position, 1.0f );
		}
		
	}
}