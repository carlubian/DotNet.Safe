using System;

namespace DotNet.Safe
{
    /// <summary>
    /// Unit is used to represent a void type in
    /// functional contexts.
    /// </summary>
    public sealed class Unit : IComparable, IEquatable<Unit>
    {
        /// <summary>
        /// Gets an instance of Unit.
        /// </summary>
        public static Unit Value { get; } = new Unit();

        /// <summary>
        /// Compares Unit to another object. This method
        /// always returns 0 when comparing Unit to itself,
        /// or -1 when comparing to anything else.
        /// </summary>
        /// <param name="obj">Other object</param>
        /// <returns></returns>
        public int CompareTo(object obj) => obj is Unit ? 0 : -1;
        /// <summary>
        /// Equals Unit with itself. Always returns true
        /// unless null is passed as parameter.
        /// </summary>
        /// <param name="other">Unit</param>
        /// <returns>Result</returns>
        public bool Equals(Unit other) => other != null;
        /// <summary>
        /// Equals Unit with another object. Returns true
        /// only if the object is also Unit.
        /// </summary>
        /// <param name="obj">Other object</param>
        /// <returns>Result</returns>
        public override bool Equals(object obj) => obj is Unit;
        /// <summary>
        /// Gets the HashCode for Unit. This method returns
        /// the constant value 0.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => 0;
        /// <summary>
        /// Returns a string text for the Unit class.
        /// </summary>
        /// <returns>The string "[Unit]"</returns>
        public override string ToString() => "[Unit]";
    }
}
