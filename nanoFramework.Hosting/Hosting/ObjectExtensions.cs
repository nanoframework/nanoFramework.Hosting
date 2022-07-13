namespace nanoFramework.Hosting
{
    /// <summary>
    /// Contains extension methods for <see cref="object"/>.
    /// </summary>
    internal static class ObjectExtensions
    {
        /// <summary>
        /// Reverses the sequence of the elements in the one-dimensional <see cref="object"/> array.
        /// </summary>
        /// <param name="source">The one-dimensional <see cref="object"/> array to reverse.</param>
        internal static object[] Reverse(this object[] source)
        {
            object temp;
            for (int i = 0; i < source.Length / 2; i++)
            {
                temp = source[i];
                source[i] = source[source.Length - i - 1];
                source[source.Length - i - 1] = temp;
            }

            return source;
        }
    }
}
