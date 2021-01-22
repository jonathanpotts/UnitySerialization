# UnitySerialization

Provides support for serializing generic key-value pairs and dictionaries in the Unity editor and assets.

**Requires Unity 2020.1 or newer.**

## Install

To install, download the [latest release](https://github.com/jonathanpotts/UnitySerialization/releases) unitypackage and install it.

## Quick start

Use `SerializableDictionary<TKey, TValue>` instead of `Dictionary<TKey, TValue>` and `SerializableKeyValuePair<TKey, TValue>` instead of `KeyValuePair<TKey, TValue>` for the type for the fields in your scripts that inherit from `MonoBehaviour` and `ScriptableObject`.

### Example

```cs
public class MyScript : MonoBehaviour
{
  public SerializableDictionary<string, Texture2D> texturesWithNames;
}
```
