
using System;
using UnityEngine;

namespace GWG.UsoUIElements.Utilities
{
    /// <summary>
    /// A Unity-serializable wrapper for System.Guid that provides globally unique identifier functionality with full Unity Editor and runtime support.
    /// This structure stores GUID data as four 32-bit unsigned integers to ensure proper serialization and Inspector display while maintaining compatibility with standard .NET Guid operations.
    /// </summary>
    /// <remarks>
    /// The standard .NET Guid struct is not serializable by Unity, making it unsuitable for use in MonoBehaviour fields or ScriptableObjects.
    /// This SerializableGuid provides a solution by breaking the GUID into four serializable parts while maintaining full functional compatibility.
    ///
    /// Key features:
    /// - Full Unity serialization support with hidden Inspector fields
    /// - Implicit conversion operators for seamless interoperability with System.Guid
    /// - Hexadecimal string representation for debugging and data exchange
    /// - Complete equality comparison and hashing support
    /// - Static factory methods for creating new GUIDs and parsing from strings
    ///
    /// The internal representation uses four 32-bit unsigned integers that map directly to the 16-byte GUID structure,
    /// ensuring perfect fidelity when converting to and from System.Guid instances.
    /// </remarks>
    [Serializable]
    public struct SerializableGuid : IEquatable<SerializableGuid>
    {
        /// <summary>
        /// The first 32-bit portion of the GUID data.
        /// </summary>
        /// <remarks>
        /// This field stores bytes 0-3 of the GUID as a 32-bit unsigned integer.
        /// It is serialized by Unity but hidden in the Inspector for cleaner display.
        /// </remarks>
        [SerializeField, HideInInspector] public uint Part1;

        /// <summary>
        /// The second 32-bit portion of the GUID data.
        /// </summary>
        /// <remarks>
        /// This field stores bytes 4-7 of the GUID as a 32-bit unsigned integer.
        /// It is serialized by Unity but hidden in the Inspector for cleaner display.
        /// </remarks>
        [SerializeField, HideInInspector] public uint Part2;

        /// <summary>
        /// The third 32-bit portion of the GUID data.
        /// </summary>
        /// <remarks>
        /// This field stores bytes 8-11 of the GUID as a 32-bit unsigned integer.
        /// It is serialized by Unity but hidden in the Inspector for cleaner display.
        /// </remarks>
        [SerializeField, HideInInspector] public uint Part3;

        /// <summary>
        /// The fourth 32-bit portion of the GUID data.
        /// </summary>
        /// <remarks>
        /// This field stores bytes 12-15 of the GUID as a 32-bit unsigned integer.
        /// It is serialized by Unity but hidden in the Inspector for cleaner display.
        /// </remarks>
        [SerializeField, HideInInspector] public uint Part4;

        /// <summary>
        /// Gets a SerializableGuid instance that represents an empty GUID (all zeros).
        /// This is equivalent to System.Guid.Empty but for the serializable version.
        /// </summary>
        /// <value>
        /// A SerializableGuid with all parts set to zero, representing an empty/null GUID.
        /// </value>
        /// <remarks>
        /// This static property provides a standardized way to represent empty or uninitialized GUIDs.
        /// It's commonly used for default values, null checks, and initialization scenarios.
        /// The empty GUID has the hexadecimal representation of "00000000000000000000000000000000".
        /// </remarks>
        public static SerializableGuid Empty => new(0, 0, 0, 0);

        /// <summary>
        /// Initializes a new SerializableGuid instance with the specified four 32-bit unsigned integer parts.
        /// This constructor allows direct specification of the internal GUID representation.
        /// </summary>
        /// <param name="val1">The first 32-bit portion of the GUID data.</param>
        /// <param name="val2">The second 32-bit portion of the GUID data.</param>
        /// <param name="val3">The third 32-bit portion of the GUID data.</param>
        /// <param name="val4">The fourth 32-bit portion of the GUID data.</param>
        /// <remarks>
        /// This constructor is useful for creating SerializableGuids from known values or for testing purposes.
        /// The four parameters represent the complete 128-bit GUID data split into four 32-bit segments.
        /// This is the most direct way to construct a SerializableGuid when you have the raw data components.
        /// </remarks>
        public SerializableGuid(uint val1, uint val2, uint val3, uint val4)
        {
            Part1 = val1;
            Part2 = val2;
            Part3 = val3;
            Part4 = val4;
        }

