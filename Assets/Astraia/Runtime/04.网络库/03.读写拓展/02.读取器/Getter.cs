// *********************************************************************************
// # Project: Astraia
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-22 19:12:21
// # Recently: 2024-12-22 20:12:59
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using UnityEngine;

namespace Astraia.Net
{
    public static partial class Extensions
    {
        public static Vector2 GetVector2(this MemoryGetter getter)
        {
            return getter.Get<Vector2>();
        }

        public static Vector2? GetVector2Null(this MemoryGetter getter)
        {
            return getter.Getable<Vector2>();
        }

        public static Vector3 GetVector3(this MemoryGetter getter)
        {
            return getter.Get<Vector3>();
        }

        public static Vector3? GetVector3Null(this MemoryGetter getter)
        {
            return getter.Getable<Vector3>();
        }

        public static Vector4 GetVector4(this MemoryGetter getter)
        {
            return getter.Get<Vector4>();
        }

        public static Vector4? GetVector4Null(this MemoryGetter getter)
        {
            return getter.Getable<Vector4>();
        }

        public static Vector2Int GetVector2Int(this MemoryGetter getter)
        {
            return getter.Get<Vector2Int>();
        }

        public static Vector2Int? GetVector2IntNull(this MemoryGetter getter)
        {
            return getter.Getable<Vector2Int>();
        }

        public static Vector3Int GetVector3Int(this MemoryGetter getter)
        {
            return getter.Get<Vector3Int>();
        }

        public static Vector3Int? GetVector3IntNull(this MemoryGetter getter)
        {
            return getter.Getable<Vector3Int>();
        }

        public static Quaternion GetQuaternion(this MemoryGetter getter)
        {
            return getter.Get<Quaternion>();
        }

        public static Quaternion? GetQuaternionNull(this MemoryGetter getter)
        {
            return getter.Getable<Quaternion>();
        }

        public static Color GetColor(this MemoryGetter getter)
        {
            return getter.Get<Color>();
        }

        public static Color32 GetColor32(this MemoryGetter getter)
        {
            return getter.Get<Color32>();
        }

        public static Rect GetRect(this MemoryGetter getter)
        {
            return new Rect(getter.GetVector2(), getter.GetVector2());
        }

        public static Plane GetPlane(this MemoryGetter getter)
        {
            return new Plane(getter.GetVector3(), getter.GetFloat());
        }

        public static Ray GetRay(this MemoryGetter getter)
        {
            return new Ray(getter.GetVector3(), getter.GetVector3());
        }

        public static Matrix4x4 GetMatrix4x4(this MemoryGetter getter)
        {
            return getter.Get<Matrix4x4>();
        }

        public static NetworkObject GetNetworkObject(this MemoryGetter getter)
        {
            var objectId = getter.GetUInt();
            return objectId != 0 ? NetworkManager.GetNetworkObject(objectId) : null;
        }

        public static NetworkBehaviour GetNetworkBehaviour(this MemoryGetter getter)
        {
            var @object = getter.GetNetworkObject();
            return @object != null ? @object.entities[getter.GetByte()] : null;
        }

        public static Transform GetTransform(this MemoryGetter getter)
        {
            var @object = getter.GetNetworkObject();
            return @object != null ? @object.transform : null;
        }

        public static GameObject GetGameObject(this MemoryGetter getter)
        {
            var @object = getter.GetNetworkObject();
            return @object != null ? @object.gameObject : null;
        }

        public static Texture2D GetTexture2D(this MemoryGetter getter)
        {
            var width = getter.GetShort();
            if (width < 0)
            {
                return null;
            }

            var height = getter.GetShort();
            var texture = new Texture2D(width, height);
            var pixels = getter.GetArray<Color32>();
            texture.SetPixels32(pixels);
            texture.Apply();
            return texture;
        }

        public static Sprite GetSprite(this MemoryGetter getter)
        {
            var texture = getter.GetTexture2D();
            return texture == null ? null : Sprite.Create(texture, getter.GetRect(), getter.GetVector2());
        }

        public static T GetNetworkBehaviour<T>(this MemoryGetter getter) where T : NetworkBehaviour
        {
            return getter.GetNetworkBehaviour() as T;
        }

        public static NetworkVariable GetNetworkVariable(this MemoryGetter getter)
        {
            var objectId = getter.GetUInt();
            byte componentId = default;

            if (objectId != 0)
            {
                componentId = getter.GetByte();
            }

            return new NetworkVariable(objectId, componentId);
        }
    }
}