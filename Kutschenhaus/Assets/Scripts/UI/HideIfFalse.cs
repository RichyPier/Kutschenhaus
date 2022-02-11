// #if Unity_Editor

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]

public class HideIfFalse : PropertyAttribute
{
    public string boolProperty;

    public HideIfFalse(string boolProperty)
    {
        this.boolProperty = boolProperty;
    }
}

[CustomPropertyDrawer(typeof(HideIfFalse))]

public class HidePropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        HideIfFalse hideIfAtribute = (HideIfFalse)attribute;
        if (GetConditionalAttributeResult(hideIfAtribute, property))
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        HideIfFalse hideIfAttribute = (HideIfFalse)attribute;

        if (GetConditionalAttributeResult(hideIfAttribute, property))
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    bool GetConditionalAttributeResult(HideIfFalse attribute, SerializedProperty property)
    {
        bool enabled = true;
        Debug.Log(property.propertyPath);

        string[] boolPropertyPathArray = property.propertyPath.Split('.');
        boolPropertyPathArray[boolPropertyPathArray.Length - 1] = attribute.boolProperty;
        string boolPropertyPath = string.Join(".", boolPropertyPathArray);

        SerializedProperty propertyValue = property.serializedObject.FindProperty(boolPropertyPath);

        if (propertyValue != null)
        {
            enabled = propertyValue.boolValue;
        }
        else
        {
            Debug.LogWarning("Conditional Attribute not found");
        }

        return enabled;
    }
}
// #endif
