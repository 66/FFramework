using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class Permissions : MonoBehaviour
{
    // <summary>¼�Ƶ���ƵԴ</summary>
    protected AudioClip mResultClip = null;
    /// <summary>�Ƿ����ڼ�ʱ</summary>
    protected bool mIsTiming = false;
    /// <summary>��ʼ��ʱ��¼������Ӧ��ʱ��</summary>
    protected float mStartTime;
    /// <summary>��Ƶ���ݳ���</summary>
    protected readonly int mSamplesLength;

    /// <summary>�豸����˷�</summary>
    protected string mDeviceNameMIC;
    /// <summary>¼��������AudioClip�ĳ���</summary>
    public int LengthSec { get; set; }
    /// <summary>��¼��������AudioClip�Ĳ�����</summary>
    public int Frequency { get; set; }
    public void Start()
    {
        mDeviceNameMIC = null;
        LengthSec = 60;//ASR�60��
        Frequency = 16000;
        RequestUserPermission();
    }

    public void RequestUserPermission()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
        GetVolume();
#endif
    }

    /// <summary>��ȡ��˷�����</summary>
    /// <returns>��˷��������ֵ</returns>
    public float GetVolume()
    {
        float levelMax = 0;
        if (mResultClip != null && Microphone.IsRecording(mDeviceNameMIC))
        {
            float[] samples = new float[mSamplesLength];
            int startPosition = Microphone.GetPosition(mDeviceNameMIC) - mSamplesLength + 1;
            if (startPosition >= 0)
            {//����˷绹δ��ʽ����ʱ����ֵ��Ϊ��ֵ��AudioClip.GetData�����ᱨ��
                mResultClip.GetData(samples, startPosition);
                for (int i = 0; i < mSamplesLength; i++)
                {
                    float wavePeak = samples[i];
                    if (levelMax < wavePeak)
                    {
                        levelMax = wavePeak;
                    }
                }
                levelMax = levelMax * 99;
            }
        }

        // Log.I("MicrophoneManager.GetVolume = " + levelMax);
        return levelMax;
    }


}
