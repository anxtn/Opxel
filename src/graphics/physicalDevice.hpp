#pragma once
#include <vulkan/vulkan.h>

namespace graphics {
class PhysicalDevice {
  public:
    PhysicalDevice();
    ~PhysicalDevice();

  private:
    VkPhysicalDevice vulkanPhysicalDevice_;
};
} // namespace graphics
