using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ApeCharacter.Editor
{
    [CustomEditor(typeof(ApeCharacterBase), true)]
    public class ApeCharacterBaseEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            var systemTypesField = new PropertyField(serializedObject.FindProperty("FeatureTypes"))
            {
                label = "Feature Types"
            };
            
            root.Add(systemTypesField);
            
            var addSystemButton = new Button()
            {
                text = "Add System",
            };

            addSystemButton.clicked += ShowActionList;

            root.Add(addSystemButton);

            return root;
        }

        private void ShowActionList()
        {
            CreateSearchableWindow(typeof(IApeSystem), CreateAction, 300, 200);
        }

        private void CreateSearchableWindow(Type baseType, Action<Type> callback, int width,
            int height)
        {
            var prov = ScriptableObject.CreateInstance<FeatureTypesSearchProvider>();
            prov.baseType = baseType;
            prov.OnTypeSelected = callback;
            prov.apeCharacter = target as ApeCharacterBase;
            SearchWindow.Open<FeatureTypesSearchProvider>(
                new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition),
                    width, height), prov);
        }

        private void CreateAction(Type systemType)
        {
            var apeCharacter = (ApeCharacterBase)target;
            apeCharacter.FeatureTypes.Add(new FeatureTypeReference(systemType));
        }
    }
}