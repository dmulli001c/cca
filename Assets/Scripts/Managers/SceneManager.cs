using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.CustomEvents;
using CrazyMinnow.SALSA;
using System;
using System.Net;
using System.IO;
using Mp3Sharp;

public class SceneManager : MonoBehaviour {

    public static SceneManager Instance;
    public GameObject speaker;
    public string audioURL = "http://vrex.g.comcast.net/tts/v1/cdn/location?voice=carol&text=";
    public Salsa3D salsa;
    public List<GameObject> resourcesLoaded = new List<GameObject>();

    public AudioSource textToSpeechAudioSource;
    public TVController tvController;

    public Stream l_oByteStream;
    public Mp3Stream l_oMP3Stream;
    public MemoryStream l_oConvertedAudioData;
    public WebClient myClient;
    public FileStream stream;
    public BinaryReader reader;

    void Awake()
    {
        if (Instance == null)
        {
            //If I am the first instance, make me the Singleton
            Instance = this;
            //DontDestroyOnLoad(this);
        }
        else
        {
            //If a Singleton already exists and you find
            //another reference in scene, destroy it!
            if (this != Instance)
                Destroy(this.gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        StartCoroutine(TalkTest());
    }

    IEnumerator TalkTest()
    {
        yield return new WaitForSeconds(1f);
        speakAndWriteText("welcome");

        yield return new WaitForSeconds(3.5f);
        speakAndWriteText("m theory pre sents comcast customer care");

        yield return new WaitForSeconds(4.5f);
        speakAndWriteText("How can we assist you, you can say");

        yield return new WaitForSeconds(4.5f);
        speakAndWriteText("What is my s s i d");

        yield return new WaitForSeconds(3.5f);
        speakAndWriteText("what is my wifi password");

        yield return new WaitForSeconds(3.5f);
        speakAndWriteText("how do I use my tv");
        yield return new WaitForSeconds(4f);
        textToSpeechAudioSource.clip = (AudioClip) Resources.Load("Audio/kraken");
        textToSpeechAudioSource.Play();
        yield return new WaitForSeconds(2f);
        Application.LoadLevel(Application.loadedLevel);
    }

    public void speakAndWriteText(string toBeSpoken)
    {
        StartCoroutine(LoadAudioClip(toBeSpoken));
    }

    public void speakAndWriteText(string toBeDisplayed, string toBeSpoken)
    {
        StartCoroutine(LoadAudioClip(toBeSpoken));
        //TODO add displayed text function
    }

    public void speakAndWriteText(string[] toBeDisplayed, string[] toBeSpoken)
    {
        //TODO all the things
    }

    public IEnumerator LoadAudioClip(string textToSpeech)
    {
        Debug.Log("Starting");

        if (File.Exists(Application.persistentDataPath + "/" + textToSpeech + ".wav"))
        {
            Debug.Log("stream");
            stream = new FileStream(Application.persistentDataPath + "/" + textToSpeech + ".wav", FileMode.Open);
            //FileStream stream = new FileStream(Path.Combine(Application.dataPath, "/test.mp3"), FileMode.Open);
            //FileStream stream = new FileStream("C:/Users/dmulli001c/Desktop/" + textToSpeech + ".wav", FileMode.Open);

            reader = new BinaryReader(stream);
            byte[] bytes = reader.ReadBytes((int)stream.Length);

            Debug.Log("Bytes: " + bytes.Length);
            AudioClip clip = GetAudioClipFromMP3ByteArray(bytes);

            Debug.Log(clip.length);
            gameObject.GetComponent<AudioSource>().clip = clip;
            gameObject.GetComponent<AudioSource>().Play();
        }
        else
        {
            Debug.Log("webclient");
            myClient = new WebClient();
            myClient.Proxy = null;
            myClient.DownloadFile(audioURL + textToSpeech, Application.persistentDataPath + "/" + textToSpeech + ".wav");
            //myClient.DownloadFile(audioURL + textToSpeech, "C:/Users/dmulli001c/Desktop/" + textToSpeech + ".wav");
            yield return myClient;
            stream = new FileStream(Application.persistentDataPath + "/" + textToSpeech + ".wav", FileMode.Open);
            //FileStream stream = new FileStream(Path.Combine(Application.dataPath, "/test.mp3"), FileMode.Open);
            //FileStream stream = new FileStream("C:/Users/dmulli001c/Desktop/" + textToSpeech + ".wav", FileMode.Open);
            reader = new BinaryReader(stream);

            byte[] bytes = reader.ReadBytes((int)stream.Length);

            Debug.Log("Bytes: " + bytes.Length);
            AudioClip clip = GetAudioClipFromMP3ByteArray(bytes);

            Debug.Log(clip.length);
            gameObject.GetComponent<AudioSource>().clip = clip;
            gameObject.GetComponent<AudioSource>().Play();
        }
    }

    private AudioClip GetAudioClipFromMP3ByteArray(byte[] in_aMP3Data)
    {
        AudioClip l_oAudioClip = null;
        l_oByteStream = new MemoryStream(in_aMP3Data);
        l_oMP3Stream = new Mp3Stream(l_oByteStream);
        l_oConvertedAudioData = new MemoryStream();

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

        //For some reason the MP3 header is reading as single channel despite it containing 2 channels of data (investigate later)
        l_oAudioClip = AudioClip.Create("MySound", l_aFloatArray.Length, 2, l_oMP3Stream.Frequency, false, false);
        l_oAudioClip.SetData(l_aFloatArray, 0);

        CloseStreams();

        //Get the converted stream data
        return l_oAudioClip;
    }

    void CloseStreams()
    {
        if (l_oByteStream != null)
        {
            l_oByteStream.Close();
            l_oByteStream.Dispose();
        }
        if (l_oMP3Stream != null)
        {
            l_oMP3Stream.Close();
            l_oMP3Stream.Dispose();
        }
        if (l_oConvertedAudioData != null)
        {
            l_oConvertedAudioData.Close();
            l_oConvertedAudioData.Dispose();
        }
        if (myClient != null)
        {
            myClient.Dispose();
        }
        if (stream != null)
        {
            stream.Close();
            stream.Dispose();
        }
        if (reader != null)
        {
            reader.Close();
        }
    }

    void OnApplicationQuit()
    {
        CloseStreams();
    }
}
