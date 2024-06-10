using Glitch9.DB;
using Glitch9.Routina;
using UnityEditor;
using UnityEngine;

namespace Glitch9.Game
{
    [CustomPropertyDrawer(typeof(ItemData))]
    public class GNItemDataDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var itemId = property.FindPropertyRelative("Index");
            var quantity = property.FindPropertyRelative("Quantity");

            var itemRect = new Rect(position.x, position.y, position.width * 0.7f, position.height);
            var quantityRect = new Rect(position.x + position.width * 0.7f, position.y, position.width * 0.3f, position.height);
            Item item;

            try
            {
                item = ItemDatabase.Get(itemId.intValue);
            }
            catch
            {
                item = new Item();
            }


            GUIContent buttonContent;
            if (item != null)
            {
                buttonContent = new GUIContent(item.Id, item.Icon.texture);
            }
            else
            {
                buttonContent = new GUIContent("None");
            }

            if (GUI.Button(itemRect, buttonContent))
            {
                // Show the ItemFinderWindow
                ItemFinderPopup.Show((resultInt) =>
                {
                    itemId.intValue = resultInt;
                    property.serializedObject.ApplyModifiedProperties();
                });
            }

            EditorGUI.PropertyField(quantityRect, quantity, GUIContent.none);
            property.serializedObject.ApplyModifiedProperties();
        }

    }
}
