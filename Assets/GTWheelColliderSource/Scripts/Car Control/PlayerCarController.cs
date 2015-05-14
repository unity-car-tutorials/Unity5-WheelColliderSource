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
using UnityEngine.UI;
using System.Collections;

public class PlayerCarController : MonoBehaviour
{
 
 	public Text debugText;
 
	public float EngineTorque = 800f;
	public float MaxEngineRPM = 14000.0f;
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
	
		for (int i = 0; i < wheels.Length; i++)
		{
			WheelColliderSource wheel = wheels[i];
			JointSpringSource spring = wheel.SuspensionSpring;

			spring.Spring = 50000;
			spring.Damper = 10000;
			wheel.SuspensionSpring = spring;

			wheel.SuspensionDistance = .1f;
			wheel.WheelRadius = .4f;
			wheel.Mass = 1;
			
			wheel.ForwardFriction = new WheelFrictionCurveSource();
			wheel.ForwardFriction.ExtremumSlip = 3f;
			wheel.ForwardFriction.ExtremumValue = 6000f;
			wheel.ForwardFriction.AsymptoteSlip = 4f;
			wheel.ForwardFriction.AsymptoteValue = 6000f;
			wheel.ForwardFriction.Stiffness = 3;
			
			wheel.SidewaysFriction = new WheelFrictionCurveSource();
			wheel.SidewaysFriction.ExtremumSlip = 3f;
			wheel.SidewaysFriction.ExtremumValue = 4000f;
			wheel.SidewaysFriction.AsymptoteSlip = 4f;
			wheel.SidewaysFriction.AsymptoteValue = 4000f;
			wheel.SidewaysFriction.Stiffness = 5;			
		}


    }

    public void Update()
    {
		WheelColliderSource[] wheels = {FrontLeftWheel, FrontRightWheel, BackLeftWheel, BackRightWheel};
		
		if (debugText)
		{
			debugText.text =	" FL: " + wheels[0].RPM.ToString("0000") + " FR: " + wheels[1].RPM.ToString("0000") +
								" RL: " + wheels[2].RPM.ToString("0000") + " FR: " + wheels[3].RPM.ToString("0000") +
								" Engine RPM : " + EngineRPM.ToString("00000") + " Speed: " + (rigidBody.velocity.magnitude * 2.2f).ToString("000");
	
		}
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
