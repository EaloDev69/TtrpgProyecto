using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private BoxCollider2D col;
    [SerializeField] private float movSpeed;
    private Item item;
    public void Initialize(Item item)
    {
        this.item = item;
        sr.sprite = item.image;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        bool canAdd = InventoryManager.instance.AddItem(item);
        if (canAdd)
        {
            StartCoroutine(MoveAndCollect(other.transform));
        }
    }

    private IEnumerator MoveAndCollect(Transform target)
    {
        Destroy(col);

        while(transform.position != target.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, movSpeed*Time.deltaTime);
            yield return 0;
        }
        Destroy(gameObject);
    }

}
