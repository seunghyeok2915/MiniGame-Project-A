using UnityEngine;

public class ColorObject : MonoBehaviour
{
    [HideInInspector] public Color color;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void SetColor(Color color)
    {
        if (spriteRenderer == null)
        {
            Debug.LogWarning("spriteRender is Null");
            spriteRenderer = transform.GetComponent<SpriteRenderer>();
        }

        this.color = color;
        spriteRenderer.color = color;

        gameObject.SetActive(false);
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);
    }
}
