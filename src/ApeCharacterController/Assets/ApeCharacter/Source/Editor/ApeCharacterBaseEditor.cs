using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            var systemTypesField = new PropertyField(serializedObject.FindProperty("SystemTypes"))
            {
                label = "System Types"
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
            var prov = ScriptableObject.CreateInstance<AITypesSearchProvider>();
            prov.baseType = baseType;
            prov.OnTypeSelected = callback;
            SearchWindow.Open<AITypesSearchProvider>(
                new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition),
                    width, height), prov);
        }

        private void CreateAction(Type systemType)
        {
            var apeCharacter = (ApeCharacterBase)target;
            apeCharacter.SystemTypes.Add(new SystemTypeReference(systemType));
        }
    }

    public class AITypesSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        public System.Type baseType;

        public Action<System.Type> OnTypeSelected;
        private Texture2D icon = null;

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            if (icon == null)
                icon = Resources.Load<Texture2D>("wficon");
            List<SearchTreeEntry> list = new List<SearchTreeEntry>();

            var title = new SearchTreeGroupEntry(new GUIContent("Features"));
            title.level = 0;
            title.userData = null;
            list.Add(title);

            var featureTypes = ReflectionUtilities.GetAllDerivedTypes(baseType)
                .Where(t => typeof(IApeFeature).IsAssignableFrom(t) && !t.IsAbstract)
                .OrderBy(t => t.Namespace).ThenBy(t => t.Name);

            foreach (var featureType in featureTypes)
            {
                var featureGroup = new SearchTreeEntry(
                    new GUIContent(featureType.Name, icon, featureType.FullName))
                {
                    level = 1,
                    userData = featureType
                };
                list.Add(featureGroup);
                
                // var systemTypes = GetSystemsForFeature(featureType);
                // foreach (var systemType in systemTypes)
                // {
                //     var systemEntry = new SearchTreeEntry(
                //         new GUIContent(systemType.Name, icon, systemType.FullName))
                //     {
                //         level = 2,
                //         userData = systemType
                //     };
                //     list.Add(systemEntry);
                // }
            }

            return list;
        }

        public static IEnumerable<Type> GetSystemsForFeature(Type featureType)
        {
            if (!typeof(IApeFeature).IsAssignableFrom(featureType))
                return Enumerable.Empty<Type>();

            var systemFactoryStub = new DummySystemFactory();
            var characterStub = new DummyCharacter();

            var _ = (IApeFeature)Activator.CreateInstance(featureType, systemFactoryStub, characterStub);

            systemFactoryStub.Types.Insert(0, featureType);
            return systemFactoryStub.Types ?? Enumerable.Empty<Type>();
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            OnTypeSelected?.Invoke((System.Type)SearchTreeEntry.userData);
            return true;
        }
    }

    public class DummyCharacter : IApeCharacter
    {
        public IApeSystemsContainer Systems { get; } = new DummySystemsContainer();
        public IApeComponentsContainer Components { get; }
    }

    public class DummySystemsContainer : IApeSystemsContainer
    {
        public void AddSystem<T>(T system) where T : IApeSystem
        {
        }

        public void RemoveSystem<T>() where T : IApeSystem
        {
        }

        public bool TryGetSystem<T>(out T system) where T : IApeSystem
        {
            throw new NotImplementedException();
        }

        public bool TryGetSystem(Type systemType, out IApeSystem system)
        {
            throw new NotImplementedException();
        }

        public bool TryGetSystem<T>(out IApeSystem system) where T : IApeSystem
        {
            throw new NotImplementedException();
        }

        public bool HasSystem<T>() where T : IApeSystem
        {
            throw new NotImplementedException();
        }

        public bool HasSystem(Type systemType)
        {
            throw new NotImplementedException();
        }

        public T GetSystem<T>() where T : IApeSystem
        {
            throw new NotImplementedException();
        }
    }

    public class DummySystemFactory : IApeSystemFactory
    {
        public List<Type> Types = new();
        
        public T CreateSystem<T>() where T : class, IApeSystem
        {
            Types.Add(typeof(T));
            
            return null;
        }

        public IApeSystem CreateSystem(Type systemType)
        {
            throw new NotImplementedException();
        }
    }

    public static class ReflectionUtilities
    {
        public static Type[] GetAllDerivedTypes(Type baseType)
        {
            var typesList = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(domainAssembly => domainAssembly.GetTypes())
                .Where(type =>
                    baseType.IsAssignableFrom(type) && type != baseType && !type.IsAbstract)
                .ToArray();
            return typesList;
        }
    }
}