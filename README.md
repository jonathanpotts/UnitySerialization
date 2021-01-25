# UnitySerialization

Provides support for serializing selected types that are not natively serialized by Unity. This allows them to be used in the Unity editor and assets.

**Requires Unity 2020.1 or newer.**

## Install

To install, download the [latest release unitypackage](https://github.com/jonathanpotts/UnitySerialization/releases/latest/download/UnitySerialization.unitypackage) and import it into your project.

## Quick start

Use the UnitySerialization type instead of the BCL type for the serialized fields in your scripts that inherit from `MonoBehaviour` and `ScriptableObject`.

| BCL Type | UnitySerialization Type |
| --- | --- |
| KeyValuePair<TKey, TValue> | SerializableKeyValuePair<TKey, TValue> | 
| Dictionary<TKey, TValue> | SerializableDictionary<TKey, TValue> |
| HashSet\<T> | SerializableHashSet\<T> |

### Example

```cs
public class MyScript : MonoBehaviour
{
    public SerializableDictionary<string, Texture2D> TexturesWithNames;
}
```

## Conversion

UnitySerialization objects can be converted to BCL objects and vice versa. To do so, use explicit casting.

### Example

```cs
var bclDictionary = (Dictionary<string, Texture2D>)TexturesWithNames;
var unitySerializationDictionary = (SerializedDictionary<string, Texture2D>)standardDictionary;
```
