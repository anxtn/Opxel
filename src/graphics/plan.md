Instance
 └─ DebugMessenger
 └─ Surface
 └─ PhysicalDevice  (Query)
     └─ Device (logical)
         └─ Queues
         └─ MemoryAllocator
         └─ CommandPool
         └─ DescriptorPool
         └─ ShaderModule
         └─ PipelineLayout
         └─ RenderPass
         └─ Swapchain (depends on Surface & Device)
             └─ SwapchainImages
                 └─ ImageViews
         └─ Framebuffers (depend on ImageViews + RenderPass)
         └─ Pipelines (depend on PipelineLayout + RenderPass + ShaderModule)
         └─ Buffers/Images (depend on Device + MemoryAllocator)
         └─ DescriptorSets (depend on DescriptorPool + DescriptorSetLayout)
         └─ SyncPrimitives (Fences, Semaphores)
         └─ CommandBuffers (allocated from CommandPool, reference Pipelines, Buffers)
