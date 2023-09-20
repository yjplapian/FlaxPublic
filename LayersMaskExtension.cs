using FlaxEditor.Content.Settings;
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
        public static LayersMask SetLayers(LayersMask LayersMask, int[] Index)
        {
            foreach (var Value in Index)
            {
                if (Value > 32 || Value < 0)
                    throw new ArgumentOutOfRangeException($"integer value of {Value} is not accepted. Only use values between 0 ~ 31");

                LayersMask.Mask += (uint)Value;
            }

            return LayersMask;
        }

        /// <summary>
        /// Adds A single layer to the layers mask
        /// </summary>
        public static LayersMask AddLayer(LayersMask LayersMask, int Index)
        {
            if (Index > 32 || Index < 0)
                throw new ArgumentOutOfRangeException($"integer value of {Index} is not accepted. Only use values between 0 ~ 31");

            LayersMask.Mask += (uint)Index;
            return LayersMask;
        }

        /// <summary>
        /// Removes a layer at the index provided
        /// </summary>
        public static LayersMask RemoveLayer(LayersMask LayersMask, int Index)
        {
            if (Index > 32 || Index < 0)
                throw new ArgumentOutOfRangeException($"integer value of {Index} is not accepted. Only use values between 0 ~ 31");

            LayersMask.Mask -= (uint)Index;
            return LayersMask;
        }

        /// <summary>
        /// Removes an array of layers at the indexes provided
        /// </summary>
        public static LayersMask RemoveLayers(LayersMask LayersMask, int[] Index)
        {
            foreach(int Value in Index) 
            {
                if (Value > 32 || Value < 0)
                    throw new ArgumentOutOfRangeException($"integer value of {Value} is not accepted. Only use values between 0 ~ 31");

                LayersMask.Mask -= (uint)Value; 
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
            if (Index > 32 || Index < 0)
                throw new ArgumentOutOfRangeException($"integer value of {Index} is not accepted. Only use values between 0 ~ 31");

            LayersMask.Mask = (uint)Index;
            return LayersMask;
        }

        /// <summary>
        /// Validates whether the compared index layer and the layers in the LayersMask have collisions between them enabled in the Physics Matrix
        /// </summary>
        /// <param name="LayerMask"> The targeted LayersMask in the Matrix </param>
        /// <param name="Layer"> The layer that is being compared to </param>
        /// <returns> True: if the value </returns>
        public static bool GetValidCollisionLayer(int Layer, uint LayerMask)
        {
            if (Layer < 0 || Layer > 31)
                throw new ArgumentOutOfRangeException($"argument of {Layer} is not accepted. Use an index value of 0 ~ 31");

            Debug.Log($"{(LayerMask & (1 << Layer)) != 0}");
            return (LayerMask & (1 << Layer)) != 0;
        }

        //TODO: Finish writing docs
        /// <summary>
        /// Sets the collision between two layers in the Matrix Collision layers in the PhysicsSettings directly
        /// Warning! :: Preview Function. Is buggy at the moment
        /// </summary>
        /// <param name="MatrixLayer"> The targeted matrix layer </param>
        /// <param name="Layer"> The layer it needs to set </param>
        /// <param name="Ignore"> Whether to toggle the collision between layers on or off </param>
        public static void SetMatrixCollisionLayer(int MatrixLayer, int Layer, bool Ignore)
        {
            if (MatrixLayer < 0 || MatrixLayer > 31)
                throw new ArgumentOutOfRangeException($"argument of {MatrixLayer} is not accepted. Use an index value of 0 ~ 31");

            if (Layer < 0 || Layer > 31)
                throw new ArgumentOutOfRangeException($"argument of {Layer} is not accepted. Use an index value of 0 ~ 31");

            var Settings = GameSettings.Load<PhysicsSettings>();
            uint TargetedMatrixLayer;

            if (!Ignore)
            {
                TargetedMatrixLayer = ((uint)1 << Layer);
                Settings.LayerMasks[MatrixLayer] |= TargetedMatrixLayer;
            }

            if(Ignore)
            {
                TargetedMatrixLayer = ((uint)1 << Layer);
                Settings.LayerMasks[MatrixLayer] &= ~TargetedMatrixLayer;
            }
        }
    }
}