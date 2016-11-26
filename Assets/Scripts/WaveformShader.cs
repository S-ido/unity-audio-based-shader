using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(Renderer))]
public class WaveformShader : MonoBehaviour {
	private int backgroundSampleLength = 64;
	private int foregroundSampleLength = 512;
	private AudioSource audioSource;
	private Renderer rend;
	private float[] backgroundSamples;
	private float[] foregroundSamples;
	private Texture2D backgroundTexture;
	private Texture2D foregroundTexture;

	void Start() {
		audioSource = GetComponent<AudioSource>();
		rend = GetComponent<Renderer>();

		backgroundSamples = new float[backgroundSampleLength];
		foregroundSamples = new float[foregroundSampleLength];

		backgroundTexture = new Texture2D(backgroundSampleLength, 1, TextureFormat.RGBA32, false);
		foregroundTexture = new Texture2D(foregroundSampleLength, 1, TextureFormat.RGBA32, false);

		rend.material.SetTexture("_BackgroundTex", backgroundTexture);
		rend.material.SetTexture("_ForegroundTex", foregroundTexture);

		audioSource.Play();
	}

	void Update() {
		//AudioListener.GetSpectrumData(backgroundSamples, 0, FFTWindow.Triangle);

		audioSource.GetOutputData(backgroundSamples, 0);
		for (int i = 0; i < backgroundTexture.width; i++) {
			float db = backgroundSamples[i];
			Color c = new Color(db, db, db);
			backgroundTexture.SetPixel(i, 0, c);
		}
		backgroundTexture.Apply();

		audioSource.GetOutputData(foregroundSamples, 0);
		for (int i = 0; i < foregroundTexture.width; i++) {
			float db = (foregroundSamples[i] + 1f) / 2f;
			Color c = new Color(db, db, db);
			foregroundTexture.SetPixel(i, 1, c);
		}
		foregroundTexture.Apply();
	}
}
