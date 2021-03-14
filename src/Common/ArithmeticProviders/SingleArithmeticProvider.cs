using System;

namespace CEo.ArithmeticProviders {
    internal class SingleArithmeticProvider : IArithmeticProvider<Single>
    {
        public virtual Single Add(Single left, Single right) => left + right;
        public virtual Single Subtract(Single left, Single right) => left - right;
        public virtual Single Multiply(Single left, Single right) => left * right;
        public virtual Single Divide(Single left, Single right) => left / right;
        public virtual Single Negate(Single value) => -value;
    }
}