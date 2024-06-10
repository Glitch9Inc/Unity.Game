using Glitch9.Game;
using Glitch9.Extensions;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Glitch9.EditorTools
{
    [CustomPropertyDrawer(typeof(Price))]
    public class PriceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var value = property.FindPropertyRelative("value");
            var currencyID = property.FindPropertyRelative("currencyId");

            position.height = EditorGUIUtility.singleLineHeight;

            // Indent label
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
            Rect valueRect = new Rect(labelRect.xMax, position.y, (position.width - labelRect.width) * 0.5f, position.height);
            Rect idRect = new Rect(valueRect.xMax, position.y, (position.width - labelRect.width) * 0.5f, position.height);

            // Draw fields
            EditorGUI.LabelField(labelRect, label);
            EditorGUI.PropertyField(valueRect, value, GUIContent.none);
            currencyID.intValue = CurrencyAttributeDrawer.DrawCurrencyListDropdown(idRect, currencyID.intValue, GUIContent.none);
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
