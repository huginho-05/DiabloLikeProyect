using UnityEngine;

public class ClickEffect : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float frameRate = 0.1f;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(PlayAnimation());
    }

    System.Collections.IEnumerator PlayAnimation()
    {
        foreach (Sprite s in sprites)
        {
            sr.sprite = s;
            yield return new WaitForSeconds(frameRate);
        }

        Destroy(gameObject);
    }
}
