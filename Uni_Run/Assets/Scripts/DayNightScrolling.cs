using UnityEngine;

public class DayNightScrolling : MonoBehaviour
{
    public GameObject[] backgrounds; // ³·1, ³·2, ¹ã1, ¹ã2
    public float scrollSpeed = 1f;

    private float bgWidth;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        bgWidth = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // ¹è°æ ÀÌµ¿
        foreach (GameObject bg in backgrounds)
        {
            bg.transform.Translate(Vector2.left * scrollSpeed * Time.deltaTime);
        }

        // ¹è°æ Àç¹èÄ¡
        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (IsOutOfView(backgrounds[i]))
            {
                float rightMostX = GetRightMostBackground().transform.position.x;
                backgrounds[i].transform.position = new Vector2(rightMostX + bgWidth, backgrounds[i].transform.position.y);

                ShiftBackgroundOrder(i); // ¼ø¼­ À¯Áö
            }
        }
    }

    bool IsOutOfView(GameObject bg)
    {
        float camLeftEdge = mainCam.transform.position.x - (mainCam.orthographicSize * mainCam.aspect);
        float bgRightEdge = bg.transform.position.x + (bgWidth / 2f);

        return bgRightEdge < camLeftEdge;
    }

    GameObject GetRightMostBackground()
    {
        GameObject rightMost = backgrounds[0];
        foreach (GameObject bg in backgrounds)
        {
            if (bg.transform.position.x > rightMost.transform.position.x)
                rightMost = bg;
        }
        return rightMost;
    }

    void ShiftBackgroundOrder(int movedIndex)
    {
        GameObject moved = backgrounds[movedIndex];
        for (int i = movedIndex; i < backgrounds.Length - 1; i++)
        {
            backgrounds[i] = backgrounds[i + 1];
        }
        backgrounds[backgrounds.Length - 1] = moved;
    }
}
