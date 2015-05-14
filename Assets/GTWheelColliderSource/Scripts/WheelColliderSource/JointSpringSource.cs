/******************************************
 * JointSpringSource
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
using UnityEngine;
using System.Collections;

public struct JointSpringSource
{
    private float m_spring; //The spring forces used to reach the target position
    private float m_damper; //The damper force uses to dampen the spring
    private float m_targetPosition; //The target position the joint attempts to reach.


    public float Spring
    {
        get
        {
            return m_spring;
        }
        set
        {
            m_spring = Mathf.Max(0, value);
        }
    }
    public float Damper
    {
        get
        {
            return m_damper;
        }
        set
        {
            m_damper = Mathf.Max(0, value);
        }
    }
    public float TargetPosition
    {
        get
        {
            return m_targetPosition;
        }
        set
        {
            m_targetPosition = Mathf.Clamp01(value);
        }
    }
}
