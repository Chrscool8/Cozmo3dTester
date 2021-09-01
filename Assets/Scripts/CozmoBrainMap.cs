using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class CozmoBrainMap : MonoBehaviour
{
    public Sprite mapimage;

    static int width = 10;
    static int height = 10;
    float density = .5f;
    int offset_x = 64;
    int offset_y = 64;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Rect " + mapimage.rect.width + ", " + mapimage.rect.height);
        for (int i = 0; i < mapimage.rect.width; i++)
        {
            for (int j = 0; j < mapimage.rect.height; j++)
            {
                mapimage.texture.SetPixel(i, j, new Color(1, 1, 1));
            }
        }
        mapimage.texture.Apply();
    }

    public void MarkPosition(float x, float y, bool status)
    {
        int new_x = (int)(x / density) + offset_x;
        int new_y = (int)(y / density) + offset_y;
        Debug.Log("Mark " + new_x + ", " + new_y);
        if (new_x >= 0 && new_y >= 0 && new_x < mapimage.rect.width && new_y < mapimage.rect.height)
        {
            int blot_size = 1;
            for (int i = 0; i < blot_size; i++)
            {
                for (int j = 0; j < blot_size; j++)
                {
                    mapimage.texture.SetPixel((new_x + i) - (blot_size / 2), ((int)(new_y) + j) - (blot_size / 2), new Color(0, 0, 0));
                }
            }
            mapimage.texture.Apply();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //StringBuilder sb = new StringBuilder();
        //for (int i = 0; i < _map.GetLength(1); i++)
        //{
        //    for (int j = 0; j < _map.GetLength(0); j++)
        //    {
        //        sb.Append(_map[i, j]);
        //        sb.Append(' ');
        //    }
        //    sb.AppendLine();
        //    sb.AppendLine();
        //}
        //Debug.Log(sb.ToString());
    }

}
