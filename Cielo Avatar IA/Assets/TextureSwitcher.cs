using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureSwitcher : MonoBehaviour
{
    public Material targetMaterial;
    public List<Texture> idleTextures;
    public List<Texture> animTextures;

    public float frameRate = 0.2f;
    private bool isAnimating = false;
    private Coroutine textureCoroutine;

    void Start()
    {
        StartTextureLoop();
    }

    void StartTextureLoop()
    {
        if (textureCoroutine != null)
        {
            StopCoroutine(textureCoroutine);
        }
        textureCoroutine = StartCoroutine(SwitchTextures());
    }

    public void SetAnimationState(bool animate)
    {
        isAnimating = animate;
    }

    IEnumerator SwitchTextures()
    {
        int index = 0;
        while (true)
        {
            List<Texture> currentTextures = isAnimating ? animTextures : idleTextures;

            // Garante que há texturas antes de tentar acessar o índice
            if (currentTextures == null || currentTextures.Count == 0)
            {
                index = 0; // Reseta o índice para evitar acessos inválidos
                yield return null;
                continue;
            }

            // Garante que o índice está dentro do limite válido
            if (index >= currentTextures.Count)
            {
                index = 0;
            }

            targetMaterial.mainTexture = currentTextures[index];
            index = (index + 1) % currentTextures.Count;

            yield return new WaitForSeconds(frameRate);
        }
    }

}
