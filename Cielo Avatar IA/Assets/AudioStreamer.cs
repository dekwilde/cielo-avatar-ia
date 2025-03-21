using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using WebSocketSharp;
using PimDeWitte.UnityMainThreadDispatcher;

public class AudioReceiver : MonoBehaviour
{
    private WebSocket ws;
    private string streamingAssetsPath;
    public AudioSource audioSource;
    private const float delayBeforePlay = 1.0f; // Delay de 1 segundo antes de tocar o áudio

    void Start()
    {
        streamingAssetsPath = Path.Combine(Application.streamingAssetsPath, "");

        ws = new WebSocket("ws://localhost:8080");
        ws.OnMessage += OnMessageReceived;
        ws.Connect();
    }

    private async void OnMessageReceived(object sender, MessageEventArgs e)
    {
        string fileName = e.Data;
        string filePath = Path.Combine(streamingAssetsPath, fileName);

        await Task.Run(() => 
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() => ProcurarAudio(filePath));
        });
    }

    public void ProcurarAudio(string filePath) 
    {
        Debug.Log("Achou o arquivo: " + filePath);
        StartCoroutine(PlayAudio(filePath));
    }

    IEnumerator PlayAudio(string filePath)
    {
        Debug.Log("Entrando coroutina: ");
        
        // Verificar se o arquivo existe
        if (!File.Exists(filePath))
        {
            Debug.LogError("Arquivo não encontrado: " + filePath);
            yield break;
        }

        using (var uwr = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.WAV))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Erro ao carregar o áudio: " + uwr.error);
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
                yield return new WaitForSeconds(delayBeforePlay);
                if (clip != null)
                {
                    Debug.Log("Achou o clip: ");
                    
                    if (audioSource != null)
                    {
                        // Renomear o AudioClip pode ser feito de forma indireta:
                        audioSource.clip = clip;

                        // Adicionar um delay antes de tocar o áudio
                        yield return new WaitForSeconds(delayBeforePlay);

                        audioSource.Play();
                        Debug.Log("Playing audio from: " + filePath);
                    }
                    else
                    {
                        Debug.LogError("No AudioSource component found.");
                    }
                }
                else
                {
                    Debug.LogError("Failed to load AudioClip from file.");
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        if (ws != null)
        {
            ws.Close();
        }
    }
}
