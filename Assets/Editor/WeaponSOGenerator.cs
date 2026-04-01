using System.IO;
using UnityEditor;
using UnityEngine;
using Utils.ClassUtility;

public class WeaponSOGenerator : EditorWindow
{
    [MenuItem("Tools/Weapon SO Generator")]
    public static void GenerateSO()
    {
        // JSON 파일 경로
        string jsonPath = Application.dataPath + "/09.Data/JSON/WeaponData.json";

        if (!File.Exists(jsonPath))
        {
            Debug.LogError($"JSON 파일을 찾을 수 없습니다: {jsonPath}");
            return;
        }

        // JSON 데이터 읽기
        string jsonText = File.ReadAllText(jsonPath);
        WeaponDataList dataList = JsonUtility.FromJson<WeaponDataList>(jsonText);

        // SO를 저장할 폴더 생성
        string folderPath = "Assets/08.ScriptableObjects/Weapons";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            if (!AssetDatabase.IsValidFolder("Assets/08.ScriptableObjects"))
                AssetDatabase.CreateFolder("Assets", "Resources");

            AssetDatabase.CreateFolder("Assets/08.ScriptableObjects", "Weapons");
        }

        foreach (var data in dataList.Weapons)
        {
            WeaponData asset = CreateInstance<WeaponData>();

            // 기본 데이터 할당
            asset.weaponName = data.weaponName;
            asset.weaponType = data.weaponType;
            asset.equipTarget = data.equipTarget;
            asset.speed = data.speed;
            asset.range = data.range;
            asset.damage = data.damage;
            asset.fireRate = data.fireRate;
            asset.lifetime = data.lifetime;
            asset.isMelee = data.isMelee;

            // 프리팹 로드 (Assets/03.Prefabs/Projectile/)
            if (!string.IsNullOrEmpty(data.projectilePrefab))
            {
                asset.projectilePrefab = Resources.Load<GameObject>("Projectile/" + data.projectilePrefab);
                if (asset.projectilePrefab == null)
                    Debug.LogWarning($"{data.weaponName}: {data.projectilePrefab} 프리팹을 찾을 수 없습니다.");
            }

            // 애니메이터 로드 (Assets/05.Spine/Weapons/)
            //if (!string.IsNullOrEmpty(data.weaponAnim))
            //{
            //    asset.weaponAnim = Resources.Load<RuntimeAnimatorController>($"05.Spine/Weapons/{data.weaponAnim}");
            //    if (asset.weaponAnim == null)
            //        Debug.LogWarning($"{data.weaponName}: {data.weaponAnim} 애니메이터를 찾을 수 없습니다.");
            //}

            // 파일 저장
            string assetPath = $"Assets/08.ScriptableObjects/Weapons/{data.weaponName}.asset";
            // 동일한 이름의 파일이 있으면 덮어쓰기 위해 생성
            AssetDatabase.CreateAsset(asset, assetPath);
        }

        // 변경사항 저장 및 새로고침
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"<color=green>성공!</color> {dataList.Weapons.Count}개의 무기 에셋이 생성되었습니다.");
    }
}