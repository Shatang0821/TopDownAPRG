    using System;
    using System.Collections.Generic;
    using System.IO;
    using FrameWork.Utils;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewStageDetail", menuName = "Stage/StageConfig", order = 1)]
    public class LevelData : ScriptableObject
    {
        public List<WaveConfig> WaveConfigs;  // �X�e�[�W�̓G�E�F�[�u
        public GameObject StagePrefab; // �X�e�[�W�I�u�W�F�N�g
        public string DataFilePath; // �X�e�[�W�f�[�^
        public int[,] Map; // �X�e�[�W�Q�����f�[�^
        public int MapWidth; // �}�b�v�̕�
        public int MapHeight; // �}�b�v�̍���

        public Vector3 SpawnPos; //�����ʒu
        public float CellSize = 1f; //�u���b�N�T�C�Y

        // Y����-135�x��]�����ăI�u�W�F�N�g�𐶐�
        public Vector3 rotation;
        
        public Vector3 PlayerSpawnPos; //�v���C���[�����ʒu
        
        /// <summary>
        /// ����������
        /// </summary>
        public void Initialize()
        {
            LoadMapFromCSV(DataFilePath);
        }

        
        /// <summary>
        /// ���x���̐���
        /// </summary>
        /// <returns></returns>
        public GameObject SpawnLevel()
        {
            Quaternion ro = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
            var go = Instantiate(StagePrefab, SpawnPos, ro);
            if (go != null)
            {
                return go;
            }
            else
            {
                DebugLogger.Log("�X�e�[�W�̐����Ɏ��s���܂���");
                return null;
            }
        }

        /// <summary>
        /// CSV����Map�f�[�^��ǂݎ��
        /// </summary>
        /// <param name="filePath">�p�X</param>
        private void LoadMapFromCSV(string filePath)
        {
            try
            {
                TextAsset csvFile = Resources.Load<TextAsset>(DataFilePath);
                if (csvFile == null)
                {
                    Debug.LogError("CSV�t�@�C����������܂���: " + DataFilePath);
                    return;
                }
                
                string[] lines = csvFile.text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length > 0)
                {
                    Map = new int[lines.Length, lines[0].Split(',').Length];

                    for (int y = 0; y < lines.Length; y++)
                    {
                        string[] values = lines[y].Split(',');
                        for (int x = 0; x < values.Length; x++)
                        {
                            if (int.TryParse(values[x], out int result))
                            {
                                Map[y, x] = result;
                            }
                            else
                            {
                                Debug.LogError($"Invalid format for map value at ({y}, {x}): {values[x]}");
                                // Optionally, set a default value or handle the error as needed
                            }
                        }
                    }

                    MapHeight = Map.GetLength(0);
                    MapWidth = Map.GetLength(1);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error loading map from CSV: " + e.Message);
            }
        }
    }
    
