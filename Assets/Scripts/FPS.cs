using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPS : MonoBehaviour
{
	[SerializeField]private Text updateFPS = null;
	[SerializeField]private Text fixedFPS = null;

    [SerializeField] private Text[] DropTexts = new Text[6];
    [SerializeField] private float estimateTime = 1;
    private float lowestFrame; //0
    private float highestFrame; //2
    private float averageFrame; //3
    private float lowDenoisedFrame; //1
    private int denoisedFrames;
    private float frameDelta; //4

    private int longestSpike;
    private int spikeLength;

    private int frameCount;
    private float frameSum;

    [SerializeField] private int Samples = 20;
    private float[] fpsSamples;
    private float actualFps;

    private void Start()
    {
        fpsSamples = new float[20];
    }

    private void Update()
    {
        CalculateUpdate();
        CalculateFixed();
	}

    private void CalculateUpdate()
    {
        fpsSamples[0] = Time.deltaTime;
        System.Array.Copy(fpsSamples, 0, fpsSamples, 1, fpsSamples.Length - 1);
        System.Array.Clear(fpsSamples, 0, 1);

        for (int i = 0; i < Samples; i++)
        {
            actualFps += fpsSamples[i];
        }

        actualFps /= Samples;

        updateFPS.text = 1.0f / actualFps + "";
        if (1.0f / Time.deltaTime > 70.0f)
        {
            updateFPS.color = Color.cyan;
        }
        else
        if (1.0f / Time.deltaTime > 45.0f)
        {
            updateFPS.color = Color.green;
        }
        else
        if (1.0f / Time.deltaTime > 25.0f)
        {
            updateFPS.color = Color.yellow;
        }
        else
        {
            updateFPS.color = Color.red;
        }
    }

    private void CalculateFixed()
    {
        fixedFPS.text = 1.0f / Time.fixedDeltaTime + "";
		if (1.0f / Time.fixedDeltaTime > 70.0f)
		{
			fixedFPS.color = Color.cyan;
		}
		else
		if (1.0f / Time.fixedDeltaTime > 45.0f)
		{
			fixedFPS.color = Color.green;
		}
		else
		if (1.0f / Time.fixedDeltaTime > 25.0f)
		{
			fixedFPS.color = Color.yellow;
		}
		else
		{
			fixedFPS.color = Color.red;
		}
    }

    public void RegisterDrop()
    {
        lowestFrame = 100;
        highestFrame = 0;
        denoisedFrames = 0;
        lowDenoisedFrame = 0;
        spikeLength = 0;
        frameCount = 0;
        frameSum = 0;
        longestSpike = 0;
        StartCoroutine(EstimateLowFrame());
    }

    private IEnumerator EstimateLowFrame()
    {
        float timer = 0;
        while (timer < estimateTime)
        {
            yield return null;
            timer += Time.deltaTime;
            frameSum += 1.0f / Time.deltaTime;
            frameCount++;
            if (1.0f / Time.deltaTime < 50)
            {
                spikeLength++;
                if (spikeLength > longestSpike)
                {
                    longestSpike = spikeLength;
                }
            } else
            {
                spikeLength = 0;
            }
            if (1.0f / Time.deltaTime < lowestFrame)
            {
                lowestFrame = 1.0f/Time.deltaTime;
            }
            if (1.0f/Time.deltaTime > highestFrame)
            {
                highestFrame = 1.0f/Time.deltaTime;
            }
            if (spikeLength > 2)
            {
                lowDenoisedFrame += 1.0f / Time.deltaTime;
                denoisedFrames++;
            }
        }
        if (denoisedFrames > 0)
        {
            lowDenoisedFrame = lowDenoisedFrame / (float)denoisedFrames;
        }
        averageFrame = frameSum / (float)frameCount;
        if (lowDenoisedFrame != 0)
        {
            DropTexts[1].text = lowDenoisedFrame.ToString("f2") + " Low(" + denoisedFrames+","+longestSpike+")";
        } else
        {
            DropTexts[1].text = "No Low";
            lowDenoisedFrame = averageFrame;
        }
        frameDelta = highestFrame - lowDenoisedFrame;
        DropTexts[0].text = lowestFrame.ToString("f2") + " Spike";
        DropTexts[2].text = highestFrame.ToString("f2") + " High";
        DropTexts[3].text = averageFrame.ToString("f2") + " Avg";
        DropTexts[4].text = frameDelta.ToString("f2") + " Delta";
        if (frameDelta < 6)
        {
            DropTexts[5].color = Color.green;
            DropTexts[5].text = "Optimal Delta";
        } else if (frameDelta < 10)
        {
            DropTexts[5].color = Color.yellow;
            DropTexts[5].text = "Moderate Delta";
        } else
        {
            DropTexts[5].color = Color.red;
            DropTexts[5].text = "Critical Delta";
        }
    }
}
