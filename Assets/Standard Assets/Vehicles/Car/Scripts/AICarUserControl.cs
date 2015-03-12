using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class AICarUserControl : MonoBehaviour
    {

		public GameObject waypointContainer;
		private List<Transform> waypoints;
		public int currentWaypoint = 0;

		private float inputSteer = 0.0f;
		private float inputTorque = 0.0f;

        private CarController m_Car; // the car controller we want to use

        private void Start()
        {
            // get the car controller
			GetWaypoints ();
			m_Car = GetComponent<CarController>();
        }

		void  GetWaypoints (){
			
			Transform[] potentialWaypoints = waypointContainer.GetComponentsInChildren< Transform >();
			waypoints = new List<Transform> ();
			
			foreach( Transform potentialWaypoint in potentialWaypoints ) {
				if ( potentialWaypoint != waypointContainer.transform ) {
					waypoints.Add (potentialWaypoint);
					
				}
			}
		}


        private void Update()
        {

			NavigateTowardsWaypoint ();
			m_Car.Move (inputSteer, inputTorque, 0, 0f);
			/*
            // pass the input to the car!
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("Jump");
            m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
			*/
        }


		
		void  NavigateTowardsWaypoint (){
			
			Vector3 RelativeWaypointPosition = transform.InverseTransformPoint( new Vector3( 
			                                                                                waypoints[currentWaypoint].position.x, 
			                                                                                transform.position.y, 
			                                                                                waypoints[currentWaypoint].position.z ) );
			
			inputSteer = RelativeWaypointPosition.x / RelativeWaypointPosition.magnitude;
			
			if ( Mathf.Abs( inputSteer ) < 0.5f ) {
				inputTorque = RelativeWaypointPosition.z / RelativeWaypointPosition.magnitude - Mathf.Abs( inputSteer );
			}else{
				inputTorque = 0.0f;
			}
			
			if ( RelativeWaypointPosition.magnitude < 20 ) {
				currentWaypoint ++;
				
				if ( currentWaypoint >= waypoints.Count ) {
					currentWaypoint = 0;
				}
			}
			
		}
    }




}
