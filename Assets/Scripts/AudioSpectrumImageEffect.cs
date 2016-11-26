using UnityEngine;

public class AudioSpectrumImageEffect : MonoBehaviour {
	public Shader shader;

	private int backgroundSampleLength = 2048;
	private int foregroundSampleLength = 512;
	private AudioSource audioSource;
	private float[] backgroundSamples;
	private float[] foregroundSamples;
	private Texture2D backgroundTexture;
	private Texture2D foregroundTexture;
	private Material material;

	// Creates a private material used to the effect
	void Awake() {
		audioSource = GetComponent<AudioSource>();
		material = new Material(shader);

		audioSource = GetComponent<AudioSource>();

		backgroundSamples = new float[backgroundSampleLength];
		foregroundSamples = new float[foregroundSampleLength];

		backgroundTexture = new Texture2D(backgroundSampleLength / 2, 1, TextureFormat.RGBA32, false);
		foregroundTexture = new Texture2D(foregroundSampleLength, 1, TextureFormat.RGBA32, false);

		material.SetTexture("_BackgroundTex", backgroundTexture);
		material.SetTexture("_ForegroundTex", foregroundTexture);

		audioSource.Play();
	}

	float[] mf = new float[] { 20.0f, 25, 31.5f, 40, 50, 63, 80, 100, 125, 160, 200, 250, 315, 400, 500, 630, 800, 1000, 1250, 1600, 2000, 2500, 3150, 4000, 5000, 6300, 8000, 10000, 12500, 16000, 20000 };

	// Postprocess the image
	void OnRenderImage(RenderTexture source, RenderTexture destination) {
		AudioListener.GetSpectrumData(backgroundSamples, 0, FFTWindow.Rectangular);
		AudioListener.GetOutputData(foregroundSamples, 0);

		////float time = 0f;
		//float min = 1f;
		//float max = 0f;

		//for (int i = 0; i < backgroundTexture.width; i++) {
		//	float db = foregroundSamples[i];
		//	if (db < min)
		//		min = db;

		//	if (db > max)
		//		max = db;
		//}

		for (int i = 0; i < backgroundTexture.width; i++) {
			//int index = (int)((float)i / ((float)backgroundSampleLength*0.5f) * mf.Length);
			float db = backgroundSamples[i] * backgroundSamples[i] * i;
			//if (db < 0.0005f)
			//	db = 0f;
			//Debug.Log(db.ToString("0." + new string('#', 339)));

			//db = 1 / (max - min) * (db - min);
			db = db * 10;
			if (db > 0.9f)
				db = 0.9f;
			//float x = (float)i / (float)backgroundSampleLength;
			//float f = (0.75f + 0.25f * Mathf.Sin(10.0f * i + 13.0f * backgroundSamples[i])) * Mathf.Exp(-3.0f * x);

			//if (i < 3)
			//	f = Mathf.Pow(0.50f + 0.5f * Mathf.Sin(6.2831f * backgroundSamples[i]), 4.0f) * (1.0f - i / 3.0f);

			Color c = new Color(db, db, db);

			backgroundTexture.SetPixel(i, 0, c);
		}
		backgroundTexture.Apply();

		Graphics.Blit(source, destination, material);
	}
}
