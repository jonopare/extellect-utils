#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Text;

namespace Extellect.Utilities.Math.Algebra
{
    public class Variable : IEvaluable
    {
        private readonly string _name;
        private double? _value;

        public Variable(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            _name = name;
        }

        public string Name => _name;

        public double Evaluate()
        {
            if (!_value.HasValue)
            {
                throw new UnassignedVariableException(_name);
            }
            return _value.Value;
        }

        public void Assign(double value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
