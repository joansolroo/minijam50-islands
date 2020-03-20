using System.Collections.Generic;
using UnityEngine;

public class Buildable : MonoBehaviour
{
    [SerializeField]
    private List<BuildableStep> steps;

    [SerializeField]
    private Wood wood;

    private BuildableStep currentStep;

    public void AddWood(float value)
    {
        wood.Add(value);

        ActiveCurrentStep();
    }

    private void OnValidate()
    {
        steps.Clear();
        foreach (Transform child in transform)
        {
            var step = child.GetComponent<BuildableStep>();
            if (step != null)
            {
                step.gameObject.SetActive(false);
                steps.Add(step);
            }
        }

        currentStep = null;
        ActiveCurrentStep();
    }

    private void ActiveCurrentStep()
    {
        foreach (var step in steps)
        {
            if (wood.Value >= step.minValue)
            {
                if (currentStep == null)
                {
                    SetCurrentStep(step);
                }
                else if (step.minValue > currentStep.minValue)
                {
                    SetCurrentStep(step);
                }
            }
        }
    }

    private void SetCurrentStep(BuildableStep step)
    {
        if (currentStep != null)
        {
            currentStep.gameObject.SetActive(false);
        }

        step.gameObject.SetActive(true);
        currentStep = step;
    }
}
