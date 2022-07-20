using UnityEngine;

public class BillboardController : MonoBehaviour
{
    Canvas _canvas;
    private void Start()
    {
        _canvas = GetComponent<Canvas>();
    }
    private void Update()
    {
        _canvas.transform.rotation = Camera.main.transform.rotation;
    }
}
