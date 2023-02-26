using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

class CustomBlitRenderPass : ScriptableRenderPass
{
    string profilerTag;
    Material materialToBlit;
    RenderTargetIdentifier cameraColorTargetIdent;
    RenderTargetHandle tempTexture;

    public CustomBlitRenderPass(string profilerTag, RenderPassEvent renderPassEvent, Material materialToBlit)
    {
        this.profilerTag = profilerTag;
        this.renderPassEvent = renderPassEvent;
        this.materialToBlit = materialToBlit;
        tempTexture.Init("_TempTexture");
    }

    public void Setup(RenderTargetIdentifier cameraColorTargetIdent)
    {
        this.cameraColorTargetIdent = cameraColorTargetIdent;
    }

    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
        cmd.GetTemporaryRT(tempTexture.id, cameraTextureDescriptor);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get(profilerTag);
        cmd.ClearRenderTarget(true, true, Color.clear);
        cmd.Blit(cameraColorTargetIdent, tempTexture.Identifier(), materialToBlit, 0);
        cmd.Blit(tempTexture.Identifier(), cameraColorTargetIdent);
        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }

    public override void FrameCleanup(CommandBuffer cmd)
    {
        cmd.ReleaseTemporaryRT(tempTexture.id);
    }
}