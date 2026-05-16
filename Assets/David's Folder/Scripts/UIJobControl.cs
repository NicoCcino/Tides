using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIJobControl : MonoBehaviour
{
    [SerializeField] private JobableBehaviour jobableBehaviour;
    [SerializeField] private Button btnAddWorker;
    [SerializeField] private Button btnRemoveWorker;
    [SerializeField] private Button btnCancel;
    [SerializeField] private TMP_Text textWorkerAmount;

    private void OnEnable()
    {
        btnAddWorker.onClick.AddListener(OnBtnAddWorkerClicked);
        btnRemoveWorker.onClick.AddListener(OnBtnRemoveWorkerClicked);
        btnCancel.onClick.AddListener(OnBtnCancelClicked);
    }
    private void OnDisable()
    {
        btnAddWorker.onClick.RemoveListener(OnBtnAddWorkerClicked);
        btnRemoveWorker.onClick.RemoveListener(OnBtnRemoveWorkerClicked);
        btnCancel.onClick.RemoveListener(OnBtnCancelClicked);
    }

    private void FixedUpdate()
    {
        int idleSurvivorsCount = SurvivorsController.Instance.GetIdleSurvivors().Count;
        btnAddWorker.interactable = idleSurvivorsCount > 0;
        int workerCount = jobableBehaviour.Jobable.AssignedWorkersCount;
        btnRemoveWorker.interactable = workerCount > 0;
        textWorkerAmount.text = workerCount.ToString();
    }
    private void OnBtnAddWorkerClicked()
    {
        jobableBehaviour.Jobable.AddJob();
    }
    private void OnBtnRemoveWorkerClicked()
    {
        jobableBehaviour.Jobable.RemoveJob();
    }
    private void OnBtnCancelClicked()
    {
        jobableBehaviour.Jobable.CancelJobs();
        jobableBehaviour.JobableControlsGameObject.SetActive(false);
    }

}
