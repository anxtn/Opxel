#pragma once
#include "physicalDeviceInfo.hpp"
#include <string>
#include <vector>
#include <vulkan/vulkan.h>

namespace graphics {

struct PhysicalDeviceRequirements {
    bool requireGeometryShader;
    std::vector<std::string> requiredExtensions;
    int64_t minimumVRAMSize;
    VkQueueFlags requiredQueueFlags;
    uint32_t minimumApiVersion;
};

class PhysicalDevicePicker {
  public:
    bool isPhysicalDeviceSuitable(const PhysicalDeviceInfo &deviceInfo) const;
    int evaluateScore(const PhysicalDeviceInfo &deviceInfo) const;
    PhysicalDevicePicker(const PhysicalDeviceRequirements &requirements);

  private:
    const PhysicalDeviceRequirements requirements_;
};

} // namespace graphics
