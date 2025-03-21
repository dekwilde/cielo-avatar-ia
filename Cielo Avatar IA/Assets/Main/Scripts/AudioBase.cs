using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class AudioBase : MonoBehaviour
{
    [Header("Microfone")]
    [Space(5)]
    public AudioSource microphoneAudioSource;
    public bool useMicrophone;
    public string selectedMicrophone;
    public Text micText;
    public bool showDebug = false;
    int indexMic;

    private void Awake()
    {
        loadJSON();
    }

    // a configuração da porta COM é feita pelo json
    private void loadJSON()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath + "/", "config.json");
        if (File.Exists(filePath))
        {
            var dataAsJson = File.ReadAllText(filePath);
            var loadedData = JsonUtility.FromJson<DataJson>(dataAsJson);
            indexMic = loadedData.indexMic;
        }
    }


    void Start()
    {
        if (useMicrophone)
        {
            if (Microphone.devices.Length > 0)
            {

                selectedMicrophone = Microphone.devices[indexMic].ToString();
                microphoneAudioSource.clip = Microphone.Start(
                    selectedMicrophone,
                    true,
                    20,
                    AudioSettings.outputSampleRate
                );
                micText.text = selectedMicrophone;
                microphoneAudioSource.Play();
            }
        }
    }

    void Update() {
        if(Input.GetKeyDown("d")){
            if(showDebug == false) {
                showDebug = true;
                micText.gameObject.SetActive(true);
            } else {
                showDebug = false; 
                micText.gameObject.SetActive(false);
            }
        }

    }
}