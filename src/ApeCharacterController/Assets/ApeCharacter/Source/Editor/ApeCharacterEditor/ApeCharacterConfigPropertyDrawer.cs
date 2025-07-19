using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

namespace ApeCharacter.Editor
{
    [CustomPropertyDrawer(typeof(IApeCharacterConfig), true)]
    public class ApeCharacterConfigPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var objectField = new ObjectField
            {
                objectType = typeof(ScriptableObject),
                label = property.displayName
            };

            
            objectField.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue != null && !(evt.newValue is IApeCharacterConfig))
                {
                    Debug.LogError($"Объект {evt.newValue.name} не реализует интерфейс IApeCharacterConfig");
                    objectField.value = evt.previousValue;
                    return;
                }
                
                property.serializedObject.ApplyModifiedProperties();
            });
            
            return objectField;
        }
    }
}