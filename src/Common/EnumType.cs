using System;

namespace CEo
{
    public abstract class EnumType
    {
        protected EnumType(String stringValue) :
            this(stringValue, stringValue.FromSentenceCaseToSnakeCase()) { }
        protected EnumType(String stringValue, String toStringValue) =>
            (StringValue, ToStringValue) = (stringValue, toStringValue);

        protected String StringValue { get; }
        protected String? ToStringValue { get; }
    }
}