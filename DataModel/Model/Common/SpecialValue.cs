using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Model.Common
{
    public abstract class ArrayValue<T>
    {
        public ArrayValue(T value)
        {
            Value = value;
        }

        public long Id { get; set; }
        public T Value { get; set; }
    }

    public abstract class SpecialValue
    {
        public SpecialValue()
        {
            ValuesFloat = new List<SpecialFloatValue>();
            ValuesInt = new List<SpecialIntValue>();
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public List<SpecialFloatValue> ValuesFloat { get; set; }
        public List<SpecialIntValue> ValuesInt { get; set; }
        public bool IsPercentage { get; set; }
        public string Heading { get; set; }
    }

    public class SpecialFloatValue : ArrayValue<decimal>
    {
        public SpecialFloatValue(decimal value) : base(value) { }
    }

    public class SpecialIntValue : ArrayValue<int>
    {
        public SpecialIntValue(int value) : base(value) { }
    }
}
