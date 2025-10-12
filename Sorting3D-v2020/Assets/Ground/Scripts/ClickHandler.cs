using UnityEngine;


public sealed class ClickHandler : MonoBehaviour
{
    void Update()
    {
        // Left-click
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                hit.collider.gameObject.SendMessage("OnClicked", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
