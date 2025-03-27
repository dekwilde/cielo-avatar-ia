using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.Video;

public class SpaceKeyListener : MonoBehaviour
{
    // Importa a função GetAsyncKeyState da User32.dll
    [DllImport("User32.dll")]
    private static extern short GetAsyncKeyState(int vKey);

    // Constante para a tecla 'Space'
    private const int VK_SPACE = 0x20;

    // Variável para armazenar o estado anterior da tecla 'Space'
    private bool isSpacePressed = false;

    // Referência ao GameObject do video canvas
    public GameObject videoCanvas;
    public Animator animator;

    void Update()
    {
        // Verifica o estado da tecla 'Space'
        short keyState = GetAsyncKeyState(VK_SPACE);

        // Se o bit mais significativo do valor retornado for 1, a tecla está pressionada
        bool isSpacePressedNow = (keyState & 0x8000) != 0;

        // Detecta se a tecla foi solta após ser pressionada
        if (!isSpacePressedNow && isSpacePressed)
        {
            Debug.Log("A tecla 'Space' foi solta.");
            videoCanvas.SetActive(true);
            animator.SetTrigger("02");
        }

        // Atualiza o estado anterior da tecla
        isSpacePressed = isSpacePressedNow;
    }
}
