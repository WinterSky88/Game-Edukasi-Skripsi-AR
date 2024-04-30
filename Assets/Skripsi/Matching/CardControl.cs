using UnityEngine;
using UnityEngine.UI;

public class CardControl : MonoBehaviour
{
    public int id;
    public Texture2D frontTexture;
    public Texture2D backTexture;

    public bool isFlipped;

    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
        Flip();
    }

    public void Flip()
    {
        if (isFlipped)
        {
            image.sprite = Sprite.Create(backTexture, new Rect(0, 0, backTexture.width, backTexture.height), Vector2.zero);
        }
        else
        {
            image.sprite = Sprite.Create(frontTexture, new Rect(0, 0, frontTexture.width, frontTexture.height), Vector2.zero);
        }

        isFlipped = !isFlipped;
    }
    private void OnMouseDown()
    {
        Debug.Log(111);
    }
}