        /// <summary>
        /// Initializes a new SerializableGuid instance from an existing System.Guid.
        /// This constructor enables conversion from standard .NET GUIDs to the serializable format.
        /// </summary>
        /// <param name="guid">The System.Guid to convert to a SerializableGuid.</param>
        /// <remarks>
        /// This constructor breaks down the 16-byte GUID structure into four 32-bit unsigned integers
        /// for Unity serialization compatibility. The conversion is lossless and maintains perfect fidelity
        /// with the original GUID data. The byte order is preserved to ensure correct reconstruction.
        /// This constructor is commonly used when interfacing with .NET APIs that return System.Guid instances.
        /// </remarks>
        public SerializableGuid(Guid guid)
        {
            byte[] bytes = guid.ToByteArray();
            Part1 = BitConverter.ToUInt32(bytes, 0);
            Part2 = BitConverter.ToUInt32(bytes, 4);
            Part3 = BitConverter.ToUInt32(bytes, 8);
            Part4 = BitConverter.ToUInt32(bytes, 12);
        }

        /// <summary>
        /// Creates a new SerializableGuid with a cryptographically strong random value.
        /// This method is equivalent to System.Guid.NewGuid() but returns a SerializableGuid.
        /// </summary>
        /// <returns>A new SerializableGuid instance initialized with a random, unique value.</returns>
        /// <remarks>
        /// This static factory method provides the standard way to generate new unique identifiers.
        /// It uses the system's cryptographically strong random number generator to ensure uniqueness.
        /// The generated GUID follows the standard format and is suitable for use as database keys,
        /// object identifiers, or any scenario requiring guaranteed uniqueness.
        /// </remarks>
        public static SerializableGuid NewGuid() => Guid.NewGuid().ToSerializableGuid();

        /// <summary>
        /// Creates a SerializableGuid from a 32-character hexadecimal string representation.
        /// This method parses hex strings without delimiters into SerializableGuid instances.
        /// </summary>
        /// <param name="hexString">A 32-character hexadecimal string representing the GUID data.</param>
        /// <returns>A SerializableGuid parsed from the hex string, or Empty if the string is invalid.</returns>
        /// <remarks>
        /// The hex string must be exactly 32 characters long and contain only valid hexadecimal characters (0-9, A-F).
        /// The method does not require or support GUID delimiters (hyphens or braces) - it expects a continuous hex string.
        /// If the input string is not exactly 32 characters, the method returns SerializableGuid.Empty as a safe fallback.
        /// This method is useful for deserializing GUIDs from compact string representations or database storage.
        /// </remarks>
        public static SerializableGuid FromHexString(string hexString)
        {
            if (hexString.Length != 32)
            {
                return Empty;
            }

            return new SerializableGuid
                (
                Convert.ToUInt32(hexString.Substring(0, 8), 16),
                Convert.ToUInt32(hexString.Substring(8, 8), 16),
                Convert.ToUInt32(hexString.Substring(16, 8), 16),
                Convert.ToUInt32(hexString.Substring(24, 8), 16)
                );
        }

        /// <summary>
        /// Converts the SerializableGuid to a 32-character uppercase hexadecimal string representation.
        /// This method provides a compact string format suitable for debugging, logging, or data storage.
        /// </summary>
        /// <returns>A 32-character uppercase hexadecimal string representing the GUID data without delimiters.</returns>
        /// <remarks>
        /// The returned string is always exactly 32 characters long and uses uppercase hexadecimal digits.
        /// Unlike the standard GUID string representation, this format omits hyphens and braces for compactness.
        /// The format is compatible with the FromHexString method for round-trip conversion.
        /// This representation is ideal for scenarios where space efficiency is important or where delimiters are problematic.
        /// </remarks>
        public string ToHexString()
        {
            return $"{Part1:X8}{Part2:X8}{Part3:X8}{Part4:X8}";
        }

        /// <summary>
        /// Converts the SerializableGuid to a standard System.Guid instance.
        /// This method enables interoperability with .NET APIs that expect System.Guid parameters.
        /// </summary>
        /// <returns>A System.Guid instance with identical data to this SerializableGuid.</returns>
        /// <remarks>
        /// The conversion reconstructs the original 16-byte GUID structure from the four 32-bit parts.
        /// This is a lossless conversion that maintains perfect fidelity with the original data.
        /// The resulting System.Guid can be used with any .NET API that requires GUID parameters.
        /// This method is automatically called by the implicit conversion operator for seamless interoperability.
        /// </remarks>
        public Guid ToGuid()
        {
            var bytes = new byte[16];
            BitConverter.GetBytes(Part1).CopyTo(bytes, 0);
            BitConverter.GetBytes(Part2).CopyTo(bytes, 4);
            BitConverter.GetBytes(Part3).CopyTo(bytes, 8);
            BitConverter.GetBytes(Part4).CopyTo(bytes, 12);
            return new Guid(bytes);
        }

        /// <summary>
        /// Implicitly converts a SerializableGuid to a System.Guid.
        /// This operator enables seamless use of SerializableGuid instances where System.Guid is expected.
        /// </summary>
        /// <param name="serializableGuid">The SerializableGuid to convert.</param>
        /// <returns>A System.Guid instance with identical data to the SerializableGuid.</returns>
        /// <remarks>
        /// This implicit conversion operator allows SerializableGuid instances to be used directly
        /// in contexts that expect System.Guid without requiring explicit conversion calls.
        /// The conversion is lossless and maintains perfect data fidelity.
        /// </remarks>
        public static implicit operator Guid(SerializableGuid serializableGuid) => serializableGuid.ToGuid();

