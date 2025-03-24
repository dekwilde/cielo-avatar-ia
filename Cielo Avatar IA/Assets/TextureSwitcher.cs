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

            // Evita erro verificando se há texturas na lista
            if (currentTextures == null || currentTextures.Count == 0)
            {
                yield return null;
                continue;
            }

            targetMaterial.mainTexture = currentTextures[index];
            index = (index + 1) % currentTextures.Count;

            yield return new WaitForSeconds(frameRate);
        }
    }
}
