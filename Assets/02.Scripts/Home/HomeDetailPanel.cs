using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HomeDetailPanel : MonoBehaviour
{
    [SerializeField] GridLayoutGroup itemGrid;
    [SerializeField] GameObject pointer;
    [SerializeField] TextMeshProUGUI description;

    [SerializeField] private float y_init = 0.0f;
    private float pointer_init = 0.0f;

    public void UpdateUI(string description, int idx)
    {
        if (pointer_init == 0.0f)
            Init();

        int column = itemGrid.constraintCount;
        Vector3 pos;

        // transform detail panel
        pos = transform.localPosition;
        float cell_height = itemGrid.spacing.y + itemGrid.cellSize.y;
        if (idx > 3)
        {
            pos.y = y_init - (((idx / 4) - 2) * cell_height);
        }
        else
        {
            pos.y = y_init;
        }
        transform.localPosition = pos;

        // rotate pointer direction
        float rotation = idx < column ? 0.0f : 180.0f;
        pointer.transform.rotation = Quaternion.Euler(0f, 0f, rotation);

        // transform pointer y axis
        var local_pos = pointer.transform.localPosition;
        local_pos.y = idx < column ? pointer_init : -pointer_init;
        pointer.transform.localPosition = local_pos;

        // transform pointer x axis
        pos = pointer.transform.position;
        pos.x = itemGrid.transform.GetChild(idx).position.x;
        pointer.transform.position = pos;

        // set description of the item
        this.description.text = description;
    }

    private void Init()
    {
        //y_init = transform.localPosition.y;
        pointer_init = Mathf.Abs(pointer.transform.localPosition.y);
    }
}
