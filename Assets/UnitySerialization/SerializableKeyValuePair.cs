using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A generic key-value pair implementation that works with Unity's serialization system to allow it to be used in the editor and assets.
/// </summary>
/// <typeparam name="TKey">Type used for the key.</typeparam>
/// <typeparam name="TValue">Type used for the value.</typeparam>
[Serializable]
public class SerializableKeyValuePair<TKey, TValue>
{
    /// <summary>
    /// Key.
    /// </summary>
    [SerializeField]
    private TKey _key;

    /// <summary>
    /// Key.
    /// </summary>
    public TKey Key
    {
        get => _key;
        set => _key = value;
    }

    /// <summary>
    /// Value.
    /// </summary>
    [SerializeField]
    private TValue _value;

    /// <summary>
    /// Value.
    /// </summary>
    public TValue Value
    {
        get => _value;
        set => _value = value;
    }

    /// <summary>
    /// Creates a new serializable key-value pair with no data.
    /// </summary>
    public SerializableKeyValuePair()
    {
    }

    /// <summary>
    /// Creates a new serializable key-value pair with the provided key and value.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    public SerializableKeyValuePair(TKey key, TValue value) 
        : this()
    {
        _key = key;
        _value = value;
    }

    /// <summary>
    /// Creates a new serializable key-value pair from the provided key-value pair.
    /// </summary>
    /// <param name="keyValuePair">Key-value pair.</param>
    public SerializableKeyValuePair(KeyValuePair<TKey, TValue> keyValuePair) 
        : this(keyValuePair.Key, keyValuePair.Value)
    {
    }

    /// <summary>
    /// Converts a standard key-value pair to one that can be used with Unity's serialization system.
    /// </summary>
    /// <param name="kvp">Standard key-value pair.</param>
    public static explicit operator SerializableKeyValuePair<TKey, TValue>(KeyValuePair<TKey, TValue> keyValuePair) 
        => new SerializableKeyValuePair<TKey, TValue>(keyValuePair);

    /// <summary>
    /// Converts a key-value pair used for Unity's serialization system to a standard one.
    /// </summary>
    /// <param name="skvp">Serialized key-value pair.</param>
    public static explicit operator KeyValuePair<TKey, TValue>(SerializableKeyValuePair<TKey, TValue> serializableKeyValuePair)
        => new KeyValuePair<TKey, TValue>(serializableKeyValuePair.Key, serializableKeyValuePair.Value);
}
