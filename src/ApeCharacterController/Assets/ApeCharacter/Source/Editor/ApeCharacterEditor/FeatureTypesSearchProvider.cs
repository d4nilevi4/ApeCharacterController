using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ApeCharacter.Editor
{
    public class FeatureTypesSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        public System.Type baseType;
        public ApeCharacterBase apeCharacter;

        public Action<System.Type> OnTypeSelected;

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> list = new List<SearchTreeEntry>();

            var title = new SearchTreeGroupEntry(new GUIContent("Features")) { level = 0 };
            list.Add(title);

            var existingFeatureTypeNames = apeCharacter?.FeatureTypes.Select(f => f.TypeName);

            var featureTypes = ReflectionUtilities.GetAllDerivedTypes(baseType)
                .Where(t => typeof(IApeFeature).IsAssignableFrom(t) && !t.IsAbstract)
                .Where(t => existingFeatureTypeNames != null && !existingFeatureTypeNames.Contains(t.FullName))
                .OrderBy(t => t.Namespace).ThenBy(t => t.Name);

            foreach (var featureType in featureTypes)
            {
                var featureGroup = new SearchTreeEntry(
                    new GUIContent(featureType.Name, featureType.FullName))
                {
                    level = 1,
                    userData = featureType
                };
                list.Add(featureGroup);
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
        public void AddSystem<T>(T system) where T : IApeSystem { }
        public void RemoveSystem<T>() where T : IApeSystem { }
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