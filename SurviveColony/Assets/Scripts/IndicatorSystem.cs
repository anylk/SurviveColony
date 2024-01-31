using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(SphereCollider))]
public class IndicatorSystem : MonoBehaviour
{
    public Transform targetIndicateTransform;

    [Space(20),Header("Aim Indicator")]
    public Transform aimTransform;
    public float aimingSpeed = 40;

    [Space(20),Header("Indicator Stats")]
    public LayerMask indicatorLayerMask;
    public float indicatorRadius;

    [Space(20), Header("Line Renderer")]
    private LineRenderer indicatorLineRenderer;
    private SphereCollider indicatorCollider;
    [SerializeField] private bool renderLine;
    [SerializeField] private int lineSegments = 50;

    private List<ICollectableItem> collectableItems;
    private List<IIndicatable> indicatableObjects;
    private List<Enemy> indicatableEnemies;
    private Enemy indicatableEnemy;

    private IndicatorPool indicatorPool;

    [SerializeField] private List<IndicatorItemDisplay> printedTexts;

    private void Awake()
    {
        indicatorCollider = GetComponent<SphereCollider>();
        indicatorPool = GetComponent<IndicatorPool>();
        indicatorCollider.isTrigger = true;
    }

    private void Start()
    {
        printedTexts = new List<IndicatorItemDisplay>();
        collectableItems = new List<ICollectableItem>();
        indicatableObjects = new List<IIndicatable>();
        indicatableEnemies = new List<Enemy>();

        if (indicatorLineRenderer == null)
        {
            indicatorLineRenderer = GetComponent<LineRenderer>();
            indicatorLineRenderer.positionCount = lineSegments + 1;
            indicatorLineRenderer.useWorldSpace = false;

            indicatorCollider.radius = indicatorRadius;
        }

        DrawLine();
    }

    private void Update()
    {
        DrawLine();
        UpdateNearEnemy();

        Vector3 targetIndicateCenter = targetIndicateTransform.position;
        targetIndicateCenter.y = indicatorCollider.center.y;
        indicatorCollider.center = targetIndicateCenter;

        if (indicatableEnemy != null)
        {
            //aimTransform.position = Vector3.MoveTowards(aimTransform.position, indicatableEnemy.transform.position, Time.deltaTime * aimingSpeed);
        }
        else
        {
            //aimTransform.position = targetIndicateTransform.transform.position + targetIndicateTransform.transform.forward * indicatorRadius;
        }

        UpdateTextPosition();
    }

    private void UpdateTextPosition()
    {
        int releasedCount = 0;
        for (int i = 0; i < printedTexts.Count; i++)
        {
            int targetIndex = i - releasedCount;
            if (collectableItems[targetIndex].Equals(null) || !collectableItems[targetIndex].activeOnWorld)
            {
                indicatorPool.Pool.Release(printedTexts[targetIndex]);
                printedTexts.RemoveAt(targetIndex);
                collectableItems.RemoveAt(targetIndex);
                releasedCount++;
                continue;
            }
            printedTexts[targetIndex].SetData(collectableItems[targetIndex].displayName);
            printedTexts[targetIndex].transform.position = collectableItems[targetIndex].worldPosition + Vector3.up / 5f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ICollectableItem>(out ICollectableItem collectableItem))
        {
            IndicatorItemDisplay printedText = indicatorPool.Pool.Get();
            printedText.SetData(other.gameObject.name);
            printedText.transform.position = other.transform.position + Vector3.up / 5f;
            printedTexts.Add(printedText);
            collectableItems.Add(collectableItem);
            collectableItem.ToggleIndicate(true);
        }

        if (other.TryGetComponent<IIndicatable>(out IIndicatable indicatable))
        {
            indicatableObjects.Add(indicatable);
            indicatable.ToggleIndicate(true);
        }

        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            indicatableEnemies.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<ICollectableItem>(out ICollectableItem collectableItem))
        {
            IndicatorItemDisplay printedText = printedTexts[collectableItems.IndexOf(collectableItem)];
            indicatorPool.Pool.Release(printedText);
            printedTexts.Remove(printedText);
            collectableItems.Remove(collectableItem);
            collectableItem.ToggleIndicate(false);
        }
        if (other.TryGetComponent<IIndicatable>(out IIndicatable indicatable))
        {
            indicatableObjects.Remove(indicatable);
            indicatable.ToggleIndicate(false);
        }

        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            indicatableEnemies.Remove(enemy);
        }
    }

    public void UpdateNearEnemy()
    {
        float distance = Mathf.Infinity;

        if (indicatableEnemies.Count < 1)
        {
            indicatableEnemy = null;
            PlayerController.instance.SetEnemy(indicatableEnemy);
            return;
        }

        foreach (Enemy enemy in indicatableEnemies)
        {
            float newDistance = Vector3.Distance(transform.position, enemy.transform.position);

            if (newDistance < distance)
            {
                distance = newDistance;
                indicatableEnemy = enemy;
                PlayerController.instance.SetEnemy(enemy);
            }
        }
    }



    public void UpdateIndicatorRange(float newRange)
    {
        indicatorRadius = newRange;
        indicatorCollider.radius = indicatorRadius;
        DrawLine();
    }

    [ExecuteAlways]
    private void DrawLine()
    {
        if (indicatorLineRenderer.enabled != renderLine)
        {
            indicatorLineRenderer.enabled = renderLine;
        }

        if (!renderLine)
        {
            return;
        }

        float x;
        float y;

        float angle = 20f;

        for (int i = 0; i < (lineSegments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * indicatorRadius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * indicatorRadius;

            Vector3 targetIndicatePosition = targetIndicateTransform.position;
            targetIndicatePosition.y = 0;
            indicatorLineRenderer.SetPosition(i, new Vector3(x, 0.01f, y) + targetIndicatePosition);

            angle += (360f / lineSegments);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (targetIndicateTransform == null)
        {
            return;
        }
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(targetIndicateTransform.position, indicatorRadius);
    }
}
