using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ZigSkeleton : MonoBehaviour
{
    public Transform Head;
    public Transform Neck;
    public Transform Torso;
    public Transform Waist;

    public Transform LeftCollar;
    public Transform LeftShoulder;
    public Transform LeftElbow;
    public Transform LeftWrist;
    public Transform LeftHand;
    public Transform LeftFingertip;

    public Transform RightCollar;
    public Transform RightShoulder;
    public Transform RightElbow;
    public Transform RightWrist;
    public Transform RightHand;
    public Transform RightFingertip;

    public Transform LeftHip;
    public Transform LeftKnee;
    public Transform LeftAnkle;
    public Transform LeftFoot;

    public Transform RightHip;
    public Transform RightKnee;
    public Transform RightAnkle;
    public Transform RightFoot;
    public bool mirror = false;
    public bool UpdateJointPositions = false;
    public bool UpdateRootPosition = false;
    public bool UpdateOrientation = true;
    public bool RotateToPsiPose = false;
    public float RotationDamping = 30.0f;
    public float Damping = 30.0f;
    public Vector3 Scale = new Vector3(0.001f, 0.001f, 0.001f);

    public Vector3 PositionBias = Vector3.zero;

    private Transform[] transforms;
    private Quaternion[] initialRotations;
    private Vector3 rootPosition;

	// Zoom gesture
	private Vector3 leftHand, rightHand, torso, leftShoulder, rightShoulder;
	private double headY = 0.0, rightElbowX, prevDis;
	private bool zoomTag = false;

	// Processing speed
	private int frameCounter = 0;
	public int processSpeed = 250;
	GameObject globe;
	ArrowKeyMovement key;


    ZigJointId mirrorJoint(ZigJointId joint)
    {
        switch (joint)
        {
            case ZigJointId.LeftCollar:
                return ZigJointId.RightCollar;
            case ZigJointId.LeftShoulder:
                return ZigJointId.RightShoulder;
            case ZigJointId.LeftElbow:
                return ZigJointId.RightElbow;
            case ZigJointId.LeftWrist:
                return ZigJointId.RightWrist;
            case ZigJointId.LeftHand:
                return ZigJointId.RightHand;
            case ZigJointId.LeftFingertip:
                return ZigJointId.RightFingertip;
            case ZigJointId.LeftHip:
                return ZigJointId.RightHip;
            case ZigJointId.LeftKnee:
                return ZigJointId.RightKnee;
            case ZigJointId.LeftAnkle:
                return ZigJointId.RightAnkle;
            case ZigJointId.LeftFoot:
                return ZigJointId.RightFoot;

            case ZigJointId.RightCollar:
                return ZigJointId.LeftCollar;
            case ZigJointId.RightShoulder:
                return ZigJointId.LeftShoulder;
            case ZigJointId.RightElbow:
                return ZigJointId.LeftElbow;
            case ZigJointId.RightWrist:
                return ZigJointId.LeftWrist;
            case ZigJointId.RightHand:
                return ZigJointId.LeftHand;
            case ZigJointId.RightFingertip:
                return ZigJointId.LeftFingertip;
            case ZigJointId.RightHip:
                return ZigJointId.LeftHip;
            case ZigJointId.RightKnee:
                return ZigJointId.LeftKnee;
            case ZigJointId.RightAnkle:
                return ZigJointId.LeftAnkle;
            case ZigJointId.RightFoot:
                return ZigJointId.LeftFoot;


            default:
                return joint;
        }
    }


    public void Awake()
    {
		globe = GameObject.Find ("Globe");
		key = globe.GetComponent<ArrowKeyMovement>();
        int jointCount = Enum.GetNames(typeof(ZigJointId)).Length;

        transforms = new Transform[jointCount];
        initialRotations = new Quaternion[jointCount];

        transforms[(int)ZigJointId.Head] = Head;
        transforms[(int)ZigJointId.Neck] = Neck;
        transforms[(int)ZigJointId.Torso] = Torso;
        transforms[(int)ZigJointId.Waist] = Waist;
        transforms[(int)ZigJointId.LeftCollar] = LeftCollar;
        transforms[(int)ZigJointId.LeftShoulder] = LeftShoulder;
        transforms[(int)ZigJointId.LeftElbow] = LeftElbow;
        transforms[(int)ZigJointId.LeftWrist] = LeftWrist;
        transforms[(int)ZigJointId.LeftHand] = LeftHand;
        transforms[(int)ZigJointId.LeftFingertip] = LeftFingertip;
        transforms[(int)ZigJointId.RightCollar] = RightCollar;
        transforms[(int)ZigJointId.RightShoulder] = RightShoulder;
        transforms[(int)ZigJointId.RightElbow] = RightElbow;
        transforms[(int)ZigJointId.RightWrist] = RightWrist;
        transforms[(int)ZigJointId.RightHand] = RightHand;
        transforms[(int)ZigJointId.RightFingertip] = RightFingertip;
        transforms[(int)ZigJointId.LeftHip] = LeftHip;
        transforms[(int)ZigJointId.LeftKnee] = LeftKnee;
        transforms[(int)ZigJointId.LeftAnkle] = LeftAnkle;
        transforms[(int)ZigJointId.LeftFoot] = LeftFoot;
        transforms[(int)ZigJointId.RightHip] = RightHip;
        transforms[(int)ZigJointId.RightKnee] = RightKnee;
        transforms[(int)ZigJointId.RightAnkle] = RightAnkle;
        transforms[(int)ZigJointId.RightFoot] = RightFoot;
		

        // save all initial rotations
        // NOTE: Assumes skeleton model is in "T" pose since all rotations are relative to that pose
        foreach (ZigJointId j in Enum.GetValues(typeof(ZigJointId)))
        {
            if (transforms[(int)j])
            {
                // we will store the relative rotation of each joint from the gameobject rotation
                // we need this since we will be setting the joint's rotation (not localRotation) but we 
                // still want the rotations to be relative to our game object
                initialRotations[(int)j] = Quaternion.Inverse(transform.rotation) * transforms[(int)j].rotation;
            }
        }
    }

    void Start()
    {
        // start out in calibration pose
        if (RotateToPsiPose)
        {
            RotateToCalibrationPose();
        }
    }

    void UpdateRoot(Vector3 skelRoot)
    {
        // +Z is backwards in OpenNI coordinates, so reverse it
        rootPosition = Vector3.Scale(new Vector3(skelRoot.x, skelRoot.y, skelRoot.z), doMirror(Scale)) + PositionBias;
        if (UpdateRootPosition)
        {
            transform.localPosition = (transform.rotation * rootPosition);
        }
    }

    void UpdateRotation(ZigJointId joint, Quaternion orientation)
    {
		//Debug.Log ("Update Rotation function");
        joint = mirror ? mirrorJoint(joint) : joint;
        // make sure something is hooked up to this joint
        if (!transforms[(int)joint])
        {
            return;
        }

        if (UpdateOrientation)
        {

			//if (joint == ZigJointId.Torso) {
			//	Debug.Log ("Right hand rotation");
			//}
            Quaternion newRotation = transform.rotation * orientation * initialRotations[(int)joint];
            if (mirror)
            {
                newRotation.y = -newRotation.y;
                newRotation.z = -newRotation.z;
            }
            transforms[(int)joint].rotation = Quaternion.Slerp(transforms[(int)joint].rotation, newRotation, Time.deltaTime * RotationDamping);
        }
    }
    Vector3 doMirror(Vector3 vec)
    {
        return new Vector3(mirror ? -vec.x : vec.x, vec.y, vec.z);
    }
	double average = 0.0;
	int count = 0;
    void UpdatePosition(ZigJointId joint, Vector3 position)
    {
        joint = mirror ? mirrorJoint(joint) : joint;
        // make sure something is hooked up to this joint
        if (!transforms[(int)joint])
        {
            return;
        }

        if (UpdateJointPositions)
        {
			frameCounter++;

			if(joint == ZigJointId.RightHand) {
				rightHand = position;
				// Debug.Log(rightHand.y);
			} 
			if (joint == ZigJointId.Torso) {
				torso = position;
			}
			if(joint == ZigJointId.LeftHand) leftHand = position;
			if(joint == ZigJointId.RightShoulder) {
				rightShoulder = position;
			}
			if(joint == ZigJointId.LeftShoulder) leftShoulder = position;
			if(joint == ZigJointId.RightElbow) rightElbowX = position.x; 
			if(joint == ZigJointId.Head) headY = position.y;

			checkZoom();
			if (key.sview) {
				if (rightHand.y > headY + 50) {
					key.gmap = true;
					key.sview = false;
					key.screenshot();
				}
				//Debug.Log ("zval: " + (rightHand.z - torso.z));
				if ((rightHand.z - torso.z >= 400) && (frameCounter > processSpeed)) {
					frameCounter = 0;
					key.moveForward();
				}
				Debug.Log ("xval: " + (rightHand.x - torso.x));
				if ((leftHand.x - rightShoulder.x >= 300) && (frameCounter > processSpeed * 1.2)) {
					frameCounter = 0;
					Debug.Log ("Turning right");
					key.turnRight();
				}
				if ((rightHand.x < leftShoulder.x) && (frameCounter > processSpeed * 1.2)) {
					frameCounter = 0;
					Debug.Log ("Turning left");
					key.turnLeft();
				}
			}

            Vector3 dest = Vector3.Scale(position, doMirror(Scale)) - rootPosition;
            transforms[(int)joint].localPosition = Vector3.Lerp(transforms[(int)joint].localPosition, dest, Time.deltaTime * Damping);
        }
    }

	public void checkZoom() {
		if(rightHand.x > leftShoulder.x  && rightHand.x < rightShoulder.x 
		   && frameCounter >= processSpeed * 0.05
		   && leftHand.x > leftShoulder.x  && leftHand.x < rightShoulder.x
		   && key.gmap) {
			frameCounter = 0;
			double handDis = Math.Abs(rightHand.x - leftHand.x);
			double shoulderDis = Math.Abs(rightShoulder.x - leftShoulder.x);
			
			
			count++;
			average += handDis;
			
			if(count == 10) {
				handDis = average / count;
				count = 0;
				average = 0;
				Debug.Log (handDis/shoulderDis);
				
				key.Zoom((float)(handDis/shoulderDis));
				
			}
			
			/*if(handDis < 50){// && rightHandX < leftShoulderX && leftHandX > rightShoulderX) {
					zoomTag = true;
					prevDis = handDis;
				}

				if(zoomTag) {
					if(handDis > 0 && (handDis - prevDis) > 0.025) {
						Debug.Log (handDis/shoulderDis);

						prevDis = handDis;
					}
				}*/
			
		} 
	}

    public void RotateToCalibrationPose()
    {
        foreach (ZigJointId j in Enum.GetValues(typeof(ZigJointId)))
        {
            if (null != transforms[(int)j])
            {
                transforms[(int)j].rotation = transform.rotation * initialRotations[(int)j];
            }
        }

        // calibration pose is skeleton base pose ("T") with both elbows bent in 90 degrees
        if (null != RightElbow)
        {
            RightElbow.rotation = transform.rotation * Quaternion.Euler(0, -90, 90) * initialRotations[(int)ZigJointId.RightElbow];
        }
        if (null != LeftElbow)
        {
            LeftElbow.rotation = transform.rotation * Quaternion.Euler(0, 90, -90) * initialRotations[(int)ZigJointId.LeftElbow];
        }
    }

    public void SetRootPositionBias()
    {
        this.PositionBias = -rootPosition;
    }

    public void SetRootPositionBias(Vector3 bias)
    {
        this.PositionBias = bias;
    }

    void Zig_UpdateUser(ZigTrackedUser user)
    {
        UpdateRoot(user.Position);
        if (user.SkeletonTracked)
        {
            foreach (ZigInputJoint joint in user.Skeleton)
            {
                if (joint.GoodPosition) UpdatePosition(joint.Id, joint.Position);
                if (joint.GoodRotation) UpdateRotation(joint.Id, joint.Rotation);
            }
        }
    }

}
