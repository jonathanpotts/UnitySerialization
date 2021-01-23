using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SerializableHashSet<T> : ISet<T>, ISerializationCallbackReceiver
{
    /// <summary>
    /// A list used to serialize the hash set elements.
    /// </summary>
    [SerializeField]
    private List<T> _elements;

#pragma warning disable IDE0052
    /// <summary>
    /// Tells the property drawer if there are any duplicate values in the elements list.
    /// </summary>
    [SerializeField, HideInInspector]
    private bool _containsDuplicateValues = false;
#pragma warning restore IDE0052

    /// <summary>
    /// The internal hash set used for storing data;
    /// </summary>
    private readonly HashSet<T> _hashSet = new HashSet<T>();

    public int Count => _hashSet.Count;

    public bool IsReadOnly => ((ICollection<T>)_elements)?.IsReadOnly ?? false;

    public bool Add(T item)
    {
        if (IsReadOnly)
        {
            throw new InvalidOperationException("The collection is read-only.");
        }

        if (_elements?.Contains(item) ?? false)
        {
            return false;
        }

        if (_elements == null)
        {
            _elements = new List<T>();
        }

        _elements.Add(item);

        UpdateHashSet();

        return true;
    }

    public void Clear()
    {
        if (IsReadOnly)
        {
            throw new InvalidOperationException("The collection is read-only.");
        }

        _elements?.Clear();

        UpdateHashSet();
    }

    public bool Contains(T item) => _hashSet.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) 
        => ((ICollection<T>)_hashSet).CopyTo(array, arrayIndex);

    public void ExceptWith(IEnumerable<T> other) 
        => _hashSet.ExceptWith(other);

    public IEnumerator<T> GetEnumerator() => _hashSet.GetEnumerator();

    public void IntersectWith(IEnumerable<T> other) 
        => _hashSet.IntersectWith(other);

    public bool IsProperSubsetOf(IEnumerable<T> other)
        => _hashSet.IsProperSubsetOf(other);

    public bool IsProperSupersetOf(IEnumerable<T> other)
        => _hashSet.IsProperSupersetOf(other);

    public bool IsSubsetOf(IEnumerable<T> other)
        => _hashSet.IsSubsetOf(other);

    public bool IsSupersetOf(IEnumerable<T> other)
        => _hashSet.IsSupersetOf(other);

    public bool Overlaps(IEnumerable<T> other)
        => _hashSet.Overlaps(other);

    public bool Remove(T item)
    {
        if (!(_elements?.Contains(item) ?? false))
        {
            return false;
        }

        _elements?.Remove(item);

        UpdateHashSet();

        return true;
    }

    public bool SetEquals(IEnumerable<T> other)
        => _hashSet.SetEquals(other);

    public void SymmetricExceptWith(IEnumerable<T> other)
        => _hashSet.SymmetricExceptWith(other);

    public void UnionWith(IEnumerable<T> other)
        => _hashSet.UnionWith(other);

    void ICollection<T>.Add(T item) => Add(item);

    IEnumerator IEnumerable.GetEnumerator() 
        => _hashSet.GetEnumerator();

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        UpdateHashSet();
    }

    /// <summary>
    /// Updates the internal hash set based on the current elements in the list used for serialization.
    /// </summary>
    private void UpdateHashSet()
    {
        _hashSet.Clear();

        foreach (var element in _elements?.Distinct() ?? Enumerable.Empty<T>())
        {
            _hashSet.Add(element);
        }

        _containsDuplicateValues = _elements?.GroupBy(x => x).Any(x => x.Count() > 1) ?? false;
    }

    /// <summary>
    /// Creates a new serializable hash set with no data.
    /// </summary>
    public SerializableHashSet()
    {
    }

    /// <summary>
    /// Creates a new serializable dictionary from the provided hash set.
    /// </summary>
    /// <param name="dictionary">Hash set.</param>
    public SerializableHashSet(HashSet<T> hashSet) : this()
    {
        _elements = new List<T>();

        foreach (var element in hashSet)
        {
            _elements.Add(element);
        }

        UpdateHashSet();
    }

    /// <summary>
    /// Converts a standard hash set to one that can be used with Unity's serialization system.
    /// </summary>
    /// <param name="dictionary">Standard hash set.</param>
    public static explicit operator SerializableHashSet<T>(HashSet<T> hashSet)
        => new SerializableHashSet<T>(hashSet);

    /// <summary>
    /// Converts a hash set used for Unity's serialization system to a standard one.
    /// </summary>
    /// <param name="serializableDictionary">Serialized hash set.</param>
    public static explicit operator HashSet<T>(SerializableHashSet<T> serializableHashSet)
    {
        var hashSet = new HashSet<T>();

        foreach (var element in serializableHashSet)
        {
            hashSet.Add(element);
        }

        return hashSet;
    }
}
