using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHC.Common
{
    /// <summary>
    ///	A collection of useful methods for verifying method preconditions.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        ///	Throws an exception if the specified value is null.
        /// </summary>
        /// <param name="value">Value to test for null.</param>
        /// <param name="paramName">
        ///	Name of the method parameter corresponding to <paramref name="value"/>.
        /// </param>
        public static void NotNull(object value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        ///	Throws an exception if the specified value is null.
        /// </summary>
        /// <typeparam name="T">
        ///	The type of the underlying value of <paramref name="value"/>
        /// </typeparam>
        /// <param name="value">Value to test for null.</param>
        /// <param name="paramName">
        ///	Name of the method parameter corresponding to <paramref name="value"/>.
        /// </param>
        public static void NotNull<T>(Nullable<T> value, string paramName)
            where T : struct
        {
            if (!value.HasValue)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        ///	Throws an exception if the specified value is null or an empty string.
        /// </summary>
        /// <param name="value">String to test</param>
        /// <param name="paramName">
        ///	Name of the method parameter corresponding to <paramref name="value"/>.
        /// </param>
        public static void NotNullOrEmpty(string value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }

            if (value == "")
            {
                throw new ArgumentException(String.Format(
                    "Value of parameter '{0}' cannot be an empty string.",
                    paramName),
                    paramName);
            }
        }

        /// <summary>
        ///	Throws an exception if the specified value is null or an empty collection.
        /// </summary>
        /// <param name="value">Collection to test</param>
        /// <param name="paramName">
        ///	Name of the method parameter corresponding to <paramref name="value"/>.
        /// </param>
        public static void NotNullOrEmpty<T>(IEnumerable<T> value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }

            if (!value.Any())
            {
                throw new ArgumentException(String.Format(
                    "Value of parameter '{0}' cannot be an empty collection.",
                    paramName),
                    paramName);
            }
        }

        /// <summary>convenience - returns arg</summary>
        public static T NonNull<T>(T value, string paramName) where T : class
        {
            NotNull(value, paramName);
            return value;
        }

        /// <summary>convenience - returns arg</summary>
        public static T? NonNull<T>(T? value, string paramName) where T : struct
        {
            NotNull(value, paramName);
            return value;
        }
    }
}
