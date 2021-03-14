using System;

namespace CEo.ArithmeticProviders {
    internal class DoubleArithmeticProvider : IArithmeticProvider<Double>
    {
        public virtual Double Add(Double left, Double right) => left + right;
        public virtual Double Subtract(Double left, Double right) => left - right;
        public virtual Double Multiply(Double left, Double right) => left * right;
        public virtual Double Divide(Double left, Double right) => left / right;
        public virtual Double Negate(Double value) => -value;
    }
}