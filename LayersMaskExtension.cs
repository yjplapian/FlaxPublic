using System;

namespace FlaxEngine
{
    public static class LayersMaskExtension
    {
        /// <summary>
        /// Sets a mutlitude layers of the edited LayersMask
        /// </summary>
        /// <param name="LayersMask"> The LayerMask being edited </param>
        /// <param name="Index"> The targeted layers </param>
        /// <returns> A LayersMask with the specified layers checked </returns>
        public static LayersMask SetLayer(LayersMask LayersMask, int[] Index)
        {
            foreach (var Value in Index)
            {
                if (Value > 32 || Value < 1)
                    throw new ArgumentOutOfRangeException($"integer value of {Value} is not accepted. Only use values between 1 ~ 32");

                LayersMask.Mask += (uint)Value;
            }

            return LayersMask;
        }

        /// <summary>
        /// Sets the LayersMask to a single layer
        /// </summary>
        /// <param name="LayersMask"> The LayerMask being edited </param>
        /// <param name="Index"> The targeted layer </param>
        /// <returns> A LayersMask with the specified layer checked </returns>
        public static LayersMask SetLayer(LayersMask LayersMask, int Index)
        {
            if (Index > 32 || Index < 1)
                throw new ArgumentOutOfRangeException($"integer value of {Index} is not accepted. Only use values between 1 ~ 32");

            LayersMask.Mask = (uint)Index;
            return LayersMask;
        }
    }
}