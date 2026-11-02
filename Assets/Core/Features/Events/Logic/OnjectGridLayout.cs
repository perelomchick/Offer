using System.Collections.Generic;
using UnityEngine;

namespace Core.Features.Events.Logic
{
    [ExecuteAlways]
    public class OnjectGridLayout : MonoBehaviour//переделать ии-шную хрень
    {
        [SerializeField] private RectTransform viewport;
        [SerializeField] private int columns = 2; 
        [SerializeField] private int rows = 2; 
        [SerializeField] private Vector2 spacing = new(5, 5);
        [SerializeField] private float minCellSize = 40f; 

        private readonly List<RectTransform> _items = new();


        public void SetItems(IReadOnlyList<RectTransform> items)
        {
            _items.Clear();
            _items.AddRange(items);
            Layout();
        }

        private void Layout()
        {
            if (viewport == null) return;

            int count = _items.Count;
            if (count == 0)
            {
                ((RectTransform)transform).sizeDelta = new Vector2(0, viewport.rect.height);
                return;
            }

            float width = viewport.rect.width;
            float height = viewport.rect.height;
            
            int neededRows = Mathf.CeilToInt((float)count / columns);
            int totalRows = Mathf.Max(rows, neededRows);
            
            float cellByWidth = (width - spacing.x * (columns - 1)) / columns;
            float cellByHeight = (height - spacing.y * (totalRows - 1)) / totalRows;
            float cell = Mathf.Max(minCellSize, Mathf.Min(cellByWidth, cellByHeight));

            float contentHeight = totalRows * cell + spacing.y * (totalRows - 1);
            var content = (RectTransform)transform;
            content.sizeDelta = new Vector2(width, Mathf.Max(height, contentHeight));

            for (int i = 0; i < count; i++)
            {
                int row = i / columns;
                int indexInRow = i % columns;
                
                int itemsInRow = Mathf.Min(columns, count - row * columns);


                float rowWidth = itemsInRow * cell + spacing.x * (itemsInRow - 1);
                float startX = -rowWidth / 2f + cell / 2f;
                float x = startX + indexInRow * (cell + spacing.x);

                float y = -(row * (cell + spacing.y)) - cell / 2f;
                
                var rt = _items[i];
                rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 1f);
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.sizeDelta = new Vector2(cell, cell);
                rt.anchoredPosition = new Vector2(x, y);
            }
        }
    }
}