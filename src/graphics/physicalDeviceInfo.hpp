#pragma once
#include <string>
#include <vector>
#include <vulkan/vulkan.h>

namespace graphics {

class PhysicalDeviceInfo {
  public:
    PhysicalDeviceInfo(VkPhysicalDevice device);
    const VkPhysicalDevice device;
    const VkPhysicalDeviceProperties props;
    const VkPhysicalDeviceFeatures features;
    const VkPhysicalDeviceMemoryProperties memoryProps;
    const std::vector<VkQueueFamilyProperties> queueFamProps;
    std::string getInfoStr() const;
};

} // namespace graphics
