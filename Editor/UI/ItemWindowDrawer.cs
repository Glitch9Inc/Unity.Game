using Glitch9.Routina;
using UnityEditor;
using UnityEngine;

namespace Glitch9.Extensions
{
    [CustomPropertyDrawer(typeof(ItemWindowAttribute))]
    public class ItemWindowDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Ensure the property is a float
            if (property.propertyType == SerializedPropertyType.Integer)
            {
                // Button 
                if (GUI.Button(position, label.text))
                {
                    // Show the ItemFinderWindow
                    ItemFinderPopup.Show((resultInt) =>
                    {
                        property.intValue = resultInt;
                    });
                }
            }
            else
            {
                // Draw a warning message if not a float
                EditorGUI.LabelField(position, label.text, "Use ItemWindow with int");
            }
        }
    }
}
