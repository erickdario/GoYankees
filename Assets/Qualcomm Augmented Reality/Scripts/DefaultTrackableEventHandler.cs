/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Qualcomm Connected Experiences, Inc.
==============================================================================*/

using UnityEngine;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class DefaultTrackableEventHandler : MonoBehaviour,
                                            ITrackableEventHandler
{
    #region PRIVATE_MEMBER_VARIABLES
 
    private TrackableBehaviour mTrackableBehaviour;

	private bool mHasBeenFound = false;
	private bool mLostTracking;
	private float mSecondsSinceLost;
	private float distanceToCamera;
	
	private float mVideoCurrentPosition;
	private float mCurrentVolume;
	
	private Transform mSphere;
    
    #endregion // PRIVATE_MEMBER_VARIABLES



    #region UNTIY_MONOBEHAVIOUR_METHODS
    
    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

	void Update()
	{
		//for testing audio levels while in editor      
		//distanceToCamera = Vector3.Distance(Camera.main.transform.position, transform.root.position);
		//Debug.Log(Mathf.Clamp01(1.0f-distanceToCamera*0.01f));

		if (mHasBeenFound) {
			VideoPlaybackBehaviour video = GetComponentInChildren<VideoPlaybackBehaviour>();
			mCurrentVolume = 1.0f-(Mathf.Clamp01(distanceToCamera*0.006f)*0.5f);
			video.VideoPlayer.SetVolume(mCurrentVolume);
		}

		// Pause the video if tracking is lost for more than n seconds
		if (mHasBeenFound && mLostTracking) {
				VideoPlaybackBehaviour video = GetComponentInChildren<VideoPlaybackBehaviour> ();
				if (video != null &&
					video.CurrentState == VideoPlayerHelper.MediaState.PLAYING) {
					//fade out volume from current if marker is lost
					//Debug.Log(mCurrentVolume - mSecondsSinceLost);
					video.VideoPlayer.SetVolume (Mathf.Clamp01 (mCurrentVolume - mSecondsSinceLost));
				}
				//n.0f is number of seconds before playback stops when marker is lost
				if (mSecondsSinceLost > 1.0f) {   
					if (video != null &&
						video.CurrentState == VideoPlayerHelper.MediaState.PLAYING) {
						//get last position so it can resume after video is unloaded and reloaded.
						mVideoCurrentPosition = video.VideoPlayer.GetCurrentPosition ();
						video.VideoPlayer.Pause ();
	
		
					}
					mLostTracking = false;
				}
			mSecondsSinceLost += Time.deltaTime;
		}
	}

    #endregion // UNTIY_MONOBEHAVIOUR_METHODS



    #region PUBLIC_METHODS

    /// <summary>
    /// Implementation of the ITrackableEventHandler function called when the
    /// tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
                                    TrackableBehaviour.Status previousStatus,
                                    TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OnTrackingFound();
        }
        else
        {
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS



    #region PRIVATE_METHODS


    private void OnTrackingFound()
    {
        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

        // Enable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = true;
        }

        // Enable colliders:
        foreach (Collider component in colliderComponents)
        {
            component.enabled = true;
        }

        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");

		mHasBeenFound = true;
		mLostTracking = false;
    }


    private void OnTrackingLost()
    {
        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

        // Disable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = false;
        }

        // Disable colliders:
        foreach (Collider component in colliderComponents)
        {
            component.enabled = false;
        }

        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");

		mLostTracking = true;
		mSecondsSinceLost = 0;
    }

    #endregion // PRIVATE_METHODS
}
