using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BoardGenerator))]
public class BoardEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        BoardGenerator board = target as BoardGenerator;
        board.GenerateBoard();
    }

}
