# UnitySerialization

Provides support for serializing selected types that are not natively serialized by Unity. This allows them to be used in the Unity editor and assets.

**Requires Unity 2020.1 or newer.**

## Install

To install, download the [latest release](https://github.com/jonathanpotts/UnitySerialization/releases) unitypackage and install it.

## Quick start

Use the UnitySerialization type instead of the BCL type for the serialized fields in your scripts that inherit from `MonoBehaviour` and `ScriptableObject`.

| BCL Type | UnitySerialization Type |
| --- | --- |
| KeyValuePair<TKey, TValue> | SerializableKeyValuePair<TKey, TValue> | 
| Dictionary<TKey, TValue> | SerializableDictionary<TKey, TValue> |

### Example

```cs
public class MyScript : MonoBehaviour
{
    public SerializableDictionary<string, Texture2D> texturesWithNames;
}
```
