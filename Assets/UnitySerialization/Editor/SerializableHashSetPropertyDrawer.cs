using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SerializableHashSet<>))]
public class SerializableHashSetPropertyDrawer : PropertyDrawer
{
    private const string _duplicateValues
        = "The hash set contains duplicate values. Only the first element for each value will be accessible.";

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var height = 0f;

        height += EditorStyles.boldLabel.CalcHeight(new GUIContent(property.displayName), EditorGUIUtility.currentViewWidth)
            + EditorGUIUtility.standardVerticalSpacing;

        var elements = property.FindPropertyRelative("_elements");
        height += EditorGUI.GetPropertyHeight(elements, true);

        var containsDuplicateValues = property.FindPropertyRelative("_containsDuplicateValues").boolValue;

        if (containsDuplicateValues)
        {
            height += EditorStyles.helpBox.CalcHeight(new GUIContent(_duplicateValues), EditorGUIUtility.currentViewWidth)
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

        var containsDuplicateValues = property.FindPropertyRelative("_containsDuplicateValues").boolValue;

        if (containsDuplicateValues)
        {
            var helpBoxHeight = EditorStyles.helpBox.CalcHeight(new GUIContent(_duplicateValues), EditorGUIUtility.currentViewWidth);
            EditorGUI.HelpBox(new Rect(position.x, position.y, position.width, helpBoxHeight), _duplicateValues, MessageType.Warning);
            position.y += helpBoxHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        EditorGUI.EndProperty();
    }
}
