using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TestRenderPassFeature : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        public RenderTargetIdentifier source;

        private Material material;
        private RenderTargetHandle tempRenderTargetHandler;

        public CustomRenderPass(Material material)
        {
            this.material = material;

            tempRenderTargetHandler.Init("_TemporaryColorTexture");
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            // Implement camera setup logic if needed
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer commandBuffer = CommandBufferPool.Get("CustomBlitrenderPass");

            commandBuffer.GetTemporaryRT(tempRenderTargetHandler.id, renderingData.cameraData.cameraTargetDescriptor);

            if (material != null)
            {
                Blit(commandBuffer, source, tempRenderTargetHandler.Identifier(), material);
                Blit(commandBuffer, tempRenderTargetHandler.Identifier(), source);
            }
            else
            {
                Debug.LogError("Material is null in CustomRenderPass");
            }

            commandBuffer.ReleaseTemporaryRT(tempRenderTargetHandler.id);

            context.ExecuteCommandBuffer(commandBuffer);
            CommandBufferPool.Release(commandBuffer);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            // Implement cleanup logic if needed
        }
    }

    [System.Serializable]
    public class Settings
    {
        public Material material = null;
    }

    public Settings settings = new Settings();
    CustomRenderPass m_ScriptablePass;

    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass(settings.material);

        m_ScriptablePass.renderPassEvent = RenderPassEvent.AfterRendering;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        m_ScriptablePass.source = renderer.cameraColorTarget;
        renderer.EnqueuePass(m_ScriptablePass);
    }
}


