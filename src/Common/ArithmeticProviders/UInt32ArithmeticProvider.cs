using System;

namespace CEo.ArithmeticProviders
{
    internal class UInt32ArithmeticProvider : IArithmeticProvider<UInt32>
    {
        public virtual UInt32 Add(UInt32 left, UInt32 right) => left + right;
        public virtual UInt32 Subtract(UInt32 left, UInt32 right) => left - right;
        public virtual UInt32 Multiply(UInt32 left, UInt32 right) => left * right;
        public virtual UInt32 Divide(UInt32 left, UInt32 right) => left / right;
        public virtual UInt32 Negate(UInt32 value) => UInt32.MaxValue - value;
    }
}