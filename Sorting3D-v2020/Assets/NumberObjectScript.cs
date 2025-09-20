using UnityEngine;


public class NumberObjectScript : MonoBehaviour
{
    public int number = 42;


    void Start()
    {
        foreach (Transform child in transform)
        {
            var text = child.GetComponent<TMPro.TextMeshPro>();

            if (text != null)
                text.text = number.ToString();
        }
    }


    void Update()
    {
    }
}
