using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CozmoBrainMap : MonoBehaviour
{
    public GameObject UIMapObject;

    Sprite mapimage;
    Texture2D maptex;

    Color Cell_Unknown = new Color(.5f, .5f, .5f, .5f);
    Color Cell_Blocked = new Color(0, 0, 0);
    Color Cell_Empty = new Color(1, 1, 1);

    int map_width = 128;
    int map_height = 128;
    float density = .5f;
    int offset_x = -20;
    int offset_y = 0;

    // Start is called before the first frame update
    void Start()
    {
        mapimage = Sprite.Create(new Texture2D(map_width, map_height), new Rect(0, 0, map_width, map_height), new Vector2(0, 0));
        mapimage.texture.filterMode = FilterMode.Point;
        UIMapObject.GetComponent<Image>().sprite = mapimage;

        /////

        Debug.Log("Rect " + mapimage.rect.width + ", " + mapimage.rect.height);
        for (int i = 0; i < mapimage.rect.width; i++)
        {
            for (int j = 0; j < mapimage.rect.height; j++)
            {
                mapimage.texture.SetPixel(i, j, Cell_Unknown);
            }
        }
        mapimage.texture.Apply();
    }

    private bool colors_match(Color a, Color b)
    {
        float threshold = 0.1f;
        return (
           Mathf.Abs(a.r - b.r) < threshold &&
           Mathf.Abs(a.g - b.g) < threshold &&
           Mathf.Abs(a.b - b.b) < threshold
        );
    }

    public void Set_Pixel(int x, int y, Color col, bool overwrite = false)
    {
        Color current = mapimage.texture.GetPixel(x, y);
        if (colors_match(current, Cell_Unknown) || (overwrite))
            mapimage.texture.SetPixel(x, y, col);
    }

    public void MarkPosition(float x, float y, bool status, int blot_size = 0, bool overwrite = false)
    {
        int new_x = (int)(x / density) + ((int)(mapimage.rect.width / 2)) + offset_x;
        int new_y = (int)(y / density) + ((int)(mapimage.rect.height / 2)) - offset_y;
        //Debug.Log("Mark " + new_x + ", " + new_y);
        if (new_x >= 0 && new_y >= 0 && new_x < mapimage.rect.width && new_y < mapimage.rect.height)
        {
            if (blot_size > 0)
            {
                for (int i = 0; i < blot_size; i++)
                {
                    for (int j = 0; j < blot_size; j++)
                    {
                        int xx = (new_x + i) - (blot_size / 2);
                        int yy = (new_y + j) - (blot_size / 2);
                        Set_Pixel(xx, yy, (status) ? Cell_Blocked : Cell_Empty, overwrite);
                    }
                }
            }
            else
                Set_Pixel(new_x, new_y, (status) ? Cell_Blocked : Cell_Empty, overwrite);

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
