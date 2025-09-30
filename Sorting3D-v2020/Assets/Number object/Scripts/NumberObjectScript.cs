using UnityEngine;


public class NumberObjectScript : MonoBehaviour
{
    public int Number = 42;


    public ObjectStates State = ObjectStates.Falling;


    public Material TextMaterial;


    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<TMPro.TextMeshPro>(out var text))
            {
                if (TextMaterial != null)
                {
                    text.fontMaterial = TextMaterial;
                }

                text.text = Number.ToString();
            }
        }
    }


    void Update()
    {
    }
}
