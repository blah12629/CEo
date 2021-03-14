using System;

namespace CEo.ArithmeticProviders
{
    internal class Int32ArithmeticProvider : IArithmeticProvider<Int32>
    {
        public virtual Int32 Add(Int32 left, Int32 right) => left + right;
        public virtual Int32 Subtract(Int32 left, Int32 right) => left - right;
        public virtual Int32 Multiply(Int32 left, Int32 right) => left * right;
        public virtual Int32 Divide(Int32 left, Int32 right) => left / right;
        public virtual Int32 Negate(Int32 value) => -value;
    }
}