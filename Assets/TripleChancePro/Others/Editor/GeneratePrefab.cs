using UnityEditor;
using UnityEngine;
using TripleChanceProTimer;
[CustomEditor(typeof(InstantiatePrefab))]
public class GeneratePrefab : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        InstantiatePrefab instantiatePrefab = (InstantiatePrefab)target;
        if (GUILayout.Button("Generate"))
        {
            GenerateAllPrefab(instantiatePrefab);
        }
        if (GUILayout.Button("Delete"))
        {
            instantiatePrefab.DeleteAllPrefab();
        }
    }
    public void GenerateAllPrefab(InstantiatePrefab instantiatePrefab)
    {

        string s = "00";
        for (int i = instantiatePrefab.intial; i <= instantiatePrefab.final; i++)
        {
            GameObject currentTile = UnityEditor.PrefabUtility.InstantiatePrefab(instantiatePrefab.button_prefab.gameObject, instantiatePrefab.transform) as GameObject;
            if (i < 10)
            {
                s = "00" + i.ToString();
            }
            else if (i < 100)
            {
                s = "0" + i.ToString();
            }
            else
            {
                s = i.ToString();
            }
            ButtonTap b = currentTile.GetComponent<ButtonTap>();
            b.text1.text = s;
            b.text2.text = s;


            if ((i / 10) % 2 == 0)
            {
                instantiatePrefab.ChangeImage(i, b, 0, 1);
            }
            else
            {
                instantiatePrefab.ChangeImage(i, b, 1, 0);
            }

        }

    }
}
