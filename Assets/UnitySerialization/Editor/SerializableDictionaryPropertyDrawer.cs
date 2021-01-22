using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SerializableDictionary<,>))]
public class SerializableDictionaryPropertyDrawer : PropertyDrawer
{
    private const string _nullKeysText
        = "The dictionary contains null keys. Any element with a null key will be inaccessible.";

    private const string _duplicateKeysText
        = "The dictionary contains duplicate keys. Only the first element for each key will be accessible.";

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var height = 0f;

        height += EditorStyles.boldLabel.CalcHeight(new GUIContent(property.displayName), EditorGUIUtility.currentViewWidth)
            + EditorGUIUtility.standardVerticalSpacing;

        var elements = property.FindPropertyRelative("_elements");
        height += EditorGUI.GetPropertyHeight(elements, true);

        var containsNullKeys = property.FindPropertyRelative("_containsNullKeys").boolValue;

        if (containsNullKeys)
        {
            height += EditorStyles.helpBox.CalcHeight(new GUIContent(_nullKeysText), EditorGUIUtility.currentViewWidth)
                + EditorGUIUtility.standardVerticalSpacing;
        }

        var containsDuplicateKeys = property.FindPropertyRelative("_containsDuplicateKeys").boolValue;

        if (containsDuplicateKeys)
        {
            height += EditorStyles.helpBox.CalcHeight(new GUIContent(_duplicateKeysText), EditorGUIUtility.currentViewWidth)
                + EditorGUIUtility.standardVerticalSpacing;
        }

        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var labelHeight = EditorStyles.boldLabel.CalcHeight(new GUIContent(property.displayName), EditorGUIUtility.currentViewWidth);
        EditorGUI.LabelField(new Rect(position.x, position.y, position.width, labelHeight), new GUIContent(property.displayName), EditorStyles.boldLabel);
        position.y += labelHeight + EditorGUIUtility.standardVerticalSpacing;

        var elements = property.FindPropertyRelative("_elements");
        var elementsHeight = EditorGUI.GetPropertyHeight(elements, true);
        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, elementsHeight), elements, true);
        position.y += elementsHeight + EditorGUIUtility.standardVerticalSpacing;

        var containsNullKeys = property.FindPropertyRelative("_containsNullKeys").boolValue;

        if (containsNullKeys)
        {
            var helpBoxHeight = EditorStyles.helpBox.CalcHeight(new GUIContent(_nullKeysText), EditorGUIUtility.currentViewWidth);
            EditorGUI.HelpBox(new Rect(position.x, position.y, position.width, helpBoxHeight), _nullKeysText, MessageType.Error);
            position.y += helpBoxHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        var containsDuplicateKeys = property.FindPropertyRelative("_containsDuplicateKeys").boolValue;

        if (containsDuplicateKeys)
        {
            var helpBoxHeight = EditorStyles.helpBox.CalcHeight(new GUIContent(_duplicateKeysText), EditorGUIUtility.currentViewWidth);
            EditorGUI.HelpBox(new Rect(position.x, position.y, position.width, helpBoxHeight), _duplicateKeysText, MessageType.Warning);
            position.y += helpBoxHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        EditorGUI.EndProperty();
    }
}
