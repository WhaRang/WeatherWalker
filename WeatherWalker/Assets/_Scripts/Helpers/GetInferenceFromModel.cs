using System;
using System.Linq;
using Unity.Barracuda;
using UnityEngine;

public class GetInferenceFromModel : MonoBehaviour
{
    [Serializable]
    public struct Prediction
    {
        public int predictedValue;
        public float[] predicted;

        public void SetPrediction(Tensor t)
        {
            predicted = t.AsFloats();
            predictedValue = Array.IndexOf(predicted, predicted.Max());
            Debug.Log($"Predicted {predictedValue}");
        }
    }

    [SerializeField] private Prediction prediction;
    [SerializeField] private Texture2D texture;
    [SerializeField] private NNModel modelAsset;

    private Model _runtimeModel;
    private IWorker _engine;

    void Start()
    {
        _runtimeModel = ModelLoader.Load(modelAsset);
        _engine = WorkerFactory.CreateWorker(_runtimeModel, WorkerFactory.Device.GPU);
        prediction = new Prediction();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var channelCount = 1;
            var inputX = new Tensor(texture, channelCount);
            Tensor outputY = _engine.Execute(inputX).PeekOutput();
            prediction.SetPrediction(outputY);
            inputX.Dispose();
        }
    }

    private void OnDestroy()
    {
        _engine?.Dispose();
    }
}