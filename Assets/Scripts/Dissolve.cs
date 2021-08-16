/***
 * Dissolve effect controller.
 * After effect ends deletes gameobject that has script.
 ***/
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    Material material;
    bool isDissolving = false;
    float fade = 1f;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    public void SetDissolving(bool dissolvingStatus)
    {
        isDissolving = dissolvingStatus;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDissolving && !GameTime.Instance.isPaused)
        {
            fade -= GameTime.Instance.deltaTime;
            if (fade <= 0f)
            {
                fade = 0;
                isDissolving = false;
                Destroy(gameObject);
            }
            material.SetFloat("_Fade", fade);
        }
    }
}
