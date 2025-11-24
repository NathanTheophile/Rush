#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using System.Collections.Generic;
using NUnit.Framework.Internal.Execution;
using Rush.Game;
using UnityEngine;

namespace Rush.UI
{
    public class Grid_LevelSelector : MonoBehaviour
    {
        [SerializeField] private Transform _GridRoot;

        [SerializeField] private SO_LevelCollection _LevelCollection;

        [SerializeField] private Item_LevelItem _LevelItemPrefab;

        [SerializeField] private Vector2Int _PreviewResolution = new Vector2Int(512, 512);

        [SerializeField] private float _PreviewOffset = 50f;

        [SerializeField] private Vector3 _PreviewOrigin = new Vector3(10000f, 10000f, 10000f);

        [SerializeField] public Camera _CameraPreview;

        private readonly List<GameObject> _SpawnedLevelInstances = new();

        private void Start() => Populate();

        private void Populate()
        {
            var lLevelCollection = _LevelCollection.levelDatas;

            for (int i = 0; i < lLevelCollection.Count; i++)
            {
                var lCurrentLevel = lLevelCollection[i];
                Vector3 lSpawnPosition = _PreviewOrigin + new Vector3(_PreviewOffset * i, 0f, 0f);
                GameObject lLevelInstance = Instantiate(lCurrentLevel.levelPrefab, lSpawnPosition, Quaternion.identity);
                _SpawnedLevelInstances.Add(lLevelInstance);

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
    }
}