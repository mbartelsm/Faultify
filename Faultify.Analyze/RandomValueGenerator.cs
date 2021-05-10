using System;
using System.Linq;
using Faultify.Core.Extensions;
using NLog;

namespace Faultify.Analyze
{
    /// <summary>
    ///     Random value generator that can be used for generating random values for IL-instructions.
    /// </summary>
    public static class RandomValueGenerator
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static Random Rand { get; } = new Random();

        /// <summary>
        ///     Generates a random value for the given field type.
        /// </summary>
        /// <param name="type">the type of the field for which a random value is to be generated</param>
        /// <param name="reference">a il-reference to the field that contains the originalField value</param>
        /// <returns>The random value.</returns>
        public static object GenerateValueForField(Type type, object reference)
        {
            object newRef = null;

            try
            {
                if (type == typeof(bool))
                {
                    newRef = ChangeBoolean(reference);
                    _logger.Trace($"Changing boolean value from {reference} to {newRef}");
                }
                else if (type == typeof(string))
                {
                    newRef = ChangeString();
                    _logger.Trace($"Changing string value from {reference} to {newRef}");
                }
                else if (type == typeof(char))
                {
                    newRef = ChangeChar(reference);
                    _logger.Trace($"Changing char value from {reference} to {newRef}");
                }
                else if (type.IsNumericType())
                {
                    newRef = ChangeNumber(type, reference);
                    _logger.Trace($"Changing numeric value from {reference} to {newRef}");
                }
                else
                {
                    _logger.Warn($"Type {type} is not supported");
                }
            }
            catch (Exception e)
            {
                _logger.Warn(e, "There was probably an error casting a value, defaulting to null");
            }

            return newRef;
        }

        /// <summary>
        ///     Returns a flipped version of the passed boolean.
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        private static object ChangeBoolean(object reference)
        {
            var value = Convert.ToBoolean(reference);
            return Convert.ToInt32(!value);
        }

        /// <summary>
        ///     Generates a new random string.
        /// </summary>
        /// <returns>The random string</returns>
        private static object ChangeString()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var stringChars = new char[32];

            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[Rand.Next(chars.Length)];
            }

            return new string(stringChars);
        }

        /// <summary>
        ///     Generates a new random character.
        /// </summary>
        /// <param name="originalRef">Reference to the orginial char</param>
        /// <returns>The random character</returns>
        private static object ChangeChar(object originalRef)
        { 
            var original = Convert.ToChar(originalRef);
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            
            while (true)
            {
                char generated = chars[Rand.Next(chars.Length)];
                if (original != generated) return generated;
            }
        }


        /// <summary>
        ///     Generates a new value for the given number reference
        /// </summary>
        /// <param name="type">Type of the number</param>
        /// <param name="original">Original number</param>
        /// <returns>The random numeric object</returns>
        private static object ChangeNumber(Type type, object original)
        {
            if (!TypeChecker.IsNumericType(type))
            {
                throw new ArgumentException("The given type is not numeric", nameof(type));
            }
            
            while (true)
            {
                object generated = Convert.ChangeType(Rand.Next(), type);
                if (original != generated) return generated;
            }
        }
    }
}
