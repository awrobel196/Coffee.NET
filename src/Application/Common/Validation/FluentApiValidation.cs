using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Exceptions;

namespace Application.Common.Validation
{
    public static class FluentApiValidation
    {

        public static T NotNullOrEmpty<T>(this T obj, string property)
        {
            if (String.IsNullOrEmpty(property))
            {
                throw new ValidateException("The property cannot be null or empty");
            }
            return obj;
        }

        public static T MaxLength<T>(this T obj, string property, int length)
        {
            if (property.Length>=length)
            {
                throw new ValidateException($"Property cannot be longer than {length} characters");
            }
            return obj;
        }

    }
}