        /// <summary>
        /// Implicitly converts a System.Guid to a SerializableGuid.
        /// This operator enables seamless creation of SerializableGuid instances from System.Guid values.
        /// </summary>
        /// <param name="guid">The System.Guid to convert.</param>
        /// <returns>A SerializableGuid instance with identical data to the System.Guid.</returns>
        /// <remarks>
        /// This implicit conversion operator allows System.Guid instances to be automatically converted
        /// to SerializableGuid when assigned or passed as parameters. The conversion is lossless and
        /// maintains perfect data fidelity between the two GUID representations.
        /// </remarks>
        public static implicit operator SerializableGuid(Guid guid) => new SerializableGuid(guid);

        /// <summary>
        /// Determines whether the specified object is equal to this SerializableGuid instance.
        /// This method provides object-level equality comparison with type checking.
        /// </summary>
        /// <param name="obj">The object to compare with this SerializableGuid.</param>
        /// <returns>True if the specified object is a SerializableGuid and is equal to this instance; otherwise, false.</returns>
        /// <remarks>
        /// This override of the Object.Equals method provides type-safe equality comparison.
        /// It first checks if the object is a SerializableGuid, then delegates to the strongly-typed Equals method.
        /// This method is essential for proper behavior in collections, dictionaries, and general object comparison scenarios.
        /// </remarks>
        public override bool Equals(object obj)
        {
            return obj is SerializableGuid guid && this.Equals(guid);
        }

        /// <summary>
        /// Determines whether this SerializableGuid instance is equal to another SerializableGuid.
        /// This method provides strongly-typed equality comparison by comparing all four data parts.
        /// </summary>
        /// <param name="other">The SerializableGuid to compare with this instance.</param>
        /// <returns>True if all four parts of both SerializableGuid instances are equal; otherwise, false.</returns>
        /// <remarks>
        /// This method implements the IEquatable&lt;SerializableGuid&gt; interface for efficient, type-safe comparison.
        /// Equality is determined by comparing all four 32-bit parts for exact matches.
        /// This is more efficient than converting to System.Guid for comparison and provides direct access to the comparison logic.
        /// </remarks>
        public bool Equals(SerializableGuid other)
        {
            return Part1 == other.Part1 && Part2 == other.Part2 && Part3 == other.Part3 && Part4 == other.Part4;
        }

        /// <summary>
        /// Generates a hash code for this SerializableGuid instance based on all four data parts.
        /// This method enables the use of SerializableGuid as dictionary keys and in hash-based collections.
        /// </summary>
        /// <returns>A hash code value calculated from all four parts of the SerializableGuid.</returns>
        /// <remarks>
        /// The hash code is computed using all four 32-bit parts to ensure good distribution and minimize collisions.
        /// This implementation uses the .NET HashCode.Combine method for optimal hash distribution.
        /// Objects that are equal according to Equals will produce the same hash code, satisfying the GetHashCode contract.
        /// This method is essential for proper behavior when using SerializableGuid in HashSet, Dictionary, or other hash-based collections.
        /// </remarks>
        public override int GetHashCode()
        {
            return HashCode.Combine(Part1, Part2, Part3, Part4);
        }

        /// <summary>
        /// Determines whether two SerializableGuid instances are equal.
        /// This operator provides intuitive equality comparison using the == syntax.
        /// </summary>
        /// <param name="left">The first SerializableGuid to compare.</param>
        /// <param name="right">The second SerializableGuid to compare.</param>
        /// <returns>True if both SerializableGuid instances are equal; otherwise, false.</returns>
        /// <remarks>
        /// This operator overload enables natural equality comparison syntax for SerializableGuid instances.
        /// It delegates to the Equals method to ensure consistent comparison behavior across all equality mechanisms.
        /// </remarks>
        public static bool operator ==(SerializableGuid left, SerializableGuid right) => left.Equals(right);

        /// <summary>
        /// Determines whether two SerializableGuid instances are not equal.
        /// This operator provides intuitive inequality comparison using the != syntax.
        /// </summary>
        /// <param name="left">The first SerializableGuid to compare.</param>
        /// <param name="right">The second SerializableGuid to compare.</param>
        /// <returns>True if the SerializableGuid instances are not equal; otherwise, false.</returns>
        /// <remarks>
        /// This operator overload enables natural inequality comparison syntax for SerializableGuid instances.
        /// It implements the logical negation of the equality operator to ensure consistent behavior.
        /// </remarks>
        public static bool operator !=(SerializableGuid left, SerializableGuid right) => !(left == right);
    }
}