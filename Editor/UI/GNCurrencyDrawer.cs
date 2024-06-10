using UnityEditor;
using UnityEngine;

namespace Glitch9.Game
{

    [CustomPropertyDrawer(typeof(Currency))]
    public class CurrencyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var type = property.FindPropertyRelative("currencyType");
            var price = property.FindPropertyRelative("price");

            position.height = EditorGUIUtility.singleLineHeight;

            // Indent label
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
            EditorGUI.LabelField(labelRect, label);

            Rect typeRect = new Rect(labelRect.xMax, position.y, (position.width - labelRect.width) * 0.5f, position.height);
            Rect priceRect = new Rect(typeRect.xMax, position.y, (position.width - labelRect.width) * 0.5f, position.height);
            EditorGUI.PropertyField(typeRect, type, GUIContent.none);
            EditorGUI.PropertyField(priceRect, price, GUIContent.none);

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
