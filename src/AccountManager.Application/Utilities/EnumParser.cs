// <copyright file="EnumParser.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Utilities
{
    using System.Reflection;
    using System.Runtime.Serialization;
    using AccountManager.Domain.Enums;

    /// <summary>
    /// Provides helper methods for safely parsing string values into enum types.
    /// </summary>
    public static class EnumParser
    {
        /// <summary>
        /// Attempts to parse the specified string into an enum value of type <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">
        /// The enum type to parse the value into.
        /// </typeparam>
        /// <param name="value">
        /// The string representation of the enum value.
        /// </param>
        /// <param name="result">
        /// When this method returns, contains the parsed enum value if the parsing succeeded;
        /// otherwise, the default value of <typeparamref name="TEnum"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the value was successfully parsed and represents a defined enum member;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool TryParse<TEnum>(string value, out TEnum result)
            where TEnum : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = default;
                return false;
            }

            return Enum.TryParse(value, ignoreCase: true, out result)
                   && Enum.IsDefined(typeof(TEnum), result);
        }

        /// <summary>
        /// Gets the EnumMember attribute value for an enum constant.
        /// Returns the enum name if EnumMember is not present.
        /// </summary>
        /// <typeparam name="TEnum">
        /// The enum type to retrieve the EnumMember value from.
        /// </typeparam>
        /// <param name="enumValue">
        /// The enum value whose EnumMember attribute value is to be retrieved.
        /// </param>
        /// <returns>
        /// The value of the <see cref="EnumMemberAttribute"/> if present; otherwise, the enum name.
        /// </returns>
        public static string GetEnumMemberValue<TEnum>(TEnum enumValue)
            where TEnum : struct, Enum
        {
            var field = typeof(TEnum).GetField(enumValue.ToString());
            var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
            return attribute?.Value ?? enumValue.ToString();
        }

        /// <summary>
        /// Parses a string value to an enum value by matching the <see cref="EnumMemberAttribute.Value"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The enum type to parse.
        /// </typeparam>
        /// <param name="value">
        /// The string value to match against the <see cref="EnumMemberAttribute.Value"/> of the enum fields.
        /// </param>
        /// <returns>
        /// The enum value whose <see cref="EnumMemberAttribute.Value"/> matches the provided string.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the provided value does not match any <see cref="EnumMemberAttribute"/> on the enum.
        /// </exception>
        public static T ParseFromEnumMember<T>(string value)
            where T : Enum
        {
            foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var attribute = field.GetCustomAttribute<EnumMemberAttribute>();
                if (attribute?.Value == value)
                {
                    return (T)field.GetValue(null)!;
                }
            }

            throw new ArgumentException(
                $"Unknown value '{value}' for enum {typeof(T).Name}",
                nameof(value));
        }

        /// <summary>
        /// Validates whether the integer value corresponds to a defined value in the specified enum type.
        /// </summary>
        /// <typeparam name="TEnum">The enum type to validate against.</typeparam>
        /// <param name="value">The integer value to validate.</param>
        /// <returns>
        /// <c>true</c> if the value is defined in the enum; otherwise, <c>false</c>.
        /// </returns>
        /// <example>
        /// <code>
        /// int statusCode = 2;
        /// bool isValid = statusCode.IsValidEnumValue&lt;OrderStatus&gt;();
        /// // Returns true if OrderStatus has a member with value 2
        /// </code>
        /// </example>
        public static bool IsValidEnumValue<TEnum>(this int value)
            where TEnum : struct, Enum
        {
            return Enum.IsDefined(typeof(TEnum), value);
        }

        /// <summary>
        /// Attempts to convert an integer value to the corresponding enum value of the specified enum type.
        /// </summary>
        /// <typeparam name="TEnum">The enum type to convert to.</typeparam>
        /// <param name="value">The integer value to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the enum value corresponding to the integer value if the conversion succeeded,
        /// or the default value of <typeparamref name="TEnum"/> if the conversion failed.
        /// </param>
        /// <returns>
        /// <c>true</c> if the value was successfully converted to a valid enum value; otherwise, <c>false</c>.
        /// </returns>
        public static bool TryGetEnum<TEnum>(int value, out TEnum result) where TEnum : struct, Enum
        {
            if (Enum.IsDefined(typeof(TEnum), value))
            {
                result = (TEnum)(object)value;
                return true;
            }

            result = default;
            return false;
        }
    }
}
