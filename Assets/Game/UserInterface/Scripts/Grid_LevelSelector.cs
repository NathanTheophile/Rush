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
        [SerializeField]
        private Transform _GridRoot;

        [SerializeField]
        private LevelCollection _LevelCollection;

        [SerializeField]
        private GameObject _LevelItemPrefab;

        [SerializeField]
        private Vector2Int _PreviewResolution = new Vector2Int(512, 512);

        [SerializeField]
        private float _PreviewOffset = 25f;

        [SerializeField]
        private Vector3 _PreviewOrigin = new Vector3(10000f, 10000f, 10000f);

        private readonly List<GameObject> _SpawnedLevelInstances = new();

        private void Awake()
        {
            Populate();
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _SpawnedLevelInstances.Count; i++)
            {
                if (_SpawnedLevelInstances[i] != null)
                {
                    Destroy(_SpawnedLevelInstances[i]);
                }
            }

            _SpawnedLevelInstances.Clear();
        }

        private void Populate()
        {
            if (_GridRoot == null)
            {
                Debug.LogWarning("Grid root not assigned", this);
                return;
            }

            if (_LevelCollection == null)
            {
                Debug.LogWarning("Level collection not assigned", this);
                return;
            }

            if (_LevelItemPrefab == null)
            {
                Debug.LogWarning("Level item prefab not assigned", this);
                return;
            }

            var levelPrefabs = _LevelCollection.LevelPrefabs;

            for (int i = 0; i < levelPrefabs.Count; i++)
            {
                var levelPrefab = levelPrefabs[i];
                if (levelPrefab == null)
                {
                    continue;
                }

                Vector3 spawnPosition = _PreviewOrigin + new Vector3(_PreviewOffset * i, 0f, 0f);
                GameObject levelInstance = Instantiate(levelPrefab, spawnPosition, Quaternion.identity);
                _SpawnedLevelInstances.Add(levelInstance);

                var orbitCameras = levelInstance.GetComponentsInChildren<PreviewCamera>(true);
                if (orbitCameras.Length > 0)
                {
                    for (int j = 0; j < orbitCameras.Length; j++)
                    {
                        orbitCameras[j].AddTargetWorldOffset(spawnPosition);
                    }
                }

                Camera previewCamera = null;
                var cameras = levelInstance.GetComponentsInChildren<Camera>(true);
                foreach (var camera in cameras)
                {
                    if (camera.CompareTag("PreviewCamera"))
                    {
                        previewCamera = camera;
                        break;
                    }
                }

                GameObject levelItemInstance = Instantiate(_LevelItemPrefab, _GridRoot);

                if (!levelItemInstance.TryGetComponent<Item_LevelItem>(out var levelItem))
                {
                    Debug.LogWarning("Level item prefab is missing Item_LevelItem component", levelItemInstance);
                    continue;
                }

                levelItem.Initialize(levelPrefab.name, previewCamera, _PreviewResolution);
            }
        }
    }
}