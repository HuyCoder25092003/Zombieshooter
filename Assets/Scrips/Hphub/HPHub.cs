using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPHub : MonoBehaviour
{
    Transform anchor_world;
    RectTransform parent_rect;
    [SerializeField] RectTransform rect_trans;
    public Image hp_fg;
    float cur_val,hp_value;
    Camera cam;
    [SerializeField] GameObject group_hp;
    private void Awake()
    {
        cam = Camera.main;
    }
    public void SetUp(Transform anchor_world, RectTransform parent_rect, Color color)
    {
        this.anchor_world = anchor_world;
        this.parent_rect = parent_rect;
        hp_fg.color = color;
    }
    void Update()
    {
        Vector2 screenPoint = cam.WorldToScreenPoint(anchor_world.position);
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parent_rect, screenPoint, null, out localPoint);
        rect_trans.anchoredPosition = localPoint;

        hp_value = Mathf.Lerp(hp_value, cur_val, Time.deltaTime);
        hp_fg.fillAmount = hp_value;
    }
    public void UpdateHP(int cur_hp, int max_hp)
    {
        cur_val = (float)cur_hp / (float)max_hp;
        Debug.Log("Cur val: " + cur_val);
    }
    public void OnDetachHub()
    {
        BYPoolManager.instance.GetPool("HpHub").Despawn(transform);
    }
    public void OnSpawn()
    {
        cur_val = 1;
        hp_value = 1;
        hp_fg.fillAmount = 1;
        group_hp.SetActive(true);
    }
    public void OnDespawn()
    {
        group_hp.SetActive(false);
    }
}
