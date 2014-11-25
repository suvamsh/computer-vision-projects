using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZigEngageSingleUser : MonoBehaviour {
    public bool SkeletonTracked = true;
    public bool RaiseHand;

	public bool WaveHand;

	public bool StartOnSteady = false;
	public bool StartOnWave = true;
	public bool RotateToUser = true;

	public bool ZoomHand = true;

	public List<GameObject> listeners = new List<GameObject>();

	public List<GameObject> EngagedUsers;
	
	Dictionary<int, GameObject> objects = new Dictionary<int, GameObject>();

	//bounds in mm
	public Vector3 SessionBoundsOffset = new Vector3(0, 250, -300);
	public Vector3 SessionBounds = new Vector3(1500, 700, 1000);


    public ZigTrackedUser engagedTrackedUser { get; private set; }

    void Start() {
        // make sure we get zig events
        ZigInput.Instance.AddListener(gameObject);
    }

	bool EngageUser(ZigTrackedUser user) {
		if (null == engagedTrackedUser) {
            engagedTrackedUser = user;
            foreach (GameObject go in EngagedUsers) user.AddListener(go);
            SendMessage("UserEngaged", this, SendMessageOptions.DontRequireReceiver);
			return true;
		}

		return false;
	}
	
	bool DisengageUser(ZigTrackedUser user)	{
        if (user == engagedTrackedUser) {
            foreach (GameObject go in EngagedUsers) user.RemoveListener(go);
            engagedTrackedUser = null;
            SendMessage("UserDisengaged", this, SendMessageOptions.DontRequireReceiver);

			return true;
        }

		return false;
	}
	
	void Zig_UserFound(ZigTrackedUser user) {
        // create gameobject to listen for events for this user
        GameObject go = new GameObject("WaitForEngagement" + user.Id);
        go.transform.parent = transform;
		objects[user.Id] = go;

        // add various detectors & events

        if (RaiseHand) {
            ZigHandRaiseDetector hrd = go.AddComponent<ZigHandRaiseDetector>();
            hrd.Zoom += delegate {
                EngageUser(user);
            };
        }

		if (ZoomHand) {
			Debug.Log("Created Zoom Hand Detector");
			ZigHandZoomDetector hrd = go.AddComponent<ZigHandZoomDetector>();
			hrd.Zoom += delegate {
				EngageUser(user);
			};
		}
		
		if (WaveHand) {
			ZigHandSessionDetector hsd = go.AddComponent<ZigHandSessionDetector>();        
			
			hsd.SessionBounds = SessionBounds;
			hsd.SessionBoundsOffset = SessionBoundsOffset;
			hsd.StartOnSteady = StartOnSteady;
			hsd.StartOnWave = StartOnWave;
			hsd.RotateToUser = RotateToUser;    
			
			hsd.SessionStart += delegate {
				Debug.Log("EngageSingleSession: Session start");
				if (EngageUser(user)) {
					foreach (GameObject listener in listeners) {
						Debug.Log(listener.ToString());
						hsd.AddListener(listener);
					}
				}
			};
			hsd.SessionEnd += delegate {
				Debug.Log("EngageSingleSession: Session end");
				if (DisengageUser(user)) {
					foreach (GameObject listener in listeners) {
						hsd.RemoveListener(listener);
					}
				}
			};

		}		

        // attach the new object to the new user
		user.AddListener(go);
	}
	
	void Zig_UserLost(ZigTrackedUser user) {
        DisengageUser(user);
		Destroy(objects[user.Id]);
		objects.Remove(user.Id);
	}

    void Zig_Update(ZigInput zig) {
        if (SkeletonTracked && null == engagedTrackedUser) {
            foreach (ZigTrackedUser trackedUser in zig.TrackedUsers.Values) {
                if (trackedUser.SkeletonTracked) {
                    EngageUser(trackedUser);
                }
            }
        }
    }

    public void Reset() {
        if (null != engagedTrackedUser) {
            DisengageUser(engagedTrackedUser);
        }
    }
}
