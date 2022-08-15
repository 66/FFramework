using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetAudioValue : MonoBehaviour
{
    private AudioClip micRecord;
    private string deviceName;
    private float volume;
    private Text showValue;
    public GameObject cube;
    void Start()
    {
        deviceName = Microphone.devices[0];
        micRecord = Microphone.Start(deviceName, true, 999, 44100);
        showValue = transform.GetComponent<Text>();
    }

    void Update()
    {
        float temp = GetVolume();
        showValue.text = temp.ToString();
        cube.transform.localScale = new Vector3(temp, temp, temp);
    }

    /// <summary>��ȡ��˷�����</summary>
    /// <returns>��˷��������ֵ</returns>
    private float GetVolume()
    {
        float levelMax = 0;
        if (Microphone.IsRecording(deviceName))
        {
            float[] samples = new float[128];
            int startPosition = Microphone.GetPosition(deviceName) - (128 + 1);
            if (startPosition >= 0)
            {//����˷绹δ��ʽ����ʱ����ֵ��Ϊ��ֵ��AudioClip.GetData�����ᱨ��
                micRecord.GetData(samples, startPosition);
                for (int i = 0; i < 128; i++)
                {
                    float wavePeak = samples[i];
                    if (levelMax < wavePeak)
                    {
                        levelMax = wavePeak;
                    }
                }
                levelMax = levelMax * 99;
               // Debug.Log("��˷�������" + levelMax);
            }
        }
        return levelMax;
    }

}
