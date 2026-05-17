using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Tides.Resources
{
    public partial class GatherPointBehaviour : MonoBehaviour
    {
        [SerializeField] public ResourceType ResourceType;
        [SerializeField] private AnimationCurve scaleCurve;
        [SerializeField] private Transform affectedTransform;
        public IResource Resource;
        private int baseAmount;
        public bool HasRemainingResources => Resource.GetAmount() > 0;

        /// <summary>
        /// Only for debug purpose
        /// </summary>
        private void OnDisable()
        {
            Resource.OnAmountChanged -= OnResourceAmountChanged;
            CancelJobs();
        }

        private void OnEnable()
        {
            Initialize(ResourceType, 5);
            float scale = scaleCurve.Evaluate(1);
            transform.localScale = new Vector3(scale, scale, scale);
        }
        public void Initialize(ResourceType resourceType, int amount)
        {
            this.ResourceType = resourceType;

            switch (resourceType)
            {
                case ResourceType.FOOD:
                    Resource = new FoodResource(amount);
                    break;
                case ResourceType.WOOD:
                    Resource = new WoodResource(amount);
                    break;
                default:
                    Resource = new FoodResource(amount);
                    break;
            }
            baseAmount = amount;
            Resource.OnAmountChanged += OnResourceAmountChanged;
        }

        private void OnResourceAmountChanged(int newAmount)
        {
            float scale = scaleCurve.Evaluate(newAmount / baseAmount);
            affectedTransform.localScale = new Vector3(scale, scale, scale);
            //if (newAmount <= 0)
            //{
            //    CancelJobs();
            //}
        }
    }
    public partial class GatherPointBehaviour : IJobProvider
    {
        public int AssignedWorkersCount { get; set; }
        public Vector3 JobLocation { get => transform.position; }

        public void AddJob()
        {
            JobManager.Instance.PendingJobs.Enqueue(new GatherJob(this));
            AssignedWorkersCount++;
        }

        public void CancelJobs()
        {
            List<SurvivorController> survivorControllers = SurvivorsController.Instance.Survivors.Where(s => s.gatherPointBehaviour != null && s.gatherPointBehaviour == this).ToList();
            foreach (SurvivorController survivor in survivorControllers)
            {
                survivor.StopCurrentJob();
            }
            AssignedWorkersCount = 0;
        }

        public void RemoveJob()
        {
            SurvivorController survivorController = SurvivorsController.Instance.Survivors.FirstOrDefault(s => s.gatherPointBehaviour != null && s.gatherPointBehaviour == this);
            if (survivorController == null) return;
            survivorController.StopCurrentJob();
            AssignedWorkersCount--;
        }
    }
    [System.Serializable]
    public enum ResourceType
    {
        NONE = 0,
        FOOD = 1,
        WOOD = 2
    }
}