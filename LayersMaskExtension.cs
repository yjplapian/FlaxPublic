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
            foreach(var value in Index)
                LayersMask.Mask += (uint)value;

            return LayersMask;
        }

        public static LayersMask SetLayer(LayersMask LayersMask, int Index)
        {
            LayersMask.Mask = (uint)Index;
            return LayersMask;
        }
    }
}