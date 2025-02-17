using System.Collections;

namespace JurassicParkCore.Extensions;

public static class RangeEnumeratorExtensions
{
    public static IEnumerator GetEnumerator(this Range range)
    {
        return new RangeEnumerator(range);
    }
}

public class RangeEnumerator : IEnumerator
{
    private readonly Range _range;
    private readonly bool _isFromEnd;
    
    private int _index;

    public RangeEnumerator(Range range)
    {
        _range = range;
        _isFromEnd = _range.Start.Value >= range.End.Value;

        if (_isFromEnd)
        {
            _index = range.Start.Value + 1;
        }
        else
        {
            _index = range.Start.Value - 1;
        }
    }

    public bool MoveNext()
    {
        if (_isFromEnd)
        {
            _index--;
            return _index >= _range.End.Value;
        }
            
        _index++;
        return _index <= _range.End.Value;
    }

    public void Reset()
    {
        if (_isFromEnd)
        {
            _index = _range.Start.Value + 1;
        }
        else
        {
            _index = _range.Start.Value - 1;
        }
    }

    public object Current => _index;
}
