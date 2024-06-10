using Glitch9.ExtendedEditor;
using Glitch9.Game;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Glitch9.Extensions
{
    [CustomPropertyDrawer(typeof(CurrencyAttribute))]
    public class CurrencyAttributeDrawer : PropertyDrawer
    {
        private static string _currencyName;
        private static string[] _cachedCurrencyNames;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int currentIndex = property.intValue;
            property.intValue = DrawCurrencyListDropdown(position, currentIndex, label);
        }

        public static int DrawCurrencyListDropdown(Rect rect, int currentIndex, GUIContent label)
        {
            List<CurrencyEntry> infos = ItemSettings.CurrencyInfos;
            if (_cachedCurrencyNames == null || _cachedCurrencyNames.Length != infos.Count)
            {
                _cachedCurrencyNames = new string[infos.Count];
                for (int i = 0; i < infos.Count; i++)
                {
                    _cachedCurrencyNames[i] = infos[i].currencyId;
                }
            }
            _currencyName = _cachedCurrencyNames[currentIndex];
            string newName = EGUI.StringListDropdown(rect, _currencyName, _cachedCurrencyNames, label);

            if (newName != _currencyName)
            {
                for (int i = 0; i < _cachedCurrencyNames.Length; i++)
                {
                    if (_cachedCurrencyNames[i] == newName)
                    {
                        currentIndex = i;
                        break;
                    }
                }
            }

            _currencyName = newName;
            return currentIndex;
        }
    }
}
