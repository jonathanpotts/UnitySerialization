using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A generic dictionary implementation that works with Unity's serialization system to allow it to be used in the editor and assets.
/// </summary>
/// <typeparam name="TKey">Type used for the key.</typeparam>
/// <typeparam name="TValue">Type used for the value.</typeparam>
[Serializable]
public class SerializableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    /// <summary>
    /// A list used to serialize the dictionary elements.
    /// </summary>
    [SerializeField]
    private List<SerializableKeyValuePair<TKey, TValue>> _elements;

#pragma warning disable IDE0052
    /// <summary>
    /// Tells the property drawer if there are any null keys in the elements list.
    /// </summary>
    [SerializeField, HideInInspector]
    private bool _containsNullKeys = false;

    /// <summary>
    /// Tells the property drawer if there are any duplicate keys in the elements list.
    /// </summary>
    [SerializeField, HideInInspector]
    private bool _containsDuplicateKeys = false;
#pragma warning restore IDE0052

    /// <summary>
    /// The internal dictionary used for storing data.
    /// </summary>
    private readonly Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

    public TValue this[TKey key]
    {
        get => _dictionary[key];

        set
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException("The collection is read-only.");
            }

            foreach (var element in _elements?.Where(x => x.Key.Equals(key)) 
                ?? Enumerable.Empty<SerializableKeyValuePair<TKey, TValue>>())
            {
                _elements.Remove(element);
            }

            if (_elements == null)
            {
                _elements = new List<SerializableKeyValuePair<TKey, TValue>>();
            }

            _elements.Add(new SerializableKeyValuePair<TKey, TValue>(key, value));

            UpdateDictionary();
        }
    }

    public ICollection<TKey> Keys => _dictionary.Keys;

    public ICollection<TValue> Values => _dictionary.Values;

    public int Count => _dictionary.Count;

    public bool IsReadOnly 
        => ((IList<SerializableKeyValuePair<TKey, TValue>>)_elements)?.IsReadOnly ?? false;

    public void Add(TKey key, TValue value)
    {
        if (IsReadOnly)
        {
            throw new InvalidOperationException("The collection is read-only.");
        }

        if (_elements == null)
        {
            _elements = new List<SerializableKeyValuePair<TKey, TValue>>();
        }

        _elements.Add(new SerializableKeyValuePair<TKey, TValue>(key, value));

        UpdateDictionary();
    }

    public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

    public void Clear()
    {
        if (IsReadOnly)
        {
            throw new InvalidOperationException("The collection is read-only.");
        }

        _elements?.Clear();

        UpdateDictionary();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) 
        => ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).Contains(item);

    public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) 
        => ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).CopyTo(array, arrayIndex);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

    public bool Remove(TKey key)
    {
        if (IsReadOnly)
        {
            throw new InvalidOperationException("The collection is read-only.");
        }

        var found = false;

        foreach (var element in _elements?.Where(x => x.Key.Equals(key)) 
            ?? Enumerable.Empty<SerializableKeyValuePair<TKey, TValue>>())
        {
            found = true;

            _elements.Remove(element);
        }

        if (found)
        {
            UpdateDictionary();
        }

        return found;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        if (IsReadOnly)
        {
            throw new InvalidOperationException("The collection is read-only.");
        }

        var found = false;

        foreach (var element in _elements?.Where(x => x.Key.Equals(item.Key) && x.Value.Equals(item.Value)) 
            ?? Enumerable.Empty<SerializableKeyValuePair<TKey, TValue>>())
        {
            found = true;

            _elements.Remove(element);
        }

        if (found)
        {
            UpdateDictionary();
        }

        return found;
    }

    public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        UpdateDictionary();
    }

    /// <summary>
    /// Updates the internal dictionary based on the current elements in the list used for serialization.
    /// </summary>
    private void UpdateDictionary()
    {
        _dictionary.Clear();

        foreach (var element in _elements?.GroupBy(x => x.Key).Where(x => x.Key != null).Select(x => x.First())
            ?? Enumerable.Empty<SerializableKeyValuePair<TKey, TValue>>())
        {
            _dictionary.Add(element.Key, element.Value);
        }

        _containsNullKeys = _elements?.Any(x => x.Key == null) ?? false;
        _containsDuplicateKeys = _elements?.GroupBy(x => x.Key).Any(x => x != null && x.Count() > 1) ?? false;
    }

    /// <summary>
    /// Creates a new serializable dictionary with no data.
    /// </summary>
    public SerializableDictionary()
    {
    }

    /// <summary>
    /// Creates a new serializable dictionary from the provided dictionary.
    /// </summary>
    /// <param name="dictionary">Dictionary.</param>
    public SerializableDictionary(Dictionary<TKey, TValue> dictionary)
        : this()
    {
        _elements = new List<SerializableKeyValuePair<TKey, TValue>>();

        foreach (var serializedKeyValuePair in dictionary.Select(x => new SerializableKeyValuePair<TKey, TValue>(x)))
        {
            _elements.Add(serializedKeyValuePair);
        }

        UpdateDictionary();
    }

    /// <summary>
    /// Converts a standard dictionary to one that can be used with Unity's serialization system.
    /// </summary>
    /// <param name="dictionary">Standard dictionary.</param>
    public static explicit operator SerializableDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        => new SerializableDictionary<TKey, TValue>(dictionary);

    /// <summary>
    /// Converts a dictionary used for Unity's serialization system to a standard one.
    /// </summary>
    /// <param name="serializableDictionary">Serialized dictionary.</param>
    public static explicit operator Dictionary<TKey, TValue>(SerializableDictionary<TKey, TValue> serializableDictionary)
    {
        var dictionary = new Dictionary<TKey, TValue>();

        foreach (var keyValuePair in serializableDictionary)
        {
            dictionary.Add(keyValuePair.Key, keyValuePair.Value);
        }

        return dictionary;
    }
}
