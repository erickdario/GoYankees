/*============================================================================== 
 * Copyright (c) 2012-2014 Qualcomm Connected Experiences, Inc. All Rights Reserved. 
 * ==============================================================================*/

using UnityEngine;
using System.Collections;

public class VideoPlaybackUIView : ISampleAppUIView
{
    #region PUBLIC_PROPERTIES
    public CameraDevice.FocusMode FocusMode
    {
        get
        {
            return mFocusMode;
        }
        set
        {
            mFocusMode = value;
        }
    }
    #endregion PUBLIC_PROPERTIES

    #region PUBLIC_MEMBER_VARIABLES
    public event System.Action TappedToClose;
    public SampleAppUIBox mBox;
    public SampleAppUICheckButton mAboutLabel;
    public SampleAppUILabel mVideoPlaybackLabel;
    public SampleAppUICheckButton mExtendedTracking;
    public SampleAppUICheckButton mCameraFlashSettings;
    public SampleAppUICheckButton mAutoFocusSetting;
    public SampleAppUICheckButton mPlayFullscreeSettings;
    public SampleAppUILabel mCameraLabel;
    public SampleAppUIRadioButton mCameraFacing;
    public SampleAppUIButton mCloseButton;
    #endregion PUBLIC_MEMBER_VARIABLES

    #region PRIVATE_MEMBER_VARIABLES
    private CameraDevice.FocusMode mFocusMode;
    private SampleAppsUILayout mLayout;
    #endregion PRIVATE_MEMBER_VARIABLES

    #region PUBLIC_METHODS

    public void LoadView()
    {

        mLayout = new SampleAppsUILayout();
        mVideoPlaybackLabel = mLayout.AddLabel("GoYankees settings");
		mLayout.AddGap(32	);
        mAboutLabel = mLayout.AddSimpleButton("About GoYankees");
        mLayout.AddGap(4);
        /*mExtendedTracking = mLayout.AddSlider("Extended Tracking", false);
        mLayout.AddGap(2);*/
        mAutoFocusSetting = mLayout.AddSlider("Autofocus", true);
        mLayout.AddGap(4);
        mCameraFlashSettings = mLayout.AddSlider("Flash", false);
        mLayout.AddGap(4);
        mPlayFullscreeSettings = mLayout.AddSlider("Play Fullscreen", false);
        mLayout.AddGap(32);
        mCameraLabel = mLayout.AddGroupLabel("Camera");
        string[] options = { "Front", "Rear" };
        mCameraFacing = mLayout.AddToggleOptions(options, 1);
        Rect CloseButtonRect = new Rect(0, Screen.height - (120 * Screen.width) / 800.0f, Screen.width, (100.0f * Screen.width) / 800.0f);
        mCloseButton = mLayout.AddButton("Close", CloseButtonRect);
    }

    public void UnLoadView()
    {
        mVideoPlaybackLabel = null;
        mExtendedTracking = null;
        mCameraFlashSettings = null;
        mAutoFocusSetting = null;
        mCameraLabel = null;
        mCameraFacing = null;
        mAboutLabel = null;
        mPlayFullscreeSettings = null;
    }

    public void UpdateUI(bool tf)
    {
        if (!tf)
        {
            return;
        }
        mLayout.Draw();
    }

    public void OnTappedToClose()
    {
        if (this.TappedToClose != null)
        {
            this.TappedToClose();
        }
    }
    #endregion PUBLIC_METHODS

}
