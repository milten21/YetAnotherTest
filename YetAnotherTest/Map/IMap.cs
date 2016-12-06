using System.Collections.Generic;


namespace YetAnotherTest
{
    interface IMap
    {
        /// <summary>
        /// Gets an IEnumerable of all the Entities on the map.
        /// </summary>
        IEnumerable<Entity> Entities { get; }

        /// <summary>
        /// Gets the height of the map in pixels.
        /// </summary>
        float Height { get; }

        /// <summary>
        /// Gets the width of the map in pixels.
        /// </summary>
        float Width { get; }
    }
}
