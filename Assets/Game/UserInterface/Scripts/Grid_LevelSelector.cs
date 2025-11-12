#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using System.Collections.Generic;
using Rush.Game;
using UnityEngine;

namespace Rush.UI
{
    public class Grid_LevelSelector : MonoBehaviour
    {
        [SerializeField] private Transform _GridRoot;

        [SerializeField] private LevelCollection _LevelCollection;

        [SerializeField] private GameObject _LevelItemPrefab;

        [SerializeField] private Vector2Int _PreviewResolution = new Vector2Int(512, 512);

        [SerializeField] private float _PreviewOffset = 50f;

        [SerializeField] private Vector3 _PreviewOrigin = new Vector3(10000f, 10000f, 10000f);

        private readonly List<GameObject> _SpawnedLevelInstances = new();

        private void Awake() => Populate();

        private void Populate()
        {
            var levelPrefabs = _LevelCollection.LevelPrefabs;

            for (int i = 0; i < levelPrefabs.Count; i++)
            {
                var levelPrefab = levelPrefabs[i];
                Vector3 spawnPosition = _PreviewOrigin + new Vector3(_PreviewOffset * i, 0f, 0f);
                GameObject levelInstance = Instantiate(levelPrefab, spawnPosition, Quaternion.identity);
                _SpawnedLevelInstances.Add(levelInstance);

                var orbitCameras = levelInstance.GetComponentsInChildren<PreviewCamera>(true);
                for (int j = 0; j < orbitCameras.Length; j++)
                {
                    orbitCameras[j].AddTargetWorldOffset(spawnPosition);
                }

                var previewCamera = GetPreviewCamera(levelInstance);
                var levelItem = Instantiate(_LevelItemPrefab, _GridRoot).GetComponent<Item_LevelItem>();
                levelItem.Initialize(levelPrefab.name, previewCamera, _PreviewResolution);
            }
        }

        private void OnDestroy()
        {
            foreach (var instance in _SpawnedLevelInstances)
            {
                Destroy(instance);
            }

            _SpawnedLevelInstances.Clear();
        }

        private static Camera GetPreviewCamera(GameObject levelInstance)
        {
            var cameras = levelInstance.GetComponentsInChildren<Camera>(true);
            for (int i = 0; i < cameras.Length; i++)
            {
                if (cameras[i].CompareTag("PreviewCamera"))
                {
                    return cameras[i];
                }
            }

            return null;
        }
    }
}