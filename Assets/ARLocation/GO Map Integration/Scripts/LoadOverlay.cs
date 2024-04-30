using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadOverlay : MonoBehaviour
{
    private Image _image;

    void Awake()
    {
	_image = GetComponent<Image>();
    }

    public void Show()
    {
	_image.enabled = true;
    }

    public void Hide()
    {
	_image.enabled = false;
    }
}
