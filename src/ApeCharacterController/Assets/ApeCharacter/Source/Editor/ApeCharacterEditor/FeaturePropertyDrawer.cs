// using System;
// using System.Collections.Generic;
// using System.Globalization;
// using System.Reflection;
// using UnityEditor;
// using UnityEditor.UIElements;
// using UnityEngine;
// using UnityEngine.UIElements;
//
// namespace ApeCharacter.Editor
// {
//     [CustomPropertyDrawer(typeof(IApeFeature), true)]
//     public class FeaturePropertyDrawer : PropertyDrawer
//     {
//         public override VisualElement CreatePropertyGUI(SerializedProperty property)
//         {
//             
//             return MakePropertyGUI(property, PropertyUtils.GetTypeFromProperty(property, false).Name);
//         }
//         
//         protected VisualElement MakePropertyGUI(SerializedProperty property, string headTitle)
//         {
//             VisualElement root = new VisualElement();
//             VisualElement head = new VisualElement();
//             VisualElement body = new VisualElement();
//
//             root.Add(head);
//             root.Add(body);
//
//             this.BuildHead(head, body, property, headTitle);
//             // this.BuildBody(body, property);
//
//             return root;
//         }
//
//         private void BuildHead(VisualElement head, VisualElement body, SerializedProperty property,
//             string headTitle)
//         {
//              head.Clear();
//
//             Type typeFull = PropertyUtils.GetTypeFromProperty(property, true);
//             Type typeField = PropertyUtils.GetTypeFromProperty(property, false);
//             
//             Image image = new Image
//             {
//                 image = Texture2D.redTexture
//             };
//
//             Button btnToggle = new Button
//             {
//                 text = headTitle
//             };
//
//             btnToggle.clicked += () =>
//             {
//                 property.isExpanded = !property.isExpanded;
//             };
//
//             Label btnToggleRightLabel = new Label(PropertyUtils.GetTitleFromType(typeFull));
//             
//             Button btnChangeType = new Button();
//             btnChangeType.SetEnabled(!EditorApplication.isPlayingOrWillChangePlaymode);
//             
//             btnChangeType.clicked += () => Debug.Log("Change type: " + typeField.Name);
//
//
//             Image imageChangeType = new Image
//             {
//                 image = Texture2D.whiteTexture
//             };
//
//             Image imageChevron = new Image
//             {
//                 image = Texture2D.whiteTexture
//             };
//             
//             btnChangeType.Add(imageChangeType);
//             btnChangeType.Add(imageChevron);
//
//             btnToggle.contentContainer.Add(btnToggleRightLabel);
//
//             head.Add(image);
//             head.Add(btnToggle);
//             head.Add(btnChangeType);
//
//             head.Bind(property.serializedObject);
//             // this.UpdateBodyState(property.isExpanded, body);
//
//             // this.OnBuildHead(head, property);
//         }
//     }
//
//     public static class PropertyUtils
//     {
//         private static readonly char[] ASSEMBLY_SEPARATOR = { ' ' };
//         public static Type GetTypeFromProperty(SerializedProperty property, bool fullType)
//         {
//             if (property == null)
//             {
//                 Debug.LogError("Null property was found at 'GetTypeFromProperty'");
//                 return null;
//             }
//             
//             string[] split = fullType
//                 ? property.managedReferenceFullTypename.Split(ASSEMBLY_SEPARATOR)
//                 : property.managedReferenceFieldTypename.Split(ASSEMBLY_SEPARATOR);
//
//             return split.Length != 2 
//                 ? null : 
//                 Type.GetType(Assembly.CreateQualifiedName(split[0], split[1]));
//         }
//         
//         public static string GetTitleFromType(Type type, HashSet<string> forbiddenNames = null)
//         {
//             if (type == null) return "(none)";
//
//             string titleName = GetNiceName(type);
//
//             if (forbiddenNames == null) return titleName;
//             if (string.IsNullOrEmpty(titleName)) return titleName;
//
//             int number = 1;
//             string complete = titleName;
//
//             while (forbiddenNames.Contains(complete))
//             {
//                 complete = $"{titleName} ({number})";
//                 number += 1;
//             }
//
//             return complete;
//         }
//         
//         public static string GetNiceName(Type type)
//         {
//             return GetNiceName(type.ToString());
//         }
//
//         private static readonly char[] SEPARATOR = { '.' };
//         public static string GetNiceName(string type)
//         {
//             string[] split = type.Split(SEPARATOR);
//             return split.Length > 0 
//                 ? Humanize(split[^1]) 
//                 : string.Empty;
//         }
//         
//         private static readonly TextInfo TXT = CultureInfo.InvariantCulture.TextInfo;
//         
//         public static string Humanize(string source)
//         {
// #if UNITY_EDITOR
//             source = UnityEditor.ObjectNames.NicifyVariableName(source);
// #endif
//             
//             char[] characters = source.ToCharArray();
//             for (int i = 0; i < characters.Length; ++i)
//             {
//                 if (characters[i] == '-') characters[i] = ' ';
//                 if (characters[i] == '_') characters[i] = ' ';
//             }
//
//             source = new string(characters);
//             source = TXT.ToTitleCase(source);
//             
//             return source;
//         }
//     }
//     
// }