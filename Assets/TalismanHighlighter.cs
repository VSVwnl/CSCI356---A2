using UnityEngine;

public class TalismanHighlight : MonoBehaviour
{
    public Material defaultMaterial;
    public Material highlightMaterial;

    private Renderer talismanRenderer;

    void Start()
    {
        talismanRenderer = GetComponent<Renderer>();
        talismanRenderer.material = defaultMaterial;
    }

    public void Highlight(bool isHighlighted)
    {
        if (isHighlighted)
        {
            talismanRenderer.material = highlightMaterial;
        }
        else
        {
            talismanRenderer.material = defaultMaterial;
        }
    }
}
