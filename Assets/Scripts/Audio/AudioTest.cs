using System;
using System.IO;
using UnityEngine;
using Mp3Sharp;

public class AudioTest : MonoBehaviour 
{
    AudioSource audioSource;

	// Use this for initialization
	void Start () 
    {
        audioSource = GetComponent<AudioSource>();
        LoadAudioClip();
	}
	
	void LoadAudioClip()
    {
        Debug.Log("Starting");
        
        FileStream stream = new FileStream(Path.Combine(Application.dataPath, "/test.mp3"), FileMode.Open);
        //FileStream stream = new FileStream("C:/Users/ReliC/Desktop/waterloop1.mp3", FileMode.Open);
        BinaryReader reader = new BinaryReader(stream);
        byte[] bytes = reader.ReadBytes((int)stream.Length);
        
        Debug.Log("Bytes: " + bytes.Length);

        AudioClip clip = GetAudioClipFromMP3ByteArray(bytes);

        Debug.Log("Playing");
        audioSource.clip = clip;
        audioSource.Play();
    }

    private AudioClip GetAudioClipFromMP3ByteArray(byte[] in_aMP3Data)
    {
        AudioClip l_oAudioClip = null;
        Stream l_oByteStream = new MemoryStream(in_aMP3Data);
        Mp3Stream l_oMP3Stream = new Mp3Stream(l_oByteStream);

        //Get the converted stream data
        MemoryStream l_oConvertedAudioData = new MemoryStream();
        byte[] l_aBuffer = new byte[2048];
        int l_nBytesReturned = -1;
        int l_nTotalBytesReturned = 0;

        while (l_nBytesReturned != 0)
        {
            l_nBytesReturned = l_oMP3Stream.Read(l_aBuffer, 0, l_aBuffer.Length);
            l_oConvertedAudioData.Write(l_aBuffer, 0, l_nBytesReturned);
            l_nTotalBytesReturned += l_nBytesReturned;
        }

        Debug.Log("MP3 file has " + l_oMP3Stream.ChannelCount + " channels with a frequency of " + l_oMP3Stream.Frequency);

        byte[] l_aConvertedAudioData = l_oConvertedAudioData.ToArray();
        Debug.Log("Converted Data has " + l_aConvertedAudioData.Length + " bytes of data");

        //Convert the byte converted byte data into float form in the range of 0.0-1.0
        float[] l_aFloatArray = new float[l_aConvertedAudioData.Length / 2];

        for (int i = 0; i < l_aFloatArray.Length; i++)
        {
            //Yikes, remember that it is SIGNED Int16, not unsigned (spent a bit of time before realizing I screwed this up...)
            l_aFloatArray[i] = (float)(BitConverter.ToInt16(l_aConvertedAudioData, i * 2) / 32768.0f);
        }

        //For some reason the MP3 header is readin as single channel despite it containing 2 channels of data (investigate later)
        l_oAudioClip = AudioClip.Create("MySound", l_aFloatArray.Length, 2, l_oMP3Stream.Frequency, false, false);
        l_oAudioClip.SetData(l_aFloatArray, 0);

        return l_oAudioClip;
    }
}
