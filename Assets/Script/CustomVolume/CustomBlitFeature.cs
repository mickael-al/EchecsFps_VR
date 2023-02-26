using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CustomBlitFeature : ScriptableRendererFeature
{
  [System.Serializable]
  public class CustomFeatureSettings
  {
    public bool IsEnabled = true;
    public RenderPassEvent WhenToInsert = RenderPassEvent.AfterRendering;
    public Material MaterialToBlit;
  }

  public CustomFeatureSettings settings = new CustomFeatureSettings();

  RenderTargetHandle renderTextureHandle;
  CustomBlitRenderPass customRenderPass;

  public override void Create()
  {
    customRenderPass = new CustomBlitRenderPass(
      "Custom pass",
      settings.WhenToInsert,
      settings.MaterialToBlit
    );
  }

  public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
  {
    if (!settings.IsEnabled)
    {
      return;
    }
    
    var cameraColorTargetIdent = renderer.cameraColorTarget;
    customRenderPass.Setup(cameraColorTargetIdent);
    renderer.EnqueuePass(customRenderPass);
  }
}