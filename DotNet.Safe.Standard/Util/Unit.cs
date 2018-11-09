using System;

namespace DotNet.Safe.Standard
{
    /// <summary>
    /// Unit is used as a type when handling void-consuming or
    /// supplying functions.
    /// Unit only has a single instance, and it's only equal
    /// to itself.
    /// </summary>
    public sealed class Unit : IEquatable<Unit>
    {
        private static readonly Unit _unit = new Unit();

        private Unit() { }

        /// <summary>
        /// Get the active instance of Unit
        /// </summary>
        /// <returns>Unit</returns>
        public static Unit Instance() => _unit;

        /// <summary>
        /// Compares this object with another.
        /// </summary>
        /// <param name="obj">Other object</param>
        /// <returns>Equality result</returns>
        public override bool Equals(object obj) => obj is Unit;

        /// <summary>
        /// Compares this object with another Unit.
        /// </summary>
        /// <param name="other">Other Unit</param>
        /// <returns>True (Unit is always equal to itself)</returns>
        public bool Equals(Unit other) => other != null;

        /// <summary>
        /// Returns a numeric identifier for this object.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => 0;

        /// <summary>
        /// Returns a string representation for this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "[Unit]";
    }
}
