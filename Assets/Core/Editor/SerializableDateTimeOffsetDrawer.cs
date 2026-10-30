using System;
using System.Globalization;
using Core.Features.TimeClock.Data;
using UnityEditor;
using UnityEngine;

namespace Core.Editor
{
    [CustomPropertyDrawer(typeof(SerializableDateTimeOffset))]
    public sealed class SerializableDateTimeOffsetDrawer : PropertyDrawer
    {
        private const float ButtonWidth = 46f;
        private static readonly Color ErrorColor = new(1f, 0.45f, 0.45f);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var valueProp = property.FindPropertyRelative("_value");
            var line = EditorGUIUtility.singleLineHeight;

            EditorGUI.BeginProperty(position, label, property);

            var row1 = new Rect(position.x, position.y, position.width, line);
            var fieldRect = EditorGUI.PrefixLabel(row1, label);
            var textRect = new Rect(fieldRect.x, fieldRect.y, fieldRect.width - ButtonWidth - 2, line);
            var buttonRect = new Rect(textRect.xMax + 2, fieldRect.y, ButtonWidth, line);
            var infoRect = new Rect(fieldRect.x, position.y + line + EditorGUIUtility.standardVerticalSpacing, fieldRect.width, line);

            var isValid = TryParse(valueProp.stringValue, out var parsed);

            var prevColor = GUI.color;
            if (!isValid)
                GUI.color = ErrorColor;

            // DelayedTextField применяет значение по Enter или потере фокуса, а не на каждую букву
            EditorGUI.BeginChangeCheck();
            var text = EditorGUI.DelayedTextField(textRect, valueProp.stringValue);
            if (EditorGUI.EndChangeCheck())
            {
                valueProp.stringValue = TryParse(text, out var p)
                    ? p.ToString(SerializableDateTimeOffset.Format, CultureInfo.InvariantCulture)
                    : text;
            }

            GUI.color = prevColor;

            if (GUI.Button(buttonRect, "Now"))
            {
                valueProp.stringValue = DateTimeOffset.Now
                    .ToString(SerializableDateTimeOffset.Format, CultureInfo.InvariantCulture);
                GUI.FocusControl(null);
            }

            if (isValid)
            {
                EditorGUI.LabelField(infoRect, $"UTC: {parsed.UtcDateTime:yyyy-MM-dd HH:mm:ss}", EditorStyles.miniLabel);
            }
            else
            {
                var style = new GUIStyle(EditorStyles.miniLabel);
                style.normal.textColor = ErrorColor;
                EditorGUI.LabelField(infoRect, "Формат: 2026-10-29 21:00:00 +05:00", style);
            }

            EditorGUI.EndProperty();
        }

        private static bool TryParse(string text, out DateTimeOffset result)
            => DateTimeOffset.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out result);
    }
}