// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2024-12-24 01:12:35
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

namespace JFramework
{
    public interface IJsonHelper : IBaseHelper
    {
        string ToJson<T>(T data);

        void FromJson<T>(string json, T data);

        T FromJson<T>(string json);
    }
}