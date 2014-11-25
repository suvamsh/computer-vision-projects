using UnityEngine;
using System.Collections;
using System;

public class HandZoomEventArgs : EventArgs
{
    public ZigJointId Joint { get; private set; }
    public HandZoomEventArgs(ZigJointId joint)
    {
        Joint = joint;
    }
}


public class ZigHandZoomDetector : MonoBehaviour {
	ZigSteadyDetector leftHandSteady;
	ZigSteadyDetector rightHandSteady;
    GameObject leftHandDetector;
    GameObject rightHandDetector;
	ZigTrackedUser trackedUser;
    public float angleThreshold = 30; // degrees

	bool checkRightHandZoom, checkRightFirst;
	bool checkLeftHandZoom, checkLeftFirst;
	static bool leftHandDetected = false;
	static bool rightHandDetected = false;

	double rightHandX, leftHandX, rightShoulderX, leftShoulderX = 0;

	ZigInputJoint leftHand, rightHand;
	ZigInputJoint leftShoulder, rightShoulder;

    public event EventHandler<HandZoomEventArgs> Zoom;
    protected void OnHandZoom(ZigJointId joint)
    {
       if (null != Zoom) {
            Zoom.Invoke(this, new HandZoomEventArgs(joint));
        }
       SendMessage("HandZoomDetector_HandZoom", joint, SendMessageOptions.DontRequireReceiver);
    }

    // Use this for initialization
	void Awake () {
        leftHandDetector = new GameObject("LeftHandDetector");
        leftHandDetector.transform.parent = gameObject.transform;
        ZigMapJointToSession leftMap = leftHandDetector.AddComponent<ZigMapJointToSession>();
        leftMap.joint = ZigJointId.LeftHand;

        rightHandDetector = new GameObject("RightHandDetector");
        rightHandDetector.transform.parent = gameObject.transform;
        ZigMapJointToSession rightMap = rightHandDetector.AddComponent<ZigMapJointToSession>();
        rightMap.joint = ZigJointId.RightHand;

		leftHandSteady = leftHandDetector.AddComponent<ZigSteadyDetector>();
		rightHandSteady = rightHandDetector.AddComponent<ZigSteadyDetector>();

		leftHandSteady.Steady += delegate(object sender, EventArgs e) {
			leftHandDetected = true;
			leftHand = trackedUser.Skeleton[(int)ZigJointId.LeftHand];
			leftShoulder = trackedUser.Skeleton[(int)ZigJointId.LeftShoulder];
			Left_Hand_Detect(leftHand.Position, leftShoulder.Position);
		};

        rightHandSteady.Steady += delegate(object sender, EventArgs e) {
			rightHandDetected = true;
			rightHand = trackedUser.Skeleton[(int)ZigJointId.RightHand];
			rightShoulder = trackedUser.Skeleton[(int)ZigJointId.RightShoulder];
			Right_Hand_Detect(rightHand.Position, rightShoulder.Position);
        }; 

		//Check_Hands ();

		/*if (leftHandDetected && rightHandDetected) {
			leftHandDetected = false;
			rightHandDetected = false;

			if (IsHandZoom(leftHand.Position, leftShoulder.Position, 
			               rightHand.Position, rightShoulder.Position)) {
				Debug.Log ("Detected Zoom hands");

			} else {
				Debug.Log ("Not Detected Zoom Hands");
				
			}

		} else {
			Debug.Log ("Both false");
		}*/
	}
	
    void Zig_Attach(ZigTrackedUser user)
    {
        trackedUser = user;
        user.AddListener(leftHandDetector);
        user.AddListener(rightHandDetector);
		Debug.Log ("Added all the listeners");
    }

    void Zig_UpdateUser(ZigTrackedUser user)
    {
        trackedUser = user;
    }

    void Zig_Detach(ZigTrackedUser user)
    {
        user.RemoveListener(leftHandDetector);
        user.RemoveListener(rightHandDetector);
		Debug.Log ("Removed all the listeners");
        trackedUser = null;
    }

    void Left_Hand_Detect(Vector3 lHandPos, Vector3 lShoulderPos)
    {
		this.leftHandX = lHandPos.x;
		leftShoulderX = lShoulderPos.x;
		Debug.Log ("Left Hand X: ");
		Debug.Log (lHandPos.x);
    }

	void Right_Hand_Detect(Vector3 rHandPos, Vector3 rShoulderPos)
	{
		this.rightHandX = rHandPos.x;
		rightShoulderX = rShoulderPos.x;
	}
	
	void Check_Hands() {
		double handX = Math.Abs (rightHandX - leftHandX);
		Debug.Log (handX);
	}
}
