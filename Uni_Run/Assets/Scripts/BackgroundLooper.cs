using UnityEngine;

public class BackgroundLooper : MonoBehaviour
{
    public Transform[] backgrounds;         // sky1 ~ sky4
    public float scrollSpeed = 1f;
    public float backgroundWidth = 19.2f;

    void Update()
    {
        foreach (Transform bg in backgrounds)
        {
            bg.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

            if (bg.position.x <= -backgroundWidth)
            {
                float rightMostX = GetRightMostX();
                bg.position = new Vector3(rightMostX + backgroundWidth, bg.position.y, bg.position.z);
            }
        }
    }

    float GetRightMostX()
    {
        float maxX = float.MinValue;
        foreach (Transform bg in backgrounds)
        {
            if (bg.position.x > maxX)
                maxX = bg.position.x;
        }
        return maxX;
    }
}