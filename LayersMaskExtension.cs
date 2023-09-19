namespace FlaxEngine
{
    public static class LayersMaskExtension
    {
        public static LayersMask SetLayer(LayersMask LayersMask, int[] index)
        {
            foreach(var value in index)
            {
                LayersMask.Mask += (uint)value;
            }

            return LayersMask;
        }

        public static LayersMask SetLayer(LayersMask layersMask, int index)
        {
            layersMask.Mask = (uint)index;
            return layersMask;
        }
    }
}