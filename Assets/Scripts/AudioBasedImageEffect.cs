using UnityEngine;

public class AudioBasedImageEffect : MonoBehaviour {
	public Shader shader;

	private int backgroundSampleLength = 64;
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

		backgroundTexture = new Texture2D(backgroundSampleLength, 1, TextureFormat.RGBA32, false);
		foregroundTexture = new Texture2D(foregroundSampleLength, 1, TextureFormat.RGBA32, false);

		material.SetTexture("_BackgroundTex", backgroundTexture);
		material.SetTexture("_ForegroundTex", foregroundTexture);

		audioSource.Play();
	}

	// Postprocess the image
	void OnRenderImage(RenderTexture source, RenderTexture destination) {
		AudioListener.GetOutputData(backgroundSamples, 0);
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

		Graphics.Blit(source, destination, material);
	}
}
