/******************************************
 * CarController
 *  
 * This class was created by:
 * 
 * Nic Tolentino.
 * rotatingcube@gmail.com
 * 
 * I take no liability for it's use and is provided as is.
 * 
 * The classes are based off the original code developed by Unity Technologies.
 * 
 * You are free to use, modify or distribute these classes however you wish, 
 * I only ask that you make mention of their use in your project credits.
*/

// Here's the basic car script described in my tutorial at www.gotow.net/andrew/blog.
// A Complete explaination of how this script works can be found at the link above, along
// with detailed instructions on how to write one of your own, and tips on what values to 
// assign to the script variables for it to work well for your application.

// Contact me at Maxwelldoggums@Gmail.com for more information.

// These variables allow the script to power the wheels of the car.

using UnityEngine;
using System.Collections;

public class PlayerCarController : MonoBehaviour
{
   // public Transform FrontRight;
   // public Transform FrontLeft;
   // public Transform BackRight;
   // public Transform BackLeft;

	public float EngineTorque = 600.0f;
	public float MaxEngineRPM = 3000.0f;
	public float MinEngineRPM = 1000.0f;
	private float EngineRPM = 0.0f;

	public float[] GearRatio;
	public int CurrentGear = 0;
	
	public WheelColliderSource FrontRightWheel;
    public WheelColliderSource FrontLeftWheel;
    public WheelColliderSource BackRightWheel;
    public WheelColliderSource BackLeftWheel;

	private Rigidbody rigidBody;
	private AudioSource audioSource;

    public void Awake()
    {

		rigidBody = GetComponent<Rigidbody> ();
		rigidBody.centerOfMass = new Vector3 (0f, -1f, 0f);

		audioSource = GetComponent<AudioSource> ();

		WheelColliderSource[] wheels = {FrontLeftWheel, FrontRightWheel, BackLeftWheel, BackRightWheel};
	
		foreach (WheelColliderSource wheel in wheels)
		{
			JointSpringSource spring = wheel.SuspensionSpring;

			spring.Spring = 15000f;
			spring.Damper = 2000f;
			wheel.SuspensionSpring = spring;

			wheel.SuspensionDistance = .1f;
			wheel.WheelRadius = .45f;
			wheel.Mass = 1.0f;

/*
			wheel.SidewaysFriction = new WheelFrictionCurveSource();
			wheel.SidewaysFriction.ExtremumSlip = 1f;
			wheel.SidewaysFriction.ExtremumValue = 20000f;
			wheel.SidewaysFriction.AsymptoteSlip = 2f;
			wheel.SidewaysFriction.AsymptoteValue = 10000f;
			wheel.SidewaysFriction.Stiffness = 0.022f;

			wheel.ForwardFriction = new WheelFrictionCurveSource();
			wheel.ForwardFriction.ExtremumSlip = 1f;
			wheel.ForwardFriction.ExtremumValue = 20000f;
			wheel.ForwardFriction.AsymptoteSlip = 2f;
			wheel.ForwardFriction.AsymptoteValue = 10000f;
			wheel.ForwardFriction.Stiffness = 0.092f;
*/
		}


    }

    public void Update()
    {


		rigidBody.drag = rigidBody.velocity.magnitude / 250;
		
		EngineRPM = (FrontLeftWheel.RPM + FrontRightWheel.RPM)/2 * GearRatio[CurrentGear];
		ShiftGears();

		audioSource.pitch = Mathf.Abs(EngineRPM / MaxEngineRPM) + 1.0f ;

		if ( audioSource.pitch > 2.0f ) {
			audioSource.pitch = 2.0f;
		}


        //Turn the steering wheel
        FrontRightWheel.SteerAngle = Input.GetAxis("Horizontal") * 10;
        FrontLeftWheel.SteerAngle = Input.GetAxis("Horizontal") * 10;

		FrontLeftWheel.MotorTorque = EngineTorque / GearRatio[CurrentGear] * Input.GetAxis("Vertical");
		FrontRightWheel.MotorTorque = EngineTorque / GearRatio[CurrentGear] * Input.GetAxis("Vertical");

        //Apply the hand brake
        if (Input.GetKey(KeyCode.Space))
        {
            BackRightWheel.BrakeTorque = 200000.0f;
            BackLeftWheel.BrakeTorque = 200000.0f;
        }
        else //Remove handbrake
        {
            BackRightWheel.BrakeTorque = 0;
            BackLeftWheel.BrakeTorque = 0;
        }
    }

	void  ShiftGears (){
		// this funciton shifts the gears of the vehcile, it loops through all the gears, checking which will make
		// the engine RPM fall within the desired range. The gear is then set to this "appropriate" value.
		int AppropriateGear = CurrentGear;
		
		if ( EngineRPM >= MaxEngineRPM ) {
			
			for ( int i= 0; i < GearRatio.Length; i ++ ) {
				if ( FrontLeftWheel.RPM * GearRatio[i] < MaxEngineRPM ) {
					AppropriateGear = i;
					break;
				}
			}
			
			CurrentGear = AppropriateGear;
		}
		
		if ( EngineRPM <= MinEngineRPM ) {
			AppropriateGear = CurrentGear;
			
			for ( int j= GearRatio.Length-1; j >= 0; j -- ) {
				if ( FrontLeftWheel.RPM * GearRatio[j] > MinEngineRPM ) {
					AppropriateGear = j;
					break;
				}
			}
			
			CurrentGear = AppropriateGear;
		}
	}
}
