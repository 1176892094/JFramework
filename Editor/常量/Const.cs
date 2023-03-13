internal struct Const
{
    /// <summary>
    /// Hierarchy
    /// </summary>
    public const int Int2 = 2;
    public const int Int4 = 4;
    public const int Int8 = 8;
    public const int Int12 = 12;
    public const int Int16 = 16;
    public const int Int32 = 32;
    
    /// <summary>
    /// Inspector
    /// </summary>
    public const string MenuPath = "Tools/JFramework/";
    public const string Inspector = "JFramework Inspector";
    public const string EditorWindow = "JFrameworkEditor";
    
    /// <summary>
    /// Framework
    /// </summary>
    public const string ExcelToScripts = nameof(ExcelToScripts);
    public const string ExcelToAssets = nameof(ExcelToAssets);
    public const string CurrentProjectPath = nameof(CurrentProjectPath);
    public const string PersistentDataPath = nameof(PersistentDataPath);
    public const string StreamingAssetsPath = nameof(StreamingAssetsPath);
    public const string AddressableResources = nameof(AddressableResources);

    /// <summary>
    /// Excel
    /// </summary>
    public const int Name = 2;
    public const int Type = 3;
    public const int Data = 4;
    public const string Namespace = "JFramework.Table";
    public const string ScriptPath = "Assets/Scripts/DataTable/";
    public const string AssetsPath = "Assets/AddressableResources/DataTable/";
    
    /// <summary>
    /// Support
    /// </summary>
    public const string Int = "int";
    public const string Bool = "bool";
    public const string Long = "long";
    public const string Enum = "enum";
    public const string Float = "float";
    public const string Double = "double";
    public const string String = "string";
    public const string Struct = "struct";
    public const string Vector2 = "vector2";
    public const string Vector3 = "vector3";
    public const string EnumField = "enum:field";
    public static readonly string[] Array = { Int, Bool, Long, Float, Double, String, Vector2, Vector3 };
}
