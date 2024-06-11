using Glitch9.Database;
using Glitch9.DB;
using Glitch9.ExtendedEditor;
using Glitch9.Game;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Glitch9.Routina
{
    public class ItemFinderPopup : EditorWindow
    {
        private Dictionary<int, Item> cashedItems;
        private Dictionary<int, Item> _searchedItems;
        private Dictionary<int, Item> searchedItems => _searchedItems ??= cashedItems;

        private Vector2 _scrollPosition;
        private string _searchWord;
        private static Action<int> _onItemSelected;
        private static Action<string, int> _onItemAndQuantitySelected;
        private static bool _quantitySelect = false;
        private int _quantity = 1;
        private bool _isLoading = false;

        public static void Show(Action<int> onItemSelected)
        {
            ItemFinderPopup window = (ItemFinderPopup)GetWindow(typeof(ItemFinderPopup), false, "아이템 찾기");
            window.minSize = new Vector2(480, 100);
            window.maxSize = new Vector2(480, 600);
            _onItemSelected = onItemSelected;
            _quantitySelect = false;
            window.ShowUtility();
        }

        public static void Show(Action<string, int> onItemAndQuantitySelected)
        {
            ItemFinderPopup window = (ItemFinderPopup)GetWindow(typeof(ItemFinderPopup), false, "아이템 찾기");
            window.minSize = new Vector2(480, 100);
            window.maxSize = new Vector2(480, 600);
            _onItemAndQuantitySelected = onItemAndQuantitySelected;
            _quantitySelect = true;
            window.ShowUtility();
        }

        private void DrawLabels()
        {
            EGUILayout.HorizontalLayout(EGUIStyles.Border(GUIBorder.Top), () =>
            {
                EditorGUILayout.LabelField("  Index", GUILayout.Width(42f), GUILayout.MaxWidth(84f));
                EditorGUILayout.LabelField("Id", GUILayout.Width(80f), GUILayout.MaxWidth(160f));
                EditorGUILayout.LabelField("Rarity", GUILayout.Width(50f), GUILayout.MaxWidth(100f));
                //EditorGUILayout.LabelField("Price", GUILayout.Width(80f), GUILayout.MaxWidth(160f));
            });
        }

        private void DrawItems()
        {
            foreach (KeyValuePair<int, Item> item in searchedItems)
            {
                EditorGUILayout.BeginHorizontal(EGUI.Box(new RectOffset(5, 5, 2, 2)));
                EditorGUILayout.LabelField(item.Value.Index.ToString(), GUILayout.Width(40f), GUILayout.MaxWidth(80f));
                EditorGUILayout.LabelField(new GUIContent(item.Value.Icon.texture), GUILayout.Width(20f), GUILayout.Height(20f));
                EditorGUILayout.LabelField(item.Value.Id, GUILayout.Width(140f), GUILayout.MaxWidth(280f));
                EditorGUILayout.LabelField(item.Value.Rarity.ToString(), GUILayout.Width(50f), GUILayout.MaxWidth(100f));

                if (_quantitySelect)
                {
                    _quantity = EditorGUILayout.IntField(_quantity, GUILayout.Width(40f));
                }

                if (GUILayout.Button("Add", GUILayout.Width(60f)))
                {
                    if (_quantitySelect)
                    {
                        _onItemAndQuantitySelected?.Invoke(item.Value.Id, _quantity);
                    }
                    else
                    {
                        _onItemSelected?.Invoke(item.Key);
                    }
                    this.Close();
                }

                EditorGUILayout.EndHorizontal();
            }
        }


        private void OnEnable()
        {
            LoadDatabasesAsync();
        }

        private async void LoadDatabasesAsync()
        {
            _isLoading = true;
            await Sprites.InitializeAsync();
            //lItemDatabase.InitializeWithLoaderV2();
            cashedItems = ItemDatabase.GetDatabase();
            _isLoading = false;
        }

        private void OnGUI()
        {
            if (_isLoading)
            {
                EditorGUILayout.LabelField("Loading...");
                return;
            }
            else if (cashedItems == null)
            {
                EditorGUILayout.LabelField("Error");
                return;
            }

            DrawLabels();

            EGUILayout.VerticalLayout(EGUIStyles.background, () =>
            {
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                GUILayout.Space(5);

                DrawItems();
                GUILayout.FlexibleSpace();

                GUILayout.Space(5);
                EditorGUILayout.EndScrollView();
            });


            EGUILayout.VerticalLayout(EGUIStyles.Border(GUIBorder.Bottom), () =>
            {
                GUILayout.Space(5);
                EGUILayout.HorizontalLayout(() =>
                {

                    EditorGUILayout.LabelField("Search", GUILayout.Width(50));

                    _searchWord = EditorGUILayout.TextField(_searchWord);

                    if (GUILayout.Button("Search", GUILayout.Width(80)))
                    {
                        searchedItems.Clear();
                        if (string.IsNullOrEmpty(this._searchWord)) return;

                        Debug.Log("아이템 찾기:" + _searchWord);
                        foreach (KeyValuePair<int, Item> item in cashedItems)
                        {
                            string search = this._searchWord.ToLower();
                            string key = item.Value.Id.ToLower();

                            if (key.Contains(search))
                            {
                                searchedItems.Add(item.Key, item.Value);
                            }
                        }
                    }
                });
                GUILayout.Space(5);
            });
        }
    }
}