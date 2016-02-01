using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class PropListEditor
{
    // Add a menu item named "Do Something" to MyMenu in the menu bar.
    [MenuItem("+STUFF/Add Projectiles From Selected Folder")]
    static void DoSomething()
    {
        var factory = GameObject.FindObjectOfType<ProjectileFactory>();
        Debug.Log(factory);

        if (factory)
        {
            var list = factory.prefabs;
            list.Clear();
            if (Selection.activeObject != null)
            {
                Object objSelected = Selection.activeObject;

                string sAssetFolderPath = AssetDatabase.GetAssetPath(objSelected);
                // Construct the system path of the asset folder 
                string sDataPath = Application.dataPath;
                string sFolderPath = sDataPath.Substring(0, sDataPath.Length - 6) + sAssetFolderPath;
                // get the system file paths of all the files in the asset folder
                string[] aFilePaths = Directory.GetFiles(sFolderPath);
                // enumerate through the list of files loading the assets they represent and getting their type

                foreach (string sFilePath in aFilePaths)
                {
                    string sAssetPath = sFilePath.Substring(sDataPath.Length - 6);
                    PropProjectile objAsset = (PropProjectile)AssetDatabase.LoadAssetAtPath(sAssetPath, typeof(PropProjectile));
                    if (objAsset)
                    {
                        Debug.Log(objAsset);

                        var colliders = objAsset.GetComponentsInChildren<Collider>();
                        // skip objects with no colliders
                        if (colliders.Length < 1)
                            goto dontadd;

                        var meshColliders = objAsset.GetComponentsInChildren<MeshCollider>();
                        foreach (var collider in meshColliders)
                        {
                            // skip no convex colliders
                            if (!collider.convex)
                                goto dontadd;
                        }

                        list.Add(objAsset);

                        dontadd: { }
                    }
                }
            }
            EditorUtility.SetDirty(factory);
        }
    }
}
