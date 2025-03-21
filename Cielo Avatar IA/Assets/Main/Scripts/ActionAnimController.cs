using UnityEngine;
using System.Collections;

public class ActionAnimController : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;
    public float highAmplitudeThreshold = 0.5f;
    public float lowAmplitudeThreshold = 0.1f;
    public string[] idleTriggers, talkingTriggers;

    public bool isPlayingAnimation = false; // Variável de controle para verificar se uma animação está em andamento
    private string currentAnimation = ""; // Variável de controle para verificar a animação atual

    public bool isSpeech = false;
    public bool isIdle = true;
    float currentAmplitude;
    public SpeechBlend sb;

    public GameObject videoCanvas;

    void Start() {
        IdleAnim();
    }

    private void Update()
    {
        
        
        if (GetAudioAmplitude() > highAmplitudeThreshold && isSpeech == false)
        {
            videoCanvas.SetActive(false);
            isSpeech = true;
            //sb.enabled = true;
            int randomAnimIndex = Random.Range(0, talkingTriggers.Length);
            string randomAnimName = talkingTriggers[randomAnimIndex];
            animator.SetTrigger(randomAnimName);
            isIdle = false;
            StartCoroutine(EndSpeech());
        }
    }

    // Função para obter a amplitude do som do AudioSource
    private float GetAudioAmplitude()
    {
        float[] samples = new float[1024];
        audioSource.GetOutputData(samples, 0);

        float sum = 0;
        foreach (float sample in samples)
        {
            sum += Mathf.Abs(sample);
        }

        return sum / samples.Length;
    }

    public IEnumerator EndSpeech()
    {
        yield return new WaitForSeconds(4f);
        if (GetAudioAmplitude() > highAmplitudeThreshold && isSpeech == true)
        {
            isSpeech = true;
            
            int randomAnimIndex = Random.Range(0, talkingTriggers.Length);
            string randomAnimName = talkingTriggers[randomAnimIndex];
            animator.SetTrigger(randomAnimName);
            StartCoroutine(EndSpeech());
        } else {
            IdleAnim();
           // sb.enabled = false;
            isSpeech = false;
        }
        
    }

    public void IdleAnim() {
        isIdle = true;
        int randomAnimIndex = Random.Range(0, idleTriggers.Length);
        string randomAnimName = idleTriggers[randomAnimIndex];
        animator.SetTrigger(randomAnimName);
    }
}
