using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Azurite Objects/LostPages")]
public class LostPages : Item
{
    [Serializable]
    public class Question
    {
        public string question;
        public string correctAnswer;
        [NonSerialized] public string savedAnswer;
    }

    [SerializeField] private LostPagesMenu menuPrefab;

    public List<Question> questions;

    public bool AllCorrect => questions.TrueForAll(q => !string.IsNullOrEmpty(q.savedAnswer) && q.correctAnswer == q.savedAnswer.Trim());
    public override bool Usable => true;

    public override void Use()
    {
        var ui = Instantiate(menuPrefab, UIManager.Instance.FullscreenMenuContainer.transform);
        ui.transform.localPosition = Vector3.zero;
        ui.Open();
    }
}
