using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngineInternal;
namespace UnityEditor
{
	internal sealed class LightmapVisualizationUtility
	{
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Vector4 GetLightmapTilingOffset(LightmapType lightmapType);
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Texture2D GetGITexture(GITextureType textureType);
		public static void DrawTextureWithUVOverlay(Texture2D texture, GameObject gameObject, Rect drawableArea, Rect position, GITextureType textureType, bool drawSpecularUV)
		{
			LightmapVisualizationUtility.INTERNAL_CALL_DrawTextureWithUVOverlay(texture, gameObject, ref drawableArea, ref position, textureType, drawSpecularUV);
		}
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_DrawTextureWithUVOverlay(Texture2D texture, GameObject gameObject, ref Rect drawableArea, ref Rect position, GITextureType textureType, bool drawSpecularUV);
	}
}
