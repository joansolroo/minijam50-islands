﻿using System.Collections.Generic;
using UnityEngine;

public abstract class Buildable : MonoBehaviour
{
    [SerializeField] new SpriteRenderer renderer;
    [SerializeField] Sprite[] buildSprites;
    private List<BuildableStep> steps;

    [SerializeField]
    private Wood wood;

    private BuildableStep currentStep;
    private float progress = 0;
    public float Progress
    {
        get
        {
            return progress;
        }
        set
        {
            progress = value;
            renderer.sprite = buildSprites[(int)((buildSprites.Length-1) * progress)];
        }
    }
    /*public void Upgrade(float woodValue)
    {
        wood.Add(woodValue);

        ActiveCurrentStep();
    }

    private void OnValidate()
    {
        PopulateSteps();

        currentStep = null;
        ActiveCurrentStep();
    }

    private void ActiveCurrentStep()
    {
        foreach (var step in steps)
        {
            if (wood.Value >= step.minValue)
            {
                if (currentStep == null || step.minValue > currentStep.minValue)
                {
                    SetCurrentStep(step);
                }
            }
        }
    }

    private void PopulateSteps()
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
    }

    private void SetCurrentStep(BuildableStep step)
    {
        if (currentStep != null)
        {
            currentStep.gameObject.SetActive(false);
        }

        step.gameObject.SetActive(true);
        currentStep = step;
    }*/
}
