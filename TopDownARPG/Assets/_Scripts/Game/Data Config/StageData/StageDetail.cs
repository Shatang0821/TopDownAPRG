using System;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStageDetail", menuName = "Stage/StageConfig", order = 1)]
public class StageDetail : ScriptableObject
{
    public GameObject StagePrefab;  // �X�e�[�W�I�u�W�F�N�g
    public string DataFilePath;     // �X�e�[�W�f�[�^
    public int[,] Map;              // �X�e�[�W�Q�����f�[�^
    public int MapWidth;            // �}�b�v�̕�
    public int MapHeight;           // �}�b�v�̍���

    public Vector3 SpawnPos;        //�����ʒu
    public float CellSize = 1f;     //�u���b�N�T�C�Y
    /// <summary>
    /// ����������
    /// </summary>
    public void Initialize()
    {
        LoadMapFromCSV(DataFilePath);
    }
    
    /// <summary>
    /// CSV����Map�f�[�^��ǂݎ��
    /// </summary>
    /// <param name="filePath">�p�X</param>
    private void LoadMapFromCSV(string filePath)
    {
        try
        {
            string[] lines = File.ReadAllLines(filePath);
            Map = new int[lines.Length, lines[0].Split(',').Length];

            for (int y = 0; y < lines.Length; y++)
            {
                string[] values = lines[y].Split(',');
                for (int x = 0; x < values.Length; x++)
                {
                    Map[y, x] = int.Parse(values[x]);  // ���̂܂ܓǂݍ���
                }
            }

            MapHeight = Map.GetLength(0);
            MapWidth = Map.GetLength(1);
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading map from CSV: " + e.Message);
        }
    }
}
