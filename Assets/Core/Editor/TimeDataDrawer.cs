#if UNITY_EDITOR
using Core.Features.TimeClock.Data;
using UnityEditor;
using UnityEngine;

namespace Core.Editor
{
    [CustomPropertyDrawer(typeof(TimeDate))]
    public class TimeDateDrawer : PropertyDrawer
    {
        private const int Rows = 5;
        private const float PreviewSplit = 0.4f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float lh = EditorGUIUtility.singleLineHeight;

            if (!property.isExpanded)
                return lh;

            return Rows * lh + (Rows - 1) * EditorGUIUtility.standardVerticalSpacing;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            float lh = EditorGUIUtility.singleLineHeight;
            float step = lh + EditorGUIUtility.standardVerticalSpacing;

            // Заголовок: сворачиваемая метка слева, превью справа
            var header = new Rect(position.x, position.y, position.width, lh);
            float splitX = header.width * PreviewSplit;

            var foldRect = new Rect(header.x, header.y, splitX, lh);
            var previewRect = new Rect(header.x + splitX, header.y, header.width - splitX, lh);

            property.isExpanded = EditorGUI.Foldout(foldRect, property.isExpanded, label, true);

            var preview = Read(property).ToDateTimeOffset();
            EditorGUI.LabelField(previewRect, preview.ToString("yyyy-MM-dd HH:mm:ss zzz"), EditorStyles.miniLabel);

            if (property.isExpanded)
            {
                Rect Row(int i) => new Rect(position.x, position.y + i * step, position.width, lh);

                EditorGUI.indentLevel++;
                DrawFields(property, Row(1), new[] { "Year", "Month", "Day" }, new[] { "Y", "M", "D" });
                DrawFields(property, Row(2), new[] { "Hour", "Minute", "Second" }, new[] { "H", "Min", "S" });
                DrawFields(property, Row(3), new[] { "OffsetMinutes" }, new[] { "UTC" });
                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        private static void DrawFields(SerializedProperty property, Rect row, string[] names, string[] captions)
        {
            const float gap = 6f;
            float width = (row.width - gap * (names.Length - 1)) / names.Length;

            float oldLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 40f;

            for (int i = 0; i < names.Length; i++)
            {
                var rect = new Rect(row.x + i * (width + gap), row.y, width, row.height);
                EditorGUI.PropertyField(rect, property.FindPropertyRelative(names[i]), new GUIContent(captions[i]));
            }

            EditorGUIUtility.labelWidth = oldLabelWidth;
        }

        private static TimeDate Read(SerializedProperty p) => new TimeDate
        {
            Year = p.FindPropertyRelative("Year").intValue,
            Month = p.FindPropertyRelative("Month").intValue,
            Day = p.FindPropertyRelative("Day").intValue,
            Hour = p.FindPropertyRelative("Hour").intValue,
            Minute = p.FindPropertyRelative("Minute").intValue,
            Second = p.FindPropertyRelative("Second").intValue,
            OffsetMinutes = p.FindPropertyRelative("OffsetMinutes").intValue
        };
    }
}
#endif