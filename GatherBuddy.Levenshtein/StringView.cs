using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GatherBuddy.Levenshtein;

// A string class that references a substring of an existing string.
public readonly struct StringView : IEnumerable<char>, IComparable<StringView>, IComparable<string>
{
    private readonly string _data;
    private readonly int    _start;
    public           int    Length { get; }

    public StringView(string data, int start = 0, int size = int.MaxValue)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));
        if (start < 0)
            throw new ArgumentOutOfRangeException(nameof(start));
        if (size < 0)
            throw new ArgumentOutOfRangeException(nameof(size));

        _data  = string.Intern(data);
        _start = start;
        Length = Math.Min(_data.Length - _start, size);
    }

    public char this[int idx]
        => _data[_start + idx];

    internal int UnderlyingLength
        => _data.Length;

    public IEnumerator<char> GetEnumerator()
    {
        for (var i = _start; i < Length; ++i)
            yield return _data[i];
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public bool Equals(StringView rhs)
        => rhs.SequenceEqual(this);

    public int CompareTo(StringView rhs)
    {
        var min = Math.Min(Length, rhs.Length);
        for (var i = 0; i < min; ++i)
        {
            var a = this[i];
            var b = rhs[i];
            if (a != b)
                return a - b;
        }

        if (Length < rhs.Length)
            return -1;

        return Length > rhs.Length ? 1 : 0;
    }

    public int CompareTo(string? rhs)
    {
        if (rhs == null)
            return 1;

        var min = Math.Min(Length, rhs.Length);
        for (var i = 0; i < min; ++i)
        {
            var a = this[i];
            var b = rhs[i];
            if (a != b)
                return a - b;
        }

        if (Length < rhs.Length)
            return -1;

        return Length > rhs.Length ? 1 : 0;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        return obj is IEnumerable<char> chars && chars.SequenceEqual(this);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (((_data.GetHashCode() * 397) ^ Length) * 397) ^ _start;
        }
    }

    // Find the index of the first difference between two string views.
    public int FindFirstDifference(StringView rhs)
    {
        var length = Math.Min(rhs.Length, Length);
        var l      = _start;
        var r      = rhs._start;
        var end    = l + length;
        for (; l != end; ++l, ++r)
        {
            if (_data[l] != rhs._data[r])
                return l - _start;
        }

        return length;
    }

    public bool StartsWith(StringView rhs)
    {
        if (Length < rhs.Length)
            return false;

        var l   = _start;
        var r   = rhs._start;
        var end = r + rhs.Length;
        for (; r != end; ++l, ++r)
        {
            if (_data[l] != rhs._data[r])
                return false;
        }

        return true;
    }

    public bool EndsWith(StringView rhs)
    {
        if (Length < rhs.Length)
            return false;

        var l   = _start + Length - 1;
        var r   = rhs._start + rhs.Length - 1;
        var end = rhs._start - 1;
        for (; r != end; --l, --r)
        {
            if (_data[l] != rhs._data[r])
                return false;
        }

        return true;
    }

    public bool Contains(StringView rhs)
    {
        if (Length < rhs.Length)
            return false;

        for (var i = _start; i < Length; ++i)
        {
            var broken = false;
            if (i + rhs.Length >= rhs.Length)
                return false;

            for (var j = 0; j < rhs.Length; ++j)
            {
                if (_data[i + j] == rhs[j])
                    continue;

                broken = true;
                break;
            }

            if (!broken)
                return true;
        }

        return false;
    }

    public override string ToString()
        => string.Intern(_data.Substring(_start, Length));

    // Split this string view into [0, idx) and [idx, length).
    public (StringView, StringView) Split(int idx)
        => (Substring(0, idx), Substring(idx, Length - idx));

    public StringView Substring(int from, int size)
        => new(_data, _start + from, size);

    public StringView Substring(int from)
        => Substring(from, Length - from);

    public static bool operator ==(StringView lhs, StringView rhs)
        => lhs.Equals(rhs);

    public static bool operator !=(StringView lhs, StringView rhs)
        => !lhs.Equals(rhs);

    // Enumerate all substrings starting at the beginning in order.
    public IEnumerable<StringView> AllPrefixes()
    {
        for (var i = Length - 1; i > 0; --i)
            yield return Substring(0, i);
    }

    public static IEnumerable<StringView> AllPrefixes(string word)
    {
        for (var i = word.Length - 1; i > 0; --i)
            yield return new StringView(word, 0, i);
    }

    public static implicit operator StringView(string word)
        => new(word);

    public static readonly StringView Empty = new(string.Empty);
}
